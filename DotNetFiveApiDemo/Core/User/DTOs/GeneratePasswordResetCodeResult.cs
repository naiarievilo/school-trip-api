namespace DotNetFiveApiDemo.Core.User.DTOs
{
    public class GeneratePasswordResetCodeResult
    {
        public GeneratePasswordResetCodeResult(string passwordResetCode)
        {
            PasswordResetCode = passwordResetCode;
        }

        public string PasswordResetCode { get; init; }
    }
}