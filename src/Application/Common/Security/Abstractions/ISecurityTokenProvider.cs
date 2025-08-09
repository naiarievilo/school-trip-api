using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Common.Security.Abstractions;

public interface ISecurityTokenProvider
{
    public AccessTokenResult IssueAccessToken(Guid accountId);
    Task<Result<AuthenticationTokensResult>> RefreshAccessTokenAsync(string refreshToken);
    Task<Result<string>> IssueRefreshTokenAsync(Guid accountId, string tokenFamily);
    Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(Guid accountId);
    Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(string refreshToken);
    Task<Result<string>> GeneratePasswordResetCodeAsync(string email);
    Task<Result<string>> GenerateEmailConfirmationTokenAsync(string email);
}