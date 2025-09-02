using System.ComponentModel.DataAnnotations;
using Mediator;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Commands.ReauthenticateAccount;

public sealed class ReauthenticateAccountCommand(string refreshToken) : ICommand<Result<AuthenticationTokensResult>>
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken { get; } = refreshToken;
}