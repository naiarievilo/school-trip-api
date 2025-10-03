namespace SchoolTripApi.Application.Agreements.DTOs;

public class FileValidationInfo
{
    public required string FileName { get; init; }
    public required string Hash { get; init; }
    public required DateTime ValidationDate { get; init; }
}