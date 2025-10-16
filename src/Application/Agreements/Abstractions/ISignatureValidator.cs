using SchoolTripApi.Application.Agreements.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Agreements.Abstractions;

public interface ISignatureValidator
{
    Task<Result<SignatureValidationResult>> ValidateFileSignatureAsync(byte[] file, string fileName);
}