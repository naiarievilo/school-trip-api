using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.StudentMedicalProfileAggregate;

public class StudentMedicalProfile : IAggregateRoot
{
    public StudentMedicalProfile(Guid studentId, List<MedicalCondition> conditions, List<Allergy> allergies,
        string diet,
        Dictionary<string, object> customFields)
    {
        Id = studentId;
        Conditions = conditions;
        Allergies = allergies;
        Diet = diet;
        CustomFields = customFields;
    }

    public Guid Id { get; set; }
    public List<MedicalCondition> Conditions { get; set; }
    public List<Allergy> Allergies { get; set; }
    public required string Diet { get; set; }
    public Dictionary<string, object>? CustomFields { get; set; }
}