namespace SchoolTripApi.Domain.StudentMedicalProfileAggregate;

public sealed class Medication
{
    public required string Name { get; set; }
    public required string Dosage { get; set; }
    public required string Frequency { get; set; }
    public string? Notes { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}