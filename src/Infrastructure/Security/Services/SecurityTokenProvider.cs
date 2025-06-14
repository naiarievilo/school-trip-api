using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SchoolTripApi.Application.Account;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Application.Common.Security.Errors;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Infrastructure.Security.Entities;
using SchoolTripApi.Infrastructure.Security.Settings;
using SchoolTripApi.Infrastructure.Security.Specifications;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Infrastructure.Security.Services;

internal sealed class SecurityTokenProvider(
    IOptions<JwtSettings> jwtSettings,
    SymmetricSecurityKey symmetricKey,
    IRepository<RefreshToken> refreshTokenRepository,
    UserManager<Account> userManager,
    IAppLogger<SecurityTokenProvider> logger)
    : ISecurityTokenProvider
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly SigningCredentials _signingCredentials = new(symmetricKey, SecurityAlgorithms.HmacSha256);

    private DateTime AccessTokenExpiresAt => DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiresAtInMinutes);

    public async Task<Result<AuthenticationTokensResult>> RefreshAccessTokenAsync(string refreshToken)
    {
        var refreshTokenByTokenSpec = new RefreshTokenByTokenSpec(refreshToken);
        var currentRefreshToken = await refreshTokenRepository.SingleOrDefaultAsync(refreshTokenByTokenSpec);

        if (currentRefreshToken is null)
            return Result.Failure<AuthenticationTokensResult>(SecurityError.RefreshTokenExpired);

        if (currentRefreshToken.IsExpired || currentRefreshToken.IsRevoked)
        {
            var currentTokenFamily = currentRefreshToken.TokenFamily;
            var spec = new RefreshTokenByTokenFamilySpec(currentTokenFamily);
            var refreshTokensFromFamily = await refreshTokenRepository.ListAsync(spec);

            await refreshTokenRepository.DeleteRangeAsync(refreshTokensFromFamily);
            return Result.Failure<AuthenticationTokensResult>(SecurityError.RefreshTokenExpired);
        }

        await refreshTokenRepository.UpdateAsync(currentRefreshToken.Revoke());
        await refreshTokenRepository.SaveChangesAsync();

        var userId = currentRefreshToken.AccountId;
        var issueAccessToken = IssueAccessToken(userId);
        var issueNewRefreshToken =
            await IssueRefreshTokenAsync(userId, currentRefreshToken.TokenFamily);
        if (issueNewRefreshToken.Failed)
            return Result.Failure<AuthenticationTokensResult>(issueNewRefreshToken.Error);

        var newAccessToken = issueAccessToken.AccessToken;
        var expiresAt = issueAccessToken.ExpiresAt;
        var newRefreshToken = issueNewRefreshToken.Value;

        return Result.Success(AuthenticationTokensResult.From(newAccessToken, expiresAt, newRefreshToken));
    }


    public async Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(AccountId accountId)
    {
        var issueAccessToken = IssueAccessToken(accountId);
        var issueRefreshToken = await IssueRefreshTokenAsync(accountId, null);
        if (issueRefreshToken.Failed)
            return Result.Failure<AuthenticationTokensResult>(issueRefreshToken.Error);

        var accessToken = issueAccessToken.AccessToken;
        var expiresAt = issueAccessToken.ExpiresAt;
        var refreshToken = issueRefreshToken.Value;
        return Result.Success(AuthenticationTokensResult.From(accessToken, expiresAt, refreshToken));
    }

    public async Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(string refreshToken)
    {
        return await RefreshAccessTokenAsync(refreshToken);
    }

    public async Task<Result<string>> IssueRefreshTokenAsync(AccountId accountId, string? tokenFamily)
    {
        var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[32];
        rng.GetBytes(randomBytes);

        var refreshToken = new RefreshToken(
            accountId,
            Convert.ToBase64String(randomBytes),
            tokenFamily ?? Guid.NewGuid().ToString(),
            DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresAtInDays)
        );

        try
        {
            await refreshTokenRepository.AddAsync(refreshToken);
        }
        catch (Exception ex)
        {
            logger.LogError("Couldn't issue new refresh token: {1}", ex.Message);
            return Result.Failure<string>(SecurityError.FailedToIssueNewRefreshToken);
        }

        return Result.Success(refreshToken.Token);
    }

    public AccessTokenResult IssueAccessToken(AccountId accountId)
    {
        var claims = CreateClaims(accountId.ToString());
        var expiresAt = AccessTokenExpiresAt;
        var tokenDescriptor = GenerateTokenDescriptor(claims, expiresAt);

        var accessToken = new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        return AccessTokenResult.From(accessToken, expiresAt);
    }

    public async Task<Result<string>> GeneratePasswordResetCodeAsync(string email)
    {
        var getUser = await GetApplicationUserByEmailAsync(email);
        if (getUser.Failed) return Result.Failure<string>(getUser.Error);
        var user = getUser.Value;

        var passwordResetCode = await userManager.GeneratePasswordResetTokenAsync(user);
        return Result.Success(passwordResetCode);
    }

    public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(string email)
    {
        var getAppUser = await GetApplicationUserByEmailAsync(email);
        if (getAppUser.Failed) return Result.Failure<string>(getAppUser.Error);

        var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(getAppUser.Value);
        if (string.IsNullOrWhiteSpace(emailConfirmationToken))
            return Result.Failure<string>(AccountError.FailedToGenerateEmailConfirmationToken);
        return Result.Success(emailConfirmationToken);
    }

    private static List<Claim> CreateClaims(string userId)
    {
        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, userId)
        };
    }

    private SecurityTokenDescriptor GenerateTokenDescriptor(IEnumerable<Claim> claims, DateTime expiresAt)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            SigningCredentials = _signingCredentials,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };
    }

    private async Task<Result<Account>> GetApplicationUserByEmailAsync(string email)
    {
        var appUser = await userManager.FindByEmailAsync(email);
        return appUser is not null
            ? Result.Success(appUser)
            : Result.Failure<Account>(AccountError.UserNotFound(email));
    }
}