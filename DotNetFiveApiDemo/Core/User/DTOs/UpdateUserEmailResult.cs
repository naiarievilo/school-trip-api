namespace DotNetFiveApiDemo.Core.User.DTOs
{
    public class UpdateUserEmailResult
    {
        public UpdateUserEmailResult(bool isEmailConfirmed)
        {
            IsEmailConfirmed = isEmailConfirmed;
        }

        public bool IsEmailConfirmed { get; init; }
    }
}