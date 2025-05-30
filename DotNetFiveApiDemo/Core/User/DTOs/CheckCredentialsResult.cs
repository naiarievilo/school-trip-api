namespace DotNetFiveApiDemo.Core.User.DTOs
{
    public class CheckCredentialsResult<TUser>
    {
        public CheckCredentialsResult(TUser user, bool requiresTwoFactor = false)
        {
            User = user;
            RequiresTwoFactor = requiresTwoFactor;
        }

        public TUser User { get; init; }
        public bool RequiresTwoFactor { get; init; }
    }
}