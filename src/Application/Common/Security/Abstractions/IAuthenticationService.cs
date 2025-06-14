using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Domain.Common.DTOs;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Common.Security.Abstractions;

public interface IAuthenticationService
{
    Task<Result<AuthenticationTokensResult>> CheckCredentialsAsync(string email, string password);
    Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokens(AccountId accountId);
    Task<Result<AuthenticationTokensResult>> RefreshAuthenticationTokensAsync(string refreshToken);
}