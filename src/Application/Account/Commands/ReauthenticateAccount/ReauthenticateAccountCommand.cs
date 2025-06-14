using System.ComponentModel.DataAnnotations;
using Mediator;
using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.ReauthenticateAccount;

public class ReauthenticateAccountCommand(string refreshToken) : ICommand<Result<AuthenticationTokensResult>>
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; } = refreshToken;
}