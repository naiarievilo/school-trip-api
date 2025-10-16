namespace SchoolTripApi.Domain.StudentMedicalProfileAggregate;

public sealed class Allergy
{
    public required string AllergenName { get; set; }
    public required string Type { get; set; }
    public required string Severity { get; set; }
    public List<Medication>? Medications { get; set; }
    public string? Notes { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}