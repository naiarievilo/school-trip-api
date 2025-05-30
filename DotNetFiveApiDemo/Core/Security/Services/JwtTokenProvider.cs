using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.Common.Interfaces;
using DotNetFiveApiDemo.Core.Security.DTOs;
using DotNetFiveApiDemo.Core.Security.Entities;
using DotNetFiveApiDemo.Core.Security.Errors;
using DotNetFiveApiDemo.Core.Security.Interfaces;
using DotNetFiveApiDemo.Core.Security.Settings;
using DotNetFiveApiDemo.Core.Security.Specifications;
using DotNetFiveApiDemo.Core.User.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DotNetFiveApiDemo.Core.Security.Services
{
    internal sealed class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtTokenProvider> _logger;
        private readonly IRepository<RefreshToken<AppUser>> _repository;
        private readonly SigningCredentials _signingCredentials;

        public JwtTokenProvider(IOptions<JwtSettings> jwtSettings, SymmetricSecurityKey symmetricKey,
            IRepository<RefreshToken<AppUser>> refreshTokenRepository, ILogger<JwtTokenProvider> logger)
        {
            _repository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;
            _signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            _logger = logger;
        }

        private DateTime AccessTokenExpiresAt => DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiresAtInMinutes);

        public async Task<Result<AuthenticationTokensResult>> RefreshAccessTokenAsync(string refreshToken)
        {
            var refreshTokenByTokenSpec = new RefreshTokenByTokenSpec(refreshToken);
            var currentRefreshToken = await _repository.GetBySpecAsync(refreshTokenByTokenSpec);

            if (currentRefreshToken is null)
                return Result.Failure<AuthenticationTokensResult>(SecurityError.RefreshTokenExpired);

            if (currentRefreshToken.IsExpired || currentRefreshToken.IsRevoked)
            {
                var currentTokenFamily = currentRefreshToken.TokenFamily;
                var spec = new RefreshTokenByTokenFamilySpec(currentTokenFamily);
                var refreshTokensFromFamily = await _repository.ListAsync(spec);

                await _repository.DeleteRangeAsync(refreshTokensFromFamily);
                return Result.Failure<AuthenticationTokensResult>(SecurityError.RefreshTokenExpired);
            }

            currentRefreshToken.IsRevoked = true;
            await _repository.UpdateAsync(currentRefreshToken);
            await _repository.SaveChangesAsync();

            var userId = currentRefreshToken.UserId;
            var issueAccessToken = IssueAccessToken(userId);
            var issueNewRefreshToken =
                await IssueRefreshTokenAsync(currentRefreshToken.User.Id, currentRefreshToken.TokenFamily);
            if (issueNewRefreshToken.Failed)
                return Result.Failure<AuthenticationTokensResult>(issueNewRefreshToken.Error);

            var newAccessToken = issueAccessToken.AccessToken;
            var expiresAt = issueAccessToken.ExpiresAt;
            var newRefreshToken = issueNewRefreshToken.Value;

            return Result.Success(new AuthenticationTokensResult
            {
                AccessToken = newAccessToken,
                ExpiresAt = expiresAt,
                RefreshToken = newRefreshToken
            });
        }

        public async Task<Result<string>> IssueRefreshTokenAsync(int userId, string tokenFamily = null)
        {
            var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[32];
            rng.GetBytes(randomBytes);

            var refreshToken = new RefreshToken<AppUser>
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomBytes),
                TokenFamily = tokenFamily ?? Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiresAtInDays)
            };

            try
            {
                await _repository.AddAsync(refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result.Failure<string>(SecurityError.FailedToIssueNewRefreshToken);
            }

            return Result.Success(refreshToken.Token);
        }

        public AccessTokenResult IssueAccessToken(int userId)
        {
            var claims = CreateClaims(userId.ToString());
            var expiresAt = AccessTokenExpiresAt;
            var tokenDescriptor = GenerateTokenDescriptor(claims, expiresAt);

            var accessToken = new JsonWebTokenHandler().CreateToken(tokenDescriptor);
            return new AccessTokenResult
            {
                AccessToken = accessToken,
                ExpiresAt = expiresAt
            };
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
    }
}