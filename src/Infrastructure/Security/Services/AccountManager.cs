using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Application.Accounts.Commands.UpdateAccountEmail;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Application.Accounts.Errors;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Application.Common.DTOs;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Infrastructure.Data;
using SchoolTripApi.Infrastructure.Security.Entities;
using AccountId = SchoolTripApi.Domain.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Infrastructure.Security.Services;

internal sealed class AccountManager(
    UserManager<Account> userManager,
    AppDbContext context,
    IAppLogger<AccountManager> logger,
    IMapper mapper)
    : IAccountManager
{
    public async Task<Result> ResetAccountPasswordAsync(string email, string resetCode, string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return Result.Failure(AccountError.UserNotFound(email));

        var userIsLockedOut = await userManager.IsLockedOutAsync(user);
        if (userIsLockedOut) await userManager.SetLockoutEnabledAsync(user, false);

        var resetPassword = await userManager.ResetPasswordAsync(user, resetCode, newPassword);
        if (resetPassword.Succeeded) return Result.Success();

        var errors = FormatIdentityErrors(resetPassword.Errors);
        return Result.Failure(AccountError.FailedToResetPassword(errors));
    }

    public async Task<Result> ConfirmAccountEmailAsync(string email, string confirmationToken,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return Result.Failure(AccountError.UserNotFound(email));

        var confirmEmail = await userManager.ConfirmEmailAsync(user, confirmationToken);
        return confirmEmail.Succeeded
            ? Result.Success()
            : Result.Failure(AccountError.FailedToConfirmUserEmail(user.Email!));
    }

    public async Task<Result<AccountDto>> GetAccountInfoAsync(string email,
        CancellationToken cancellationToken = default)
    {
        var account = await userManager.FindByEmailAsync(email);
        if (account is null) return Result.Failure<AccountDto>(AccountError.UserNotFound(email));

        var accountDto = await BuildAccountDto(account);
        return Result.Success(accountDto);
    }

    public async Task<Result<PageOf<AccountDto>>> GetAccountsInfoAsync(PaginationDetails paginationDetails,
        CancellationToken cancellationToken)
    {
        var pageNumber = paginationDetails.PageNumber;
        var pageSize = paginationDetails.PageSize;

        List<Account> requestedAccountPage;
        int totalCount;
        int totalPages;

        try
        {
            totalCount = await context.Users.CountAsync(cancellationToken);
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            requestedAccountPage = await context.Users
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve accounts info: {1}", ex.Message);
            return Result.Failure<PageOf<AccountDto>>(AccountError.FailedToRetrieveAccountsInfo);
        }

        var accountDtoPage = requestedAccountPage.Select(mapper.Map<AccountDto>).ToList();
        return Result.Success(new PageOf<AccountDto>(accountDtoPage, pageNumber, pageSize, totalCount, totalPages));
    }

    public async Task<Result<AccountDto>> GetAccountInfoAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        var account = await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId.Value), cancellationToken);
        if (account is null) return Result.Failure<AccountDto>(AccountError.UserNotFound(accountId.Value));

        var accountDto = await BuildAccountDto(account);
        return Result.Success(accountDto);
    }

    public async Task<Result> UpdateAccountPasswordAsync(AccountId accountId, string currentPassword,
        string newPassword, CancellationToken cancellationToken)
    {
        var userAccount =
            await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId.Value), cancellationToken);
        if (userAccount is null) return Result.Failure(AccountError.UserNotFound(accountId.Value));

        var passwordsMatch = await userManager.CheckPasswordAsync(userAccount, currentPassword);
        if (!passwordsMatch)
            return Result.Failure(AccountError.FailedToUpdateUserInfo("Current password is incorrect."));

        var updatePassword = await userManager.ChangePasswordAsync(userAccount, currentPassword, newPassword);
        if (updatePassword.Succeeded) return Result.Success();

        var errors = FormatIdentityErrors(updatePassword.Errors);
        return Result.Failure(AccountError.FailedToUpdateUserInfo(errors));
    }

    public async Task<Result> DeleteAccountAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        var userAccount =
            await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId.Value), cancellationToken);
        if (userAccount is null) return Result.Failure(AccountError.UserNotFound(accountId.Value));

        var deleteUser = await userManager.DeleteAsync(userAccount);
        if (deleteUser.Succeeded) return Result.Success<Guardian>();

        var errors = FormatIdentityErrors(deleteUser.Errors);
        logger.LogInformation("Failed to delete user: {1}", errors);
        return Result.Failure<Guardian>(AccountError.FailedToDeleteUser(errors));
    }

    public async Task<Result<AccountDto>> CreateAccountAsync(string email, string password,
        CancellationToken cancellationToken)
    {
        var userAccount = await userManager.FindByEmailAsync(email);
        if (userAccount is not null)
            return Result.Failure<AccountDto>(AccountError.EmailAlreadyInUse);

        var userName = GetEmailUserName(email);
        userAccount = new Account { Email = email, UserName = userName };

        var createUser = await userManager.CreateAsync(userAccount, password);
        if (!createUser.Succeeded)
        {
            var errors = FormatIdentityErrors(createUser.Errors);
            return Result.Failure<AccountDto>(AccountError.FailedToSignUpUser(errors));
        }

        var getAccountInfo = await GetAccountInfoAsync(email, cancellationToken);
        if (getAccountInfo.Failed) return Result.Failure<AccountDto>(getAccountInfo.Error);
        var accountInfo = getAccountInfo.Value;

        return Result.Success(accountInfo);
    }

    public async Task<Result<UpdateAccountEmailResult>> UpdateAccountEmailAsync(AccountId accountId,
        string newEmail, CancellationToken cancellationToken)
    {
        var userAccount =
            await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId.Value), cancellationToken);
        if (userAccount is null)
            return Result.Failure<UpdateAccountEmailResult>(AccountError.UserNotFound(accountId.Value));

        if (newEmail.Equals(userAccount.Email))
            return Result.Failure<UpdateAccountEmailResult>(AccountError.EmailAlreadyInUse);

        var newEmailUserAccount = await userManager.FindByEmailAsync(newEmail);
        if (newEmailUserAccount is not null)
            return Result.Failure<UpdateAccountEmailResult>(AccountError.EmailAlreadyInUse);

        userAccount.Email = newEmail;
        userAccount.UserName = GetEmailUserName(newEmail);
        var updateUser = await userManager.UpdateAsync(userAccount);
        if (updateUser.Succeeded) return Result.Success(UpdateAccountEmailResult.From(userAccount.EmailConfirmed));

        var errors = FormatIdentityErrors(updateUser.Errors);
        logger.LogInformation("Failed to update user email: {1}", errors);
        return Result.Failure<UpdateAccountEmailResult>(AccountError.FailedToUpdateEmail(errors));
    }

    private string FormatIdentityErrors(IEnumerable<IdentityError> errors)
    {
        // All IdentityError descriptions end with a period.
        return string.Join(" ", errors.Select(e => e.Description));
    }

    public static string GetEmailUserName(string email)
    {
        return email[..email.IndexOf('@')];
    }

    private async Task<AccountDto> BuildAccountDto(Account account)
    {
        var accountRoles = await userManager.GetRolesAsync(account);
        var accountDto = new AccountDto
        {
            Id = account.Id,
            // Email is required
            Email = account.Email!,
            IsEmailConfirmed = account.EmailConfirmed,
            PhoneNumber = account.PhoneNumber,
            Roles = accountRoles.ToList()
        };

        return accountDto;
    }
}