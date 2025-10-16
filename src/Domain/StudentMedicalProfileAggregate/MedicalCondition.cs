namespace SchoolTripApi.Domain.StudentMedicalProfileAggregate;

public sealed class MedicalCondition
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Severity { get; set; }
    public List<Medication>? Medications { get; set; }
    public string? Notes { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}