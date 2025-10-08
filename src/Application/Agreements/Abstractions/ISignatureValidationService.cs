using SchoolTripApi.Application.Agreements.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Agreements.Abstractions;

public interface ISignatureValidationService
{
    Task<Result<SignatureValidationResult>> ValidatePdfAsync(byte[] pdfData, string fileName);
}