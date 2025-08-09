using Microsoft.AspNetCore.Identity;
using SchoolTripApi.Application.Account;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Security.Services;

public class AuthenticationService(
    ISecurityTokenProvider securityTokenProvider,
    UserManager<Account> userManager,
    SignInManager<Account> signInManager)
    : IAuthenticationService
{
    public async Task<Result<AuthenticationTokensResult>> CheckCredentialsAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return Result.Failure<AuthenticationTokensResult>(AccountError.FailedToSignInUser);

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
                return Result.Failure<AuthenticationTokensResult>(AccountError.UserIsLockedOut);

            if (signInResult.IsNotAllowed)
                throw new Exception("'IdentityOptions.SignIn.RequireConfirmedEmail' must be set to 'true'.");

            return Result.Failure<AuthenticationTokensResult>(AccountError.FailedToSignInUser);
        }

        var resetAccessFailedCount = await userManager.ResetAccessFailedCountAsync(user);
        if (!resetAccessFailedCount.Succeeded)
            throw new Exception("Failed to reset access failed count.");

        return await securityTokenProvider.IssueAuthenticationTokensAsync(user.Id);
    }

    public async Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokens(Guid accountId)
    {
        return await securityTokenProvider.IssueAuthenticationTokensAsync(accountId);
    }

    public async Task<Result<AuthenticationTokensResult>> RefreshAuthenticationTokensAsync(string refreshToken)
    {
        return await securityTokenProvider.IssueAuthenticationTokensAsync(refreshToken);
    }
}