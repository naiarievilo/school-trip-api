using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTripApi.Application.Accounts.Commands.DeleteAccount;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Application.Accounts.Queries.GetAccountsInfo;
using SchoolTripApi.Application.Common.DTOs;
using SchoolTripApi.Infrastructure.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolTripApi.WebApi.Accounts;

[Authorize(Roles = AuthorizationConstants.Roles.Administrator)]
[ApiController]
[Route("/accounts/admin")]
[Produces("application/json")]
[SwaggerTag("Manages admin operations")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpPost("account-deletion")]
    [SwaggerOperation("Deletes account")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Deletes account via admin rights successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body or business rule validation failed",
        typeof(Domain.Common.Errors.Error))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Client does not have permission to delete account")]
    public async Task<ActionResult> DeleteAccount([FromBody] DeleteAccountCommand command,
        CancellationToken cancellationToken)
    {
        var deleteAccount = await mediator.Send(command, cancellationToken);
        return deleteAccount.Succeeded
            ? NoContent()
            : BadRequest(deleteAccount.Error);
    }

    [HttpGet("accounts-information")]
    [SwaggerOperation("Gets accounts' information")]
    [SwaggerResponse(StatusCodes.Status200OK, "Gets accounts' information")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Client does not have permission to get accounts' information")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request or business rule validation failed",
        typeof(Domain.Common.Errors.Error))]
    public async Task<ActionResult<PageOf<AccountDto>>> GetAccountsInfo([FromQuery] PaginationDetails paginationDetails,
        CancellationToken cancellationToken)
    {
        var getAccountsInfoPage =
            await mediator.Send(GetAccountsInfoQuery.With(paginationDetails), cancellationToken);
        return getAccountsInfoPage.Succeeded
            ? Ok(getAccountsInfoPage.Value)
            : BadRequest(getAccountsInfoPage.Error);
    }
}