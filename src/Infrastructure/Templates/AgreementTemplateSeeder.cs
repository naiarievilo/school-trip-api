using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SchoolTripApi.Domain.AgreementAggregate;
using SchoolTripApi.Domain.AgreementAggregate.Extensions;

namespace SchoolTripApi.Infrastructure.Templates;

public static partial class AgreementTemplateSeeder
{
    private static readonly string TemplatesFolder = "Templates/";
    private static readonly Regex FileNamePattern = FileNameRegex();
    private static readonly string HtmlExtension = ".html";

    [GeneratedRegex(@"^v(\d+)-(national)-trip-agreement-template.html$", RegexOptions.IgnoreCase)]
    private static partial Regex FileNameRegex();

    public static async Task SeedAgreementTemplateAsync(this ModelBuilder modelBuilder)
    {
        var templates = await LoadTemplatesFromEmbeddedResourceAsync();

        if (templates.Count != 0) modelBuilder.Entity<AgreementTemplate>().HasData(templates);
    }

    public static async Task<List<AgreementTemplate>> LoadTemplatesFromEmbeddedResourceAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(name => name.Contains(TemplatesFolder) && name.EndsWith(HtmlExtension))
            .ToList();

        var templates = new List<AgreementTemplate>();
        foreach (var resourceName in resourceNames)
        {
            var match = FileNamePattern.Match(resourceName.Replace(TemplatesFolder, string.Empty));
            if (!match.Success) continue;

            var (version, type) = ExtractTemplateInfo(match);

            await using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null) throw new Exception($"Resource '{resourceName}' is empty.");

            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();

            templates.Add(AgreementTemplate.Create(content, version, type, "System"));
        }

        return templates;
    }

    private static (int version, TemplateType templateType) ExtractTemplateInfo(Match match)
    {
        var version = int.Parse(match.Groups[1].Value);
        var templateType = match.Groups[2].Value.ToLowerInvariant().ConvertToTemplateType();
        return (version, templateType);
    }
}