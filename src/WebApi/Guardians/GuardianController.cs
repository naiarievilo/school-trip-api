using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolTripApi.Application.Guardians.Commands.UpdateGuardianInfo;
using SchoolTripApi.Application.Guardians.DTOs;
using SchoolTripApi.Application.Guardians.Queries.GetGuardianInfo;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolTripApi.WebApi.Guardians;

[Authorize]
[ApiController]
[Route("accounts/{accountId}/info")]
[Produces("application/json")]
[SwaggerTag("Manages user accounts guardian information")]
public class GuardianController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation("Gets user account's guardian information")]
    [SwaggerResponse(StatusCodes.Status200OK, "Gets guardian information successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request or business rule validation failed.",
        typeof(Domain.Common.Errors.Error))]
    public async Task<ActionResult<GuardianDto>> GetGuardianInfo([FromRoute] string accountId,
        CancellationToken cancellationToken)
    {
        var getGuardianInfo = await mediator.Send(GetGuardianInfoQuery.For(accountId), cancellationToken);
        return getGuardianInfo.Succeeded
            ? getGuardianInfo.Value
            : BadRequest(getGuardianInfo.Error);
    }

    [HttpPut]
    [SwaggerOperation("Updates user account's guardian information")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Updates guardian information successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest,
        "Request body or business rule validation failed",
        typeof(Domain.Common.Errors.Error))]
    public async Task<ActionResult> UpdateGuardianInfo([FromRoute] string accountId,
        [FromBody] UpdateGuardianInfoCommand command, CancellationToken cancellationToken)
    {
        var updateGuardianInfo = await mediator.Send(command.For(accountId), cancellationToken);
        return updateGuardianInfo.Succeeded
            ? NoContent()
            : BadRequest(updateGuardianInfo.Error);
    }
}