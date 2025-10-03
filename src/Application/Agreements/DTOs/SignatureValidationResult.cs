using SchoolTripApi.Application.Agreements.Enums;

namespace SchoolTripApi.Application.Agreements.DTOs;

public sealed class SignatureValidationResult
{
    public required FileValidationInfo FileInfo { get; init; }
    public required SignatureInfo SignatureInfo { get; init; }
    public required byte[] ValidationReportPdf { get; init; }

    public bool Valid => SignatureInfo.Status == SignatureStatus.Approved;
}