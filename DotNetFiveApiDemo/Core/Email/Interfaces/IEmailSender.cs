using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;

namespace DotNetFiveApiDemo.Core.Email.Interfaces
{
    public interface IEmailSender
    {
        public Task<Result> SendEmailAsync(string email, string subject, string htmlMessage);
    }
}