using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTripApi.Application.Account;
using SchoolTripApi.Application.Account.Commands.AuthenticateAccount;
using SchoolTripApi.Application.Account.Commands.ConfirmAccountEmail;
using SchoolTripApi.Application.Account.Commands.CreateAccount;
using SchoolTripApi.Application.Account.Commands.DeleteAccount;
using SchoolTripApi.Application.Account.Commands.ReauthenticateAccount;
using SchoolTripApi.Application.Account.Commands.ResetAccountPassword;
using SchoolTripApi.Application.Account.Commands.SendAccountEmailConfirmation;
using SchoolTripApi.Application.Account.Commands.SendPasswordResetCode;
using SchoolTripApi.Application.Account.Commands.UpdateAccountEmail;
using SchoolTripApi.Application.Account.Commands.UpdateAccountPassword;
using SchoolTripApi.Application.Account.Queries;
using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.WebApi.Account.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolTripApi.WebApi.Account;

[Authorize]
[ApiController]
[Route("/accounts")]
[Produces("application/json")]
[SwaggerTag("Manages user accounts")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost("creation")]
    [AllowAnonymous]
    [SwaggerOperation("Creates new account")]
    [SwaggerResponse(StatusCodes.Status201Created,
        "Creates new account successfully",
        typeof(CreateAccountResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult<CreateAccountResult>> CreateAccount(
        [FromBody] CreateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var createAccount = await mediator.Send(command, cancellationToken);
        return createAccount.Succeeded
            ? CreatedAtAction(nameof(GetAccountInfo), new { userId = createAccount.Value.AccountId },
                createAccount.Value)
            : BadRequest(createAccount.Error);
    }

    [HttpPost("authentication")]
    [AllowAnonymous]
    [SwaggerOperation("Authenticates account")]
    [SwaggerResponse(StatusCodes.Status200OK, "Authenticates account successfully",
        typeof(AuthenticateAccountResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(AccountError))]
    public async Task<ActionResult<AuthenticateAccountResult>> AuthenticateAccount(
        [FromBody] AuthenticateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var authenticateAccount = await mediator.Send(command, cancellationToken);
        return authenticateAccount.Succeeded
            ? Ok(authenticateAccount.Value)
            : BadRequest(authenticateAccount.Error);
    }

    [HttpPost("reauthentication")]
    [AllowAnonymous]
    [SwaggerOperation("Reauthenticates account")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reauthenticates account successfully",
        typeof(AuthenticationTokensResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult<AuthenticationTokensResult>> ReauthenticateAccount(
        [FromBody] ReauthenticateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var reauthenticateUser = await mediator.Send(command, cancellationToken);
        return reauthenticateUser.Succeeded
            ? Ok(reauthenticateUser.Value)
            : BadRequest(reauthenticateUser.Error);
    }

    [HttpPost("password-reset")]
    [AllowAnonymous]
    [SwaggerOperation("Initiates password reset process")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sends password reset message successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult> SendResetPasswordCode(
        [FromBody] SendPasswordResetCodeCommand command,
        CancellationToken cancellationToken)
    {
        var sendPasswordResetCode = await mediator.Send(command, cancellationToken);
        return sendPasswordResetCode.Succeeded
            ? Ok()
            : BadRequest(sendPasswordResetCode.Error);
    }

    [HttpPatch("password-reset-confirmation")]
    [AllowAnonymous]
    [SwaggerOperation("Completes password reset")]
    [SwaggerResponse(StatusCodes.Status200OK, "Resets password and authenticates account successfully",
        typeof(AuthenticateAccountResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult<AuthenticateAccountResult>> ResetAccountPassword(
        [FromBody] ResetAccountPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var resetAccountPassword = await mediator.Send(command, cancellationToken);
        return resetAccountPassword.Succeeded
            ? Ok(resetAccountPassword.Value)
            : BadRequest(resetAccountPassword.Error);
    }

    [HttpPost("email-confirmation")]
    [AllowAnonymous]
    [SwaggerOperation("Completes account's email confirmation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Confirms account's email successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult> ConfirmAccountEmail(
        [FromBody] ConfirmAccountEmailCommand command,
        CancellationToken cancellationToken)
    {
        var confirmEmail = await mediator.Send(command, cancellationToken);
        return confirmEmail.Succeeded
            ? Ok()
            : BadRequest(confirmEmail.Error);
    }

    [HttpGet("{accountId}")]
    [MatchesAuthenticatedAccountId]
    [SwaggerOperation("Gets account info")]
    [SwaggerResponse(StatusCodes.Status200OK, "Gets account info successfully",
        typeof(AccountDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult<AccountDto>> GetAccountInfo(
        [FromRoute] string accountId,
        CancellationToken cancellationToken)
    {
        var getUser = await mediator.Send(GetAccountInfoQuery.For(accountId), cancellationToken);
        return getUser.Succeeded
            ? Ok(getUser.Value)
            : BadRequest(getUser.Error);
    }

    [HttpDelete("{accountId}")]
    [MatchesAuthenticatedAccountId]
    [SwaggerOperation("Deletes account")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Deletes account successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult> DeleteAccount([FromRoute] string accountId, CancellationToken cancellationToken)
    {
        var deleteAccount = await mediator.Send(DeleteAccountCommand.For(accountId), cancellationToken);
        return deleteAccount.Succeeded
            ? NoContent()
            : BadRequest(deleteAccount.Error);
    }

    [HttpPost("{accountId}/email")]
    [MatchesAuthenticatedAccountId]
    [SwaggerOperation("Updates account's email")]
    [SwaggerResponse(StatusCodes.Status200OK, "Updates account's email successfully",
        typeof(UpdateAccountEmailResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult<UpdateAccountEmailResult>> UpdateAccountEmail(
        [FromBody] UpdateAccountEmailCommand command,
        [FromRoute] string accountId,
        CancellationToken cancellationToken)
    {
        var updateAccountEmail = await mediator.Send(command.For(accountId), cancellationToken);
        return updateAccountEmail.Succeeded
            ? Ok(updateAccountEmail.Value)
            : BadRequest(updateAccountEmail.Error);
    }

    [HttpPost("{accountId}/email-confirmation")]
    [MatchesAuthenticatedAccountId]
    [SwaggerOperation("Initiates email confirmation process")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sends account email confirmation successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult> SendAccountEmailConfirmation(
        [FromRoute] string accountId,
        CancellationToken cancellationToken)
    {
        var sendAccountEmailConfirmation =
            await mediator.Send(SendAccountEmailConfirmationCommand.For(accountId), cancellationToken);
        return sendAccountEmailConfirmation.Succeeded
            ? Ok()
            : BadRequest(sendAccountEmailConfirmation.Error);
    }

    [HttpPost("{accountId}/password")]
    [MatchesAuthenticatedAccountId]
    [SwaggerOperation("Updates account password")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Updates account password successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body validation or business rule validation failed",
        typeof(Domain.Common.DTOs.Error))]
    public async Task<ActionResult> UpdateAccountPassword([FromBody] UpdateAccountPasswordCommand command,
        [FromRoute] string accountId,
        CancellationToken cancellationToken)
    {
        var updateAccountPassword = await mediator.Send(command.For(accountId), cancellationToken);
        return updateAccountPassword.Succeeded
            ? NoContent()
            : BadRequest(updateAccountPassword.Error);
    }
}