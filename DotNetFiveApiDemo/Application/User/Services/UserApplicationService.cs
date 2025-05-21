using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotNetFiveApiDemo.Application.Auth.Interfaces;
using DotNetFiveApiDemo.Application.Common.DTOs;
using DotNetFiveApiDemo.Application.Common.DTOs.Base;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.Application.User.Identity;
using DotNetFiveApiDemo.Application.User.Interfaces;
using DotNetFiveApiDemo.Domain.User.Interfaces;
using DotNetFiveApiDemo.WebApi.DTOs;

namespace DotNetFiveApiDemo.Application.User.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IAuthenticationService<ApplicationUser> _authenticationService;
        private readonly IMapper _mapper;
        private readonly IUserService<ApplicationUser> _userService;

        public UserApplicationService(IUserService<ApplicationUser> userService,
            IAuthenticationService<ApplicationUser> authenticationService, IMapper mapper)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<Result<Dictionary<string, string>>> CreateUserAsync(UserCreationCommand command)
        {
            var user = _mapper.Map<ApplicationUser>(command);

            var newUserResult = await _userService.CreateUserAsync(user);
            if (newUserResult.IsFailure) return Result.Failure<Dictionary<string, string>>(newUserResult.Error);

            var result = _authenticationService.GenerateAccessAndRefreshTokens(user);
            result.Add("userId", user.Id.ToString());

            return Result.Success(result);
        }

        public async Task<Result<UserDto>> GetUserAsync(int userId)
        {
            var getUserResult = await _userService.GetUserAsync(userId);
            if (getUserResult.IsFailure) return Result.Failure<UserDto>(getUserResult.Error);

            var userDto = _mapper.Map<UserDto>(getUserResult.Value);
            return Result.Success(userDto);
        }

        public async Task<Result<Dictionary<string, string>>> SignInUserAsync(UserLoginCommand command)
        {
            var user = _mapper.Map<ApplicationUser>(command);

            var validationResult = await _userService.ValidateCredentials(user);
            if (validationResult.IsFailure)
                return Result.Failure<Dictionary<string, string>>(validationResult.Error);

            user = validationResult.Value;
            var result = _authenticationService.GenerateAccessAndRefreshTokens(user);
            result.Add("userId", user.Id.ToString());
            return Result.Success(result);
        }

        public async Task<Result<UserDto>> UpdateUserAsync(UserUpdateCommand command, int userId)
        {
            var getUserResult = await _userService.GetUserAsync(userId);
            if (getUserResult.IsFailure) return Result.Failure<UserDto>(getUserResult.Error);

            var user = _mapper.Map(command, getUserResult.Value);
            var currentPassword = command.CurrentPassword;
            var newPassword = command.NewPassword;

            var userUpdateResult = await _userService.UpdateUserAsync(user, currentPassword, newPassword);
            if (userUpdateResult.IsFailure) return Result.Failure<UserDto>(userUpdateResult.Error);

            var userDto = _mapper.Map<UserDto>(userUpdateResult.Value);
            return Result.Success(userDto);
        }

        public async Task<Result> DeleteUserAsync(int userId)
        {
            var userDeletionResult = await _userService.DeleteUserAsync(userId);
            if (userDeletionResult.IsFailure) return Result.Failure(userDeletionResult.Error);
            return Result.Success();
        }
    }
}