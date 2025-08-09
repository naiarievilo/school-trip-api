using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Application.Account;

public sealed class AccountError(string code, string description) : Error(code, description)
{
    public const string UserIsLockedOutCode = "UserError.UserIsLockedOut";

    private const string FailedToIssueEmailConfirmationTokenCode =
        "UserError.FailedToGenerateEmailConfirmationToken";

    private const string FailedToUpdateEmailCode = "UserError.FailedToUpdateEmailCode";
    private const string FailedToConfirmUserEmailCode = "UserError.FailedToConfirmUserEmail";
    private const string EmailAlreadyInUseCode = "UserError.EmailAlreadyInUse";
    private const string UserNotFoundCode = "UserError.UserNotFound";
    private const string UserEmailNotConfirmedCode = "UserError.UserEmailNotConfirmed";
    private const string FailedToSignUpUserCode = "UserError.FailedToSignUpUser";
    private const string FailedToUpdateUserInfoCode = "UserError.FailedToUpdateUserInfo";
    private const string FailedToDeleteUserCode = "UserError.FailedToDeleteUser";
    private const string FailedToSignInUserCode = "UserError.FailedToSignInUser";
    private const string FailedToResetPasswordCode = "UserError.FailedToResetPassword";

    public static Error UserNotFound(Guid accountId)
    {
        return new AccountError(UserNotFoundCode, $"User with id '{accountId}' not found.");
    }

    public static Error UserNotFound(string email)
    {
        return new AccountError(UserNotFoundCode, $"User with email '{email}' not found.");
    }

    public static Error EmailAlreadyInUse()
    {
        return new AccountError(EmailAlreadyInUseCode, "Email is already in use.");
    }

    public static Error FailedToSignUpUser(string description)
    {
        return new AccountError(FailedToSignUpUserCode, description);
    }

    public static Error FailedToUpdateUserInfo(string description)
    {
        return new AccountError(FailedToUpdateUserInfoCode, description);
    }

    public static Error FailedToDeleteUser(string description)
    {
        return new AccountError(FailedToDeleteUserCode, description);
    }

    public static Error FailedToSignInUser()
    {
        return new AccountError(FailedToSignInUserCode, "Email and/or password are incorrect.");
    }

    public static Error UserIsLockedOut()
    {
        return new AccountError(UserIsLockedOutCode, "User is locked out.");
    }

    public static Error UserEmailNotConfirmed()
    {
        return new AccountError(UserEmailNotConfirmedCode, "User email is not confirmed.");
    }

    public static Error FailedToResetPassword(string errors)
    {
        return new AccountError(FailedToResetPasswordCode, errors);
    }

    public static Error FailedToConfirmUserEmail(string email)
    {
        return new AccountError(FailedToConfirmUserEmailCode, $"Failed to confirm '{email}'.");
    }

    public static Error FailedToGenerateEmailConfirmationToken()
    {
        return new AccountError(FailedToIssueEmailConfirmationTokenCode,
            "Failed to generate email confirmation token.");
    }

    public static Error FailedToUpdateEmail(string errors)
    {
        return new AccountError(FailedToUpdateEmailCode, errors);
    }
}