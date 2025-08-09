using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Common.Security.Abstractions;

public interface IAuthenticationService
{
    Task<Result<AuthenticationTokensResult>> CheckCredentialsAsync(string email, string password);
    Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokens(Guid accountId);
    Task<Result<AuthenticationTokensResult>> RefreshAuthenticationTokensAsync(string refreshToken);
}