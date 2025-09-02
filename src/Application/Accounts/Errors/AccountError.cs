using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Application.Accounts.Errors;

public sealed class AccountError(string code, string description) : Error(code, description)
{
    private const string UserIsLockedOutCode = "UserError.UserIsLockedOut";

    private const string FailedToIssueEmailConfirmationTokenCode =
        "AccountError.FailedToGenerateEmailConfirmationToken";

    private const string FailedToRetrieveAccountsInfoCode = "AccountError.FailedToRetrieveAccountsInfo";
    private const string FailedToUpdateEmailCode = "AccountError.FailedToUpdateEmailCode";
    private const string FailedToConfirmUserEmailCode = "AccountError.FailedToConfirmUserEmail";
    private const string EmailAlreadyInUseCode = "AccountError.EmailAlreadyInUse";
    private const string UserNotFoundCode = "AccountError.UserNotFound";
    private const string UserEmailNotConfirmedCode = "AccountError.UserEmailNotConfirmed";
    private const string FailedToSignUpUserCode = "AccountError.FailedToSignUpUser";
    private const string FailedToUpdateUserInfoCode = "AccountError.FailedToUpdateUserInfo";
    private const string FailedToDeleteUserCode = "AccountError.FailedToDeleteUser";
    private const string FailedToSignInUserCode = "AccountError.FailedToSignInUser";
    private const string FailedToResetPasswordCode = "AccountError.FailedToResetPassword";

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

    public static Error FailedToRetrieveAccountsInfo()
    {
        return new AccountError(FailedToRetrieveAccountsInfoCode, "Failed to retrieve the accounts info.");
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