namespace SchoolTripApi.Domain.AgreementAggregate.Extensions;

public static class TemplateTypeConverter
{
    private static readonly Dictionary<string, TemplateType> TemplateTypes = new()
    {
        { "national", TemplateType.National }
    };

    public static TemplateType ConvertToTemplateType(this string templateTypeFromFileName)
    {
        return TemplateTypes.TryGetValue(templateTypeFromFileName, out var templateType)
            ? templateType
            : throw new Exception($"'{templateTypeFromFileName}' couldn't be converted to '{typeof(TemplateType)}'.");
    }
}