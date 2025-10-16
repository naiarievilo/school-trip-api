using System.Globalization;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using SchoolTripApi.Application.Agreements.Abstractions;
using SchoolTripApi.Application.Agreements.DTOs;
using SchoolTripApi.Application.Agreements.Enums;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Infrastructure.FileStorage;
using SchoolTripApi.Infrastructure.WebScraping.Abstractions;
using SchoolTripApi.Infrastructure.WebScraping.Errors;
using SchoolTripApi.Infrastructure.WebScraping.Settings;

namespace SchoolTripApi.Infrastructure.WebScraping.Services;

public class SignatureValidator(
    IOptions<SignatureValidatorSettings> signatureValidationOptions,
    IAppLogger<SignatureValidator> logger,
    IBrowserService<IBrowser, IPage> browserService,
    IFileStore fileStore
) : ISignatureValidator
{
    private const string PdfExtension = ".pdf";
    private const string DownloadExtension = ".crdownload";
    private const string ReportPrefix = "Relatorio - ";

    private readonly SignatureValidatorSettings _settings = signatureValidationOptions.Value;

    public async Task<Result<SignatureValidationResult>> ValidateFileSignatureAsync(byte[] file, string fileName)
    {
        try
        {
            logger.LogInformation("Starting PDF validation for file '{fileName}'...", fileName);

            var getPage = await browserService.GetPageAsync();
            if (getPage.Failed) return Result.Failure<SignatureValidationResult>(getPage.Error);
            var page = getPage.Value;

            await page.GoToAsync(_settings.ValidatorUrl,
                PageNavigationOptions(WaitUntilNavigation.Networkidle0, 30000));
            await page.WaitForSelectorAsync("#form", WaitForSelector(10000));
            await page.ClickAsync("#acceptTerms");

            var fileInput = await page.QuerySelectorAsync("#signature_files");
            if (fileInput is null)
                throw new Exception($"Couldn't find input to upload file '{fileName}'.");

            await fileStore.SaveFileAsync(file, fileName);
            var filePath = fileStore.GetFilePath(fileName);

            await fileInput.UploadFileAsync(filePath);
            await page.WaitForFunctionAsync(@"
                document.querySelector('#validateSignature') &&
                !document.querySelector('#validateSignature').disabled"
            );

            await page.ClickAsync("#validateSignature");

            var signatureValidationTimeout = _settings.ValidationTimeout;
            var navigationTask = page.WaitForNavigationAsync(PageNavigationOptions(WaitUntilNavigation.Networkidle0,
                signatureValidationTimeout));

            await page.WaitForFunctionAsync(@"
                !document.querySelector('.loading:not([style*=""display: none""])') &&
                !document.querySelector('#loadingNew:not([style*=""display: none""])')
            ", WaitForNavigation(signatureValidationTimeout));

            await navigationTask;

            var documentElement = await page.QuerySelectorAsync("#documento");
            if (documentElement is not null) return await ExtractSignatureValidationResultAsync(page);

            var errorElement = await page.QuerySelectorAsync("#swal2-html-container");
            if (errorElement is null)
                throw new Exception("Document was not accepted, but error message couldn't be retrieved.");


            var errorMessage =
                await errorElement.EvaluateFunctionAsync<string>("element => element.textContent?.trim() || ''");
            return string.IsNullOrEmpty(errorMessage)
                ? Result.Failure<SignatureValidationResult>(SignatureValidationError.ValidationFailed(errorMessage))
                : Result.Failure<SignatureValidationResult>(
                    SignatureValidationError.ValidationFailed(
                        "Document doesn't have recognizable signature or signature is corrupted."));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating PDF file '{fileName}': {errorMessage}", fileName, ex.Message);
            return Result.Failure<SignatureValidationResult>(SignatureValidationError.ValidationFailed(fileName));
        }
    }

    private async Task<Result<SignatureValidationResult>> ExtractSignatureValidationResultAsync(IPage page)
    {
        var extractFileValidationInfo = await ExtractFileValidationInfoAsync(page);
        if (extractFileValidationInfo.Failed)
            return Result.Failure<SignatureValidationResult>(extractFileValidationInfo.Error);

        var extractSignatureInfo = await ExtractSignatureInfoAsync(page);
        if (extractSignatureInfo.Failed)
            return Result.Failure<SignatureValidationResult>(extractSignatureInfo.Error);

        var extractValidationReport = await ExtractValidationReportAsync(page);
        if (extractValidationReport.Failed)
            return Result.Failure<SignatureValidationResult>(extractValidationReport.Error);

        return Result.Success(new SignatureValidationResult
        {
            FileInfo = extractFileValidationInfo.Value,
            SignatureInfo = extractSignatureInfo.Value,
            ValidationReportPdf = extractValidationReport.Value
        });
    }

    private async Task<Result<SignatureInfo>> ExtractSignatureInfoAsync(IPage page)
    {
        var extractFileSignee = await ExtractFileSigneeAsync(page);
        if (extractFileSignee.Failed) return Result.Failure<SignatureInfo>(extractFileSignee.Error);

        var extractMaskedCpf = await ExtractMaskedCpfAsync(page);
        if (extractMaskedCpf.Failed) return Result.Failure<SignatureInfo>(extractMaskedCpf.Error);

        var extractIssuingCertificateSerialNumber = await ExtractIssuerCertificateSerialNumber(page);
        if (extractIssuingCertificateSerialNumber.Failed)
            return Result.Failure<SignatureInfo>(extractIssuingCertificateSerialNumber.Error);

        var extractSignedDate = await ExtractSignedDateAsync(page);
        if (extractSignedDate.Failed) return Result.Failure<SignatureInfo>(extractSignedDate.Error);

        var extractSignatureStatus = await ExtractSignatureStatusAsync(page);
        if (extractSignatureStatus.Failed) return Result.Failure<SignatureInfo>(extractSignatureStatus.Error);

        return Result.Success(new SignatureInfo
        {
            SignedBy = extractFileSignee.Value,
            MaskedCpf = extractMaskedCpf.Value,
            IssuingCertificateSerialNumber = extractIssuingCertificateSerialNumber.Value,
            SignedDate = extractSignedDate.Value,
            Status = extractSignatureStatus.Value
        });
    }

    private async Task<Result<string>> ExtractFileSigneeAsync(IPage page)
    {
        var signedBy = await page.EvaluateExpressionAsync<string>(@"
            document.querySelector('.assinadoPor')?.nextElementSibling?.textContent?.trim() || '';
        ");

        if (!string.IsNullOrEmpty(signedBy)) return Result.Success(signedBy);

        const string errorMessage = "Couldn't extract file signee.";
        logger.LogError(errorMessage);
        return Result.Failure<string>(SignatureValidationError.ExtractSignatureInfoFailed(errorMessage));
    }

    private async Task<Result<string>> ExtractMaskedCpfAsync(IPage page)
    {
        var maskedCpf = await page.EvaluateFunctionAsync<string>(@"
            document.querySelector('.identificador')?.nextElementSibling?.textContent?.trim() || '';
        ");

        if (!string.IsNullOrEmpty(maskedCpf)) return Result.Success(maskedCpf);

        const string errorMessage = "Couldn't extract signee's CPF.";
        logger.LogError(errorMessage);
        return Result.Failure<string>(SignatureValidationError.ExtractSignatureInfoFailed(errorMessage));
    }

    private async Task<Result<string>> ExtractIssuerCertificateSerialNumber(IPage page)
    {
        var issuerCertificateSerialNumber = await page.EvaluateFunctionAsync<string>(@"
            document.querySelector('.numserie')?.nextElementSibling?.textContent?.trim() || '';
        ");

        if (!string.IsNullOrEmpty(issuerCertificateSerialNumber)) return Result.Success(issuerCertificateSerialNumber);

        const string errorMessage = "Couldn't extract issuer certificate serial number.";
        logger.LogError(errorMessage);
        return Result.Failure<string>(SignatureValidationError.ExtractSignatureInfoFailed(errorMessage));
    }

    private async Task<Result<DateTimeOffset>> ExtractSignedDateAsync(IPage page)
    {
        var signedDate = await page.EvaluateFunctionAsync<string>(@"
                document.querySelector('.dataDaValidacao')?.nextElementSibling?.textContent?.trim() || '';
            ");

        if (string.IsNullOrEmpty(signedDate))
        {
            const string errorMessage = "Signed date is null or empty.";
            logger.LogError(errorMessage);
            return Result.Failure<DateTimeOffset>(SignatureValidationError.ExtractSignatureInfoFailed(errorMessage));
        }

        try
        {
            var parsedSignedDate = DateTimeOffset.ParseExact(
                signedDate,
                "dd/MM/yyyy hh:mm:ss zzz",
                CultureInfo.InvariantCulture
            );

            return Result.Success(parsedSignedDate);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Couldn't parse signed date: {errorMessage}", ex.Message);
            return Result.Failure<DateTimeOffset>(SignatureValidationError
                .ExtractSignatureInfoFailed("Couldn't extract file signed date."));
        }
    }

    private async Task<Result<SignatureStatus>> ExtractSignatureStatusAsync(IPage page)
    {
        var signatureStatus = await page.EvaluateExpressionAsync<string>(@"
            document.querySelector('.frase')?.textContent?.trim() || '';
        ");

        if (!string.IsNullOrEmpty(signatureStatus))
            return signatureStatus.Contains("aprovada", StringComparison.InvariantCultureIgnoreCase)
                ? Result.Success(SignatureStatus.Approved)
                : Result.Success(SignatureStatus.Rejected);

        const string errorMessage = "Signature status message is null or empty.";
        logger.LogError(errorMessage);
        return Result.Failure<SignatureStatus>(SignatureValidationError.ExtractSignatureInfoFailed(errorMessage));
    }

    private async Task<Result<byte[]>> ExtractValidationReportAsync(IPage page)
    {
        await page.ClickAsync("#botaoVisualizarConf");
        await page.WaitForFunctionAsync(@"
            document.querySelector('#btn-pdf') && !document.querySelector('#btn-pdf').disabled
        ");

        var filesBasePath = fileStore.GetFilesBasePath();
        await page.Client.SendAsync("Browser.setDownloadBehavior", new
        {
            behavior = "allow",
            downloadpath = filesBasePath
        });

        var downloadedFile = string.Empty;
        var downloadCompleteTcs = new TaskCompletionSource<string>();

        var watcher = new FileSystemWatcher(filesBasePath)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        watcher.Created += (sender, e) =>
        {
            if (e.Name!.EndsWith(PdfExtension) && !e.Name.EndsWith(DownloadExtension)) downloadedFile = e.FullPath;
        };

        watcher.Changed += (sender, e) =>
        {
            if (e.Name!.EndsWith(PdfExtension) && downloadedFile.Equals(e.FullPath) &&
                !File.Exists(e.FullPath + DownloadExtension))
                Task.Delay(500).ContinueWith(_ => downloadCompleteTcs.TrySetResult(e.FullPath));
        };

        await page.ClickAsync("#btn-pdf");
        var completedTask = await Task.WhenAny(downloadCompleteTcs.Task, Task.Delay(TimeSpan.FromSeconds(30)));
        if (completedTask != downloadCompleteTcs.Task)
            throw new TimeoutException("Timed out waiting for download to complete.");

        downloadedFile = await downloadCompleteTcs.Task;
        // Wait a bit more to ensure the downloaded file is fully written
        await Task.Delay(1000);

        var fileName = Path.GetFileNameWithoutExtension(downloadedFile);
        if (fileName.StartsWith(ReportPrefix)) fileName = fileName[ReportPrefix.Length..];

        var newFileName = $"{fileName}-validation-report{PdfExtension}";
        var newFileFullPath = Path.Combine(filesBasePath, newFileName);
        var renameDownloadedReport = fileStore.RenameFileAsync(downloadedFile, newFileFullPath);
        if (renameDownloadedReport.Failed) return Result.Failure<byte[]>(renameDownloadedReport.Error);


        watcher.Dispose();
        await browserService.ReleasePageAsync(page);
        return await fileStore.GetFileAsync(newFileFullPath);
    }

    private async Task<Result<FileValidationInfo>> ExtractFileValidationInfoAsync(IPage page)
    {
        var extractFileName = await ExtractFileNameAsync(page);
        if (extractFileName.Failed) return Result.Failure<FileValidationInfo>(extractFileName.Error);

        var extractFileHash = await ExtractFileHashAsync(page);
        if (extractFileHash.Failed) return Result.Failure<FileValidationInfo>(extractFileHash.Error);

        var extractValidationDate = await ExtractValidationDateAsync(page);
        if (extractValidationDate.Failed) return Result.Failure<FileValidationInfo>(extractValidationDate.Error);

        return Result.Success(new FileValidationInfo
        {
            FileName = extractFileName.Value,
            Hash = extractFileHash.Value,
            ValidationDate = extractValidationDate.Value
        });
    }

    private async Task<Result<string>> ExtractFileNameAsync(IPage page)
    {
        var fileNameElement = await page.QuerySelectorAsync("#nomeArquivo");
        if (fileNameElement is null)
        {
            const string errorMessage = "HTML element containing file name was not found.";
            logger.LogError(errorMessage);
            return Result.Failure<string>(SignatureValidationError.ExtractFileValidationInfoFailed(errorMessage));
        }

        var fileName =
            await fileNameElement.EvaluateFunctionAsync<string>("element => element?.textContent?.trim() || ''");
        if (string.IsNullOrEmpty(fileName))
        {
            const string errorMessage = "File name is null or empty.";
            logger.LogError(errorMessage);
            return Result.Failure<string>(SignatureValidationError.ExtractFileValidationInfoFailed(errorMessage));
        }

        return Result.Success(fileName);
    }

    private async Task<Result<DateTime>> ExtractValidationDateAsync(IPage page)
    {
        var validationDateElementHandle = await page.QuerySelectorAsync("#dataValidacao");
        if (validationDateElementHandle is null)
        {
            const string errorMessage = "HTML element containing validation date was not found.";
            logger.LogError(errorMessage);
            return Result.Failure<DateTime>(SignatureValidationError.ExtractFileValidationInfoFailed(errorMessage));
        }

        var validationDate = await validationDateElementHandle
            .EvaluateFunctionAsync<DateTime>("element => element.textContent");

        return Result.Success(validationDate);
    }

    private async Task<Result<string>> ExtractFileHashAsync(IPage page)
    {
        var fileHashElement = await page.QuerySelectorAsync("#hashArquivo");
        if (fileHashElement is null)
        {
            const string errorMessage = "HTML element containing file hash was not found.";
            logger.LogError(errorMessage);
            return Result.Failure<string>(SignatureValidationError.ExtractFileValidationInfoFailed(errorMessage));
        }

        var fileHash =
            await fileHashElement.EvaluateFunctionAsync<string>("element => element?.textContent?.trim() || ''");
        if (string.IsNullOrEmpty(fileHash))
        {
            const string errorMessage = "File hash is null or empty.";
            logger.LogError(errorMessage);
            return Result.Failure<string>(SignatureValidationError.ExtractFileValidationInfoFailed(errorMessage));
        }

        return Result.Success(fileHash);
    }

    private NavigationOptions WaitForNavigation(int timeout)
    {
        return new NavigationOptions { Timeout = timeout };
    }

    private NavigationOptions PageNavigationOptions(WaitUntilNavigation waitUntil, int timeout)
    {
        return new NavigationOptions { WaitUntil = [waitUntil], Timeout = timeout };
    }

    private WaitForSelectorOptions WaitForSelector(int timeout)
    {
        return new WaitForSelectorOptions { Timeout = timeout };
    }
}