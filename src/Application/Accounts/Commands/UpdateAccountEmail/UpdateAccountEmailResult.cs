namespace SchoolTripApi.Application.Accounts.Commands.UpdateAccountEmail;

public sealed class UpdateAccountEmailResult(bool isEmailConfirmed)
{
    public bool IsEmailConfirmed { get; private set; } = isEmailConfirmed;

    public static UpdateAccountEmailResult From(bool isEmailConfirmed)
    {
        return new UpdateAccountEmailResult(isEmailConfirmed);
    }
}