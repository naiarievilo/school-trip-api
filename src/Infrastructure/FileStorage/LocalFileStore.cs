using Microsoft.Extensions.Options;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Infrastructure.FileStorage;

public class LocalFileStore(IOptions<LocalFileStoreSettings> options, IAppLogger<LocalFileStore> logger)
    : IFileStore
{
    private readonly LocalFileStoreSettings _settings = options.Value;

    public async Task<Result> SaveFileAsync(byte[] file, string fileName)
    {
        var filePath = BuildFilePath(fileName);
        try
        {
            await File.WriteAllBytesAsync(filePath, file);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Couldn't save file '{fileName}': {errorMessage}", fileName, ex.Message);
            return Result.Failure(FileStoreError.FailedToSaveFile(fileName));
        }
    }

    public Result DeleteFileAsync(string fileName)
    {
        var filePath = BuildFilePath(fileName);
        if (!File.Exists(filePath)) Result.Success();

        try
        {
            File.Delete(filePath);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete file '{fileName}'.", fileName);
            return Result.Failure(FileStoreError.FailedToDeleteFile(fileName));
        }
    }

    public async Task<Result<byte[]>> GetFileAsync(string fileName)
    {
        var filePath = BuildFilePath(fileName);
        if (!File.Exists(filePath)) return Result.Failure<byte[]>(FileStoreError.FileNotFound(fileName));

        var file = await File.ReadAllBytesAsync(filePath);
        return Result.Success(file);
    }

    public Result RenameFileAsync(string oldFileName, string newFileName)
    {
        var oldFileFullPath = BuildFilePath(oldFileName);
        if (!File.Exists(oldFileFullPath)) return Result.Failure(FileStoreError.FileNotFound(oldFileName));

        var newFileFullPath = BuildFilePath(newFileName);
        try
        {
            File.Move(oldFileFullPath, newFileFullPath, true);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Couldn't rename file '{oldFileName}': {errorMessage}", oldFileName, ex.Message);
            return Result.Failure(FileStoreError.FailedToRenameFile(oldFileName));
        }
    }

    public string GetFilePath(string fileName)
    {
        return BuildFilePath(fileName);
    }

    public string GetFilesBasePath()
    {
        return _settings.SignedAgreementsPath;
    }

    private string BuildFilePath(string fileName)
    {
        return Path.Combine(_settings.SignedAgreementsPath, fileName);
    }
}