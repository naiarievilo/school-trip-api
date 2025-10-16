using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolTripApi.Infrastructure.Security;
using SchoolTripApi.Infrastructure.Security.Entities;
using SchoolTripApi.Infrastructure.Security.Services;
using SchoolTripApi.Infrastructure.Security.Settings;
using SchoolTripApi.Infrastructure.Templates;

namespace SchoolTripApi.Infrastructure.Data;

public static class AppDbContextSeed
{
    public static async Task SeedDatabases(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        try
        {
            var appDbContext = scopedProvider.GetRequiredService<AppDbContext>();
            var userManager = scopedProvider.GetRequiredService<UserManager<Account>>();
            var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var adminSettings = scopedProvider.GetRequiredService<IOptions<AdminSettings>>();

            // Update the database with any pending migrations
            if (appDbContext.Database.IsNpgsql()) await appDbContext.Database.MigrateAsync();

            // Default security data (e.g., roles, admin user)
            await SeedSecurityAsync(userManager, roleManager, adminSettings);

            await SeedAgreementTemplatesAsync(appDbContext);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occured while seeding the database: {errorMessage}", ex.Message);
        }
    }

    private static async Task SeedSecurityAsync(UserManager<Account> userManager,
        RoleManager<IdentityRole<Guid>> roleManager, IOptions<AdminSettings> adminSettings)
    {
        var adminRole = await roleManager.FindByNameAsync(AuthorizationConstants.Roles.Administrator);
        if (adminRole is null)
            await roleManager.CreateAsync(new IdentityRole<Guid>(AuthorizationConstants.Roles.Administrator));

        var adminEmail = adminSettings.Value.Email;
        var adminAccount = await userManager.FindByEmailAsync(adminEmail);
        if (adminAccount is not null) return;
        var adminUserName = AccountManager.GetEmailUserName(adminEmail);

        var providedAdminPassword = adminSettings.Value.Password;
        var adminPassword = string.IsNullOrEmpty(providedAdminPassword)
            ? AuthorizationConstants.DefaultPassword
            : providedAdminPassword;

        adminAccount = new Account { Email = adminEmail, UserName = adminUserName };
        await userManager.CreateAsync(adminAccount, adminPassword);

        adminAccount = await userManager.FindByNameAsync(adminUserName);
        if (adminAccount is not null)
            await userManager.AddToRoleAsync(adminAccount, AuthorizationConstants.Roles.Administrator);
    }

    private static async Task SeedAgreementTemplatesAsync(AppDbContext context)
    {
        var templates = await AgreementTemplateSeeder.LoadTemplatesFromEmbeddedResourceAsync();
        if (templates.Count == await context.AgreementTemplates.CountAsync()) return;

        if (templates.Count > 0)
            foreach (var template in templates)
            {
                var result = await context.AgreementTemplates
                    .FirstOrDefaultAsync(at => at.Version == template.Version && at.Type == template.Type);
                if (result is null) await context.AgreementTemplates.AddAsync(template);
            }

        await context.SaveChangesAsync();
    }
}