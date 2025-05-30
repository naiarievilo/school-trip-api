using System.Threading.Tasks;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.Application.User.Interfaces;
using DotNetFiveApiDemo.Core.Security.DTOs;
using DotNetFiveApiDemo.Core.User.DTOs;
using DotNetFiveApiDemo.Core.User.Errors;
using DotNetFiveApiDemo.WebApi.User.DTOs;
using DotNetFiveApiDemo.WebApi.User.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetFiveApiDemo.WebApi.User
{
    [Authorize]
    [ApiController]
    [Route("/users")]
    [Produces("application/json")]
    [SwaggerTag("Manages user accounts")]
    public class UserController : ControllerBase
    {
        private readonly IUserApplicationService _userApplicationService;

        public UserController(IUserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        [SwaggerOperation("Creates new user")]
        [SwaggerResponse(StatusCodes.Status201Created,
            "Creates new user account successfully",
            typeof(SignUpUserResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult<SignUpUserResult>> SignUpUser([FromBody] SignUpUserRequest request)
        {
            var signUpUser = await _userApplicationService.SignUpUserAsync(request);
            return signUpUser.Succeeded
                ? CreatedAtAction(nameof(GetUser), new { userId = signUpUser.Value.UserId }, signUpUser.Value)
                : BadRequest(signUpUser.Error);
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        [SwaggerOperation("Authenticates user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Authenticates user account successfully",
            typeof(SignInUserResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(UserError))]
        public async Task<ActionResult<SignInUserResult>> SignInUser([FromBody] SignInUserRequest request)

        {
            var signInUser = await _userApplicationService.SignInUserAsync(request);
            return signInUser.Succeeded
                ? Ok(signInUser.Value)
                : BadRequest(signInUser.Error);
        }

        [HttpPost("reauthentication")]
        [AllowAnonymous]
        [SwaggerOperation("Reauthenticates user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Reauthenticates user account successfully",
            typeof(AuthenticationTokensResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult<AuthenticationTokensResult>> ReauthenticateUser(
            [FromBody] RefreshAccessTokenRequest request)
        {
            var reauthenticateUser = await _userApplicationService.ReauthenticateUser(request);
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
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult> SendResetPasswordCode([FromBody] UnverifiedUserRequest request)
        {
            var sendPasswordResetCode = await _userApplicationService.SendPasswordResetCodeAsync(request);
            return sendPasswordResetCode.Succeeded
                ? Ok()
                : BadRequest(sendPasswordResetCode.Error);
        }

        [HttpPatch("password-reset-confirmation")]
        [AllowAnonymous]
        [SwaggerOperation("Completes password reset")]
        [SwaggerResponse(StatusCodes.Status200OK, "Resets password and authenticates user account successfully",
            typeof(SignInUserResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult<SignInUserResult>> ConfirmPasswordReset([FromBody] ResetPasswordRequest request)
        {
            var confirmPasswordReset = await _userApplicationService.ConfirmPasswordResetAsync(request);
            return confirmPasswordReset.Succeeded
                ? Ok(confirmPasswordReset.Value)
                : BadRequest(confirmPasswordReset.Error);
        }

        [HttpPost("email-confirmation")]
        [AllowAnonymous]
        [SwaggerOperation("Completes user email confirmation")]
        [SwaggerResponse(StatusCodes.Status200OK, "Confirms user email successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult> ConfirmUserEmail([FromBody] ConfirmUserEmailRequest request)
        {
            var confirmEmail = await _userApplicationService.ConfirmUserEmailAsync(request);
            return confirmEmail.Succeeded
                ? Ok()
                : BadRequest(confirmEmail.Error);
        }

        [HttpGet("{userId}")]
        [MatchesAuthenticatedUserId]
        [SwaggerOperation("Gets user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Gets user successfully",
            typeof(UserDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult<UserDto>> GetUser([FromRoute] int userId)
        {
            var getUser = await _userApplicationService.GetUserAsync(userId);
            return getUser.Succeeded
                ? Ok(getUser.Value)
                : BadRequest(getUser.Error);
        }

        [HttpDelete("{userId}")]
        [MatchesAuthenticatedUserId]
        [SwaggerOperation("Deletes user")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Deletes user successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            var deleteUser = await _userApplicationService.DeleteUserAsync(userId);
            return deleteUser.Succeeded
                ? NoContent()
                : BadRequest(deleteUser.Error);
        }

        [HttpPost("{userId}/info")]
        [MatchesAuthenticatedUserId]
        [SwaggerOperation("Updates user information")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Updates user information successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult> UpdateInfoUser([FromBody] UpdateUserInfoRequest request, [FromRoute] int userId)
        {
            var updateUserInfo = await _userApplicationService.UpdateUserInfoAsync(request, userId);
            return updateUserInfo.Succeeded
                ? NoContent()
                : BadRequest(updateUserInfo.Error);
        }

        [HttpPost("{userId}/email")]
        [MatchesAuthenticatedUserId]
        [SwaggerOperation("Updates user email")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updates user email successfully",
            typeof(UpdateUserEmailResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult<UpdateUserEmailResult>> UpdateUserEmail(
            [FromBody] UpdateUserEmailRequest request,
            [FromRoute] int userId)
        {
            var updateUserEmail = await _userApplicationService.UpdateUserEmailAsync(request, userId);
            return updateUserEmail.Succeeded
                ? Ok(updateUserEmail.Value)
                : BadRequest(updateUserEmail.Error);
        }

        [HttpPost("{userId}/email-confirmation")]
        [MatchesAuthenticatedUserId]
        [SwaggerOperation("Initiates email confirmation process")]
        [SwaggerResponse(StatusCodes.Status200OK, "Sends user email confirmation successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult> SendUserEmailConfirmation([FromRoute] int userId)
        {
            var confirmUserEmail = await _userApplicationService.SendEmailConfirmationAsync(userId);
            return confirmUserEmail.Succeeded
                ? Ok()
                : BadRequest(confirmUserEmail.Error);
        }

        [HttpPost("{userId}/password")]
        [MatchesAuthenticatedUserId]
        [SwaggerOperation("Updates user password")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Updates user password successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest,
            "Request body validation or business rule validation failed",
            typeof(Core.Common.DTOs.Error))]
        public async Task<ActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordRequest request,
            [FromRoute] int userId)
        {
            var updateUserPassword = await _userApplicationService.UpdateUserPasswordAsync(request, userId);
            return updateUserPassword.Succeeded
                ? NoContent()
                : BadRequest(updateUserPassword.Error);
        }
    }
}