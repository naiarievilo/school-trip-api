using SchoolTripApi.Application.Account;
using SchoolTripApi.Application.Account.Commands.UpdateAccountEmail;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Common.Security.Abstractions;

public interface IAccountManager
{
    Task<Result<AccountDto>> GetAccountInfoAsync(string email, CancellationToken cancellationToken);

    Task<Result<AccountDto>> GetAccountInfoAsync(AccountId accountId, CancellationToken cancellationToken);

    Task<Result<AccountDto>> CreateAccountAsync(string email, string password, CancellationToken cancellationToken);

    Task<Result> DeleteAccountAsync(AccountId accountId, CancellationToken cancellationToken);

    Task<Result<UpdateAccountEmailResult>> UpdateAccountEmailAsync(AccountId accountId, string newEmail,
        CancellationToken cancellationToken);

    Task<Result> UpdateAccountPasswordAsync(AccountId accountId, string currentPassword, string newPassword,
        CancellationToken cancellationToken);

    Task<Result> ResetAccountPasswordAsync(string email, string resetCode, string newPassword,
        CancellationToken cancellationToken);

    Task<Result> ConfirmAccountEmailAsync(string email, string confirmationToken,
        CancellationToken cancellationToken);
}