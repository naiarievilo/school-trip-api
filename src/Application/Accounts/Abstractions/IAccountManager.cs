using SchoolTripApi.Application.Accounts.Commands.UpdateAccountEmail;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Application.Common.DTOs;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Accounts.Abstractions;

public interface IAccountManager
{
    Task<Result<AccountDto>> GetAccountInfoAsync(string email, CancellationToken cancellationToken);

    Task<Result<PageOf<AccountDto>>> GetAccountsInfoAsync(PaginationDetails paginationDetails,
        CancellationToken cancellationToken);

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