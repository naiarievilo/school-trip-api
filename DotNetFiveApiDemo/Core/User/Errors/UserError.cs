using DotNetFiveApiDemo.Core.Common.DTOs;

namespace DotNetFiveApiDemo.Core.User.Errors
{
    public sealed class UserError : Error
    {
        public const string UserIsLockedOutCode = "UserError.UserIsLockedOut";

        public const string FailedToGenerateEmailConfirmationTokenCode =
            "UserError.FailedToGenerateEmailConfirmationToken";

        public const string FailedToUpdateEmailCode = "UserError.FailedToUpdateEmailCode";
        public const string FailedToConfirmUserEmailCode = "UserError.FailedToConfirmUserEmail";
        public const string EmailAlreadyInUseCode = "UserError.EmailAlreadyInUse";
        public const string UserNotFoundCode = "UserError.UserNotFound";
        public const string UserEmailNotConfirmedCode = "UserError.UserEmailNotConfirmed";
        public const string FailedToSignUpUserCode = "UserError.FailedToSignUpUser";
        public const string FailedToUpdateUserInfoCode = "UserError.FailedToUpdateUserInfo";
        public const string FailedToDeleteUserCode = "UserError.FailedToDeleteUser";
        public const string FailedToSignInUserCode = "UserError.FailedToSignInUser";
        public const string FailedToResetPasswordCode = "UserError.FailedToResetPassword";

        private UserError(string code, string description) : base(code, description)
        {
        }

        public static Error UserNotFound(int userId)
        {
            return new UserError(UserNotFoundCode, $"User with id '{userId}' not found.");
        }

        public static Error UserNotFound(string email)
        {
            return new UserError(UserNotFoundCode, $"User with email '{email}' not found.");
        }

        public static Error EmailAlreadyInUse()
        {
            return new UserError(EmailAlreadyInUseCode, "Email is already in use.");
        }

        public static Error FailedToSignUpUser(string description)
        {
            return new UserError(FailedToSignUpUserCode, description);
        }

        public static Error FailedToUpdateUserInfo(string description)
        {
            return new UserError(FailedToUpdateUserInfoCode, description);
        }

        public static Error FailedToDeleteUser(string description)
        {
            return new UserError(FailedToDeleteUserCode, description);
        }

        public static Error FailedToSignInUser()
        {
            return new UserError(FailedToSignInUserCode, "Email and/or password are incorrect.");
        }

        public static Error UserIsLockedOut()
        {
            return new UserError(UserIsLockedOutCode, "User is locked out.");
        }

        public static Error UserEmailNotConfirmed()
        {
            return new UserError(UserEmailNotConfirmedCode, "User email is not confirmed.");
        }

        public static Error FailedToResetPassword(string errors)
        {
            return new UserError(FailedToResetPasswordCode, errors);
        }

        public static Error FailedToConfirmUserEmail(string email)
        {
            return new UserError(FailedToConfirmUserEmailCode, $"Failed to confirm '{email}'.");
        }

        public static Error FailedToGenerateEmailConfirmationToken()
        {
            return new UserError(FailedToGenerateEmailConfirmationTokenCode,
                "Failed to generate email confirmation token.");
        }

        public static Error FailedToUpdateEmail(string errors)
        {
            return new UserError(FailedToUpdateEmailCode, errors);
        }
    }
}