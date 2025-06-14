using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Domain.Common.DTOs;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Common.Security.Abstractions;

public interface ISecurityTokenProvider
{
    public AccessTokenResult IssueAccessToken(AccountId accountId);
    Task<Result<AuthenticationTokensResult>> RefreshAccessTokenAsync(string refreshToken);
    Task<Result<string>> IssueRefreshTokenAsync(AccountId accountId, string tokenFamily);
    Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(AccountId accountId);
    Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(string refreshToken);
    Task<Result<string>> GeneratePasswordResetCodeAsync(string email);
    Task<Result<string>> GenerateEmailConfirmationTokenAsync(string email);
}