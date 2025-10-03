using SchoolTripApi.Application.Agreements.Enums;

namespace SchoolTripApi.Application.Agreements.DTOs;

public sealed class SignatureInfo
{
    public required SignatureStatus Status { get; init; }
    public required string SignedBy { get; init; }
    public required string MaskedCpf { get; init; }
    public required string IssuingCertificateSerialNumber { get; init; }
    public required DateTimeOffset SignedDate { get; init; }
}