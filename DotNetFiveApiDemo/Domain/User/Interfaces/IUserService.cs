using System.Threading.Tasks;
using DotNetFiveApiDemo.Application.Common.DTOs;

namespace DotNetFiveApiDemo.Domain.User.Interfaces
{
    public interface IUserService<TUser> where TUser : class
    {
        Task<Result<TUser>> GetUserAsync(int userId);
        Task<Result<TUser>> GetUserAsync(string email);
        Task<Result<TUser>> CreateUserAsync(TUser user);
        Task<Result<TUser>> ValidateCredentials(TUser user);
        Task<Result<TUser>> UpdateUserAsync(TUser user, string currentPassword, string newPassword);
        Task<Result<TUser>> DeleteUserAsync(int userId);
    }
}