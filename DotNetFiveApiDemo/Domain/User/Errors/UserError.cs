using DotNetFiveApiDemo.Application.Common.DTOs;

namespace DotNetFiveApiDemo.Domain.User.Errors
{
    public sealed class UserError : Error
    {
        private UserError(string code, string description) : base(code, description)
        {
        }

        public static string UserNotFoundCode => "UserError.UserNotFound";
        public static string FailedToCreateUserCode => "UserError.FailedToCreateUser";
        public static string FailedToUpdateUserCode => "UserError.FailedToCreateOrder";
        public static string FailedToDeleteUserCode => "UserError.FailedToDeleteUser";
        public static string FailedToLogInUserCode => "UserError.FailedToLogInUser";

        public static Error UserNotFound()
        {
            return new UserError(UserNotFoundCode, "User not found");
        }

        public static Error UserNotFound(int userId)
        {
            return new UserError(UserNotFoundCode, $"User with id {userId} not found.");
        }

        public static Error UserNotFound(string email)
        {
            return new UserError(UserNotFoundCode, $"User with email {email} not found.");
        }

        public static Error FailedToCreateUser()
        {
            return new UserError(FailedToCreateUserCode, "Failed to create user");
        }

        public static Error FailedToCreateUser(string description)
        {
            return new UserError(FailedToCreateUserCode, description);
        }

        public static Error FailedToUpdateUser()
        {
            return new UserError(FailedToUpdateUserCode, "Failed to update user");
        }

        public static Error FailedToUpdateUser(string description)
        {
            return new UserError(FailedToUpdateUserCode, description);
        }

        public static Error FailedToDeleteUser()
        {
            return new UserError(FailedToDeleteUserCode, "Failed to delete user");
        }

        public static Error FailedToDeleteUser(string description)
        {
            return new UserError(FailedToDeleteUserCode, description);
        }

        public static Error FailedToLogInUser(string field)
        {
            return new UserError(FailedToLogInUserCode, $"{field} and/or password are incorrect.");
        }
    }
}