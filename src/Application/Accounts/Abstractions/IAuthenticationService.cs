using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Abstractions;

public interface IAuthenticationService
{
    Task<Result<AuthenticationTokensResult>> CheckCredentialsAsync(string email, string password);
    Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokens(Guid accountId);
    Task<Result<AuthenticationTokensResult>> RefreshAuthenticationTokensAsync(string refreshToken);
}