namespace SchoolTripApi.Application.Account.Commands.UpdateAccountEmail;

public class UpdateAccountEmailResult(bool isEmailConfirmed)
{
    public bool IsEmailConfirmed { get; private set; } = isEmailConfirmed;

    public static UpdateAccountEmailResult From(bool isEmailConfirmed)
    {
        return new UpdateAccountEmailResult(isEmailConfirmed);
    }
}