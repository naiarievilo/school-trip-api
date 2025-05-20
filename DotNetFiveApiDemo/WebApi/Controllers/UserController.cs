using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Application.Auth.Filters;
using DotNetFiveApiDemo.Application.Auth.Interfaces;
using DotNetFiveApiDemo.Application.Common.DTOs;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.Application.User.Identity;
using DotNetFiveApiDemo.Application.User.Interfaces;
using DotNetFiveApiDemo.WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DotNetFiveApiDemo.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/users")]
    [Produces("application/json")]
    [SwaggerTag("Operations for managing user accounts")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService<ApplicationUser> _authenticationService;
        private readonly IUserApplicationService _userApplicationService;

        public UserController(IUserApplicationService userApplicationService,
            IAuthenticationService<ApplicationUser> authenticationService)
        {
            _userApplicationService = userApplicationService;
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result<Dictionary<string, string>>>> CreateUser(
            [FromBody] UserCreationCommand command)
        {
            var result = await _userApplicationService.CreateUserAsync(command);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetUser), new { userId = result.Value["userId"] }, result.Value)
                : BadRequest(result.Error.Description);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, string>>> LogInUser(
            [FromBody] UserLoginCommand command)
        {
            var result = await _userApplicationService.LogInUserAsync(command);
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error.Description);
        }

        [HttpGet("{userId}")]
        [MatchesUserId]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> GetUser(int userId)
        {
            var result = await _userApplicationService.GetUserAsync(userId);
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error.Description);
        }

        [HttpPatch("{userId}")]
        [MatchesUserId]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateCommand command, int userId)
        {
            var result = await _userApplicationService.UpdateUserAsync(command, userId);
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error.Description);
        }

        [HttpDelete("{userId}")]
        [MatchesUserId]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var result = await _userApplicationService.DeleteUserAsync(userId);
            return result.IsSuccess
                ? NoContent()
                : BadRequest(result.Error.Description);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult<Dictionary<string, string>> RefreshAccessToken(RefreshTokenCommand command)
        {
            var result = _authenticationService.RefreshAccessToken(command);
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error.Description);
        }
    }
}