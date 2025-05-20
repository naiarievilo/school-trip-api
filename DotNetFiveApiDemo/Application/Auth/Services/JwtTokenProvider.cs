using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DotNetFiveApiDemo.Application.Auth.Interfaces;
using DotNetFiveApiDemo.Application.Settings;
using DotNetFiveApiDemo.Application.User.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DotNetFiveApiDemo.Application.Auth.Services
{
    public sealed class JwtTokenProvider : IJwtTokenProvider<ApplicationUser>
    {
        private const string TypeClaim = "Type";
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtTokenProvider> _logger;
        private readonly byte[] _secret;

        public JwtTokenProvider(IOptions<JwtSettings> jwtSettings, ILogger<JwtTokenProvider> logger)
        {
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _secret = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        }

        public string GenerateToken(string refreshToken, JwtTokenTypes tokenType)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));
            ;

            var tokenHandler = new JsonWebTokenHandler();
            var claims = tokenHandler.ValidateToken(refreshToken, GetTokenValidationParameters())
                .ClaimsIdentity.Claims.ToList();

            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) throw new Exception("Refresh token token must have an user ID.");

            var refreshTokenType = claims
                .FirstOrDefault(c => c.Properties[TypeClaim] == nameof(JwtTokenTypes.RefreshToken))
                ?.Value;
            if (refreshTokenType is null) throw new Exception("Token provided must be a refresh token.");

            var accessTokenClaims = CreateClaims(userId, tokenType);
            var tokenDescriptor = GenerateTokenDescriptor(accessTokenClaims, tokenType);

            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public bool IsTokenValid(string token, JwtTokenTypes tokenType)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            var tokenHandler = new JsonWebTokenHandler();
            var validatedToken = tokenHandler.ValidateToken(token, GetTokenValidationParameters());
            return validatedToken.ClaimsIdentity.Claims.Any(c =>
                c is { Type: TypeClaim, Value: nameof(tokenType) });
        }

        public string GenerateToken(ApplicationUser user, JwtTokenTypes tokenType)
        {
            var claims = CreateClaims(user.Id.ToString(), tokenType);
            var tokenDescriptor = GenerateTokenDescriptor(claims, tokenType);
            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(_secret),
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuer = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        private List<Claim> CreateClaims(string userId, JwtTokenTypes tokenType)
        {
            return new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new(ClaimTypes.NameIdentifier, userId),
                new(TypeClaim, tokenType.ToString())
            };
        }

        private DateTime GetExpirationTime(JwtTokenTypes tokenType)
        {
            var tokenTypes = _jwtSettings.TokenTypes;
            var defaultValue = new TokenTypesSettings { ExpirationInMinutes = _jwtSettings.ExpirationInMinutes };

            var expirationInMinutes =
                tokenTypes.GetValueOrDefault(tokenType.ToString(), defaultValue).ExpirationInMinutes;
            return DateTime.UtcNow.AddMinutes(expirationInMinutes);
        }

        private SecurityTokenDescriptor GenerateTokenDescriptor(IEnumerable<Claim> claims, JwtTokenTypes tokenType)
        {
            var expires = GetExpirationTime(tokenType);

            var symmetricSecurityKey = new SymmetricSecurityKey(_secret);
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };
        }
    }
}