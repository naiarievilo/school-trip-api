using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Application.Common.DTOs;
using DotNetFiveApiDemo.Application.Common.DTOs.Base;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.WebApi.DTOs;

namespace DotNetFiveApiDemo.Application.User.Interfaces
{
    public interface IUserApplicationService
    {
        public Task<Result<Dictionary<string, string>>> CreateUserAsync(UserCreationCommand command);
        public Task<Result<UserDto>> GetUserAsync(int userId);
        public Task<Result<Dictionary<string, string>>> LogInUserAsync(UserLoginCommand command);
        public Task<Result<UserDto>> UpdateUserAsync(UserUpdateCommand command, int userId);
        public Task<Result> DeleteUserAsync(int userId);
    }
}