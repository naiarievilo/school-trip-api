using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolTripApi.Application.Account;
using SchoolTripApi.Application.Account.Commands.CreateAccount;
using SchoolTripApi.Application.Account.Commands.UpdateAccountEmail;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Guardian.GuardianAggregate;
using SchoolTripApi.Infrastructure.Security.Entities;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Infrastructure.Security.Services;

public class AccountManager(
    UserManager<Account> userManager,
    IAppLogger<AccountManager> logger,
    IRepository<Guardian> guardianRepository,
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
        var userAccount = await userManager.FindByEmailAsync(email);
        if (userAccount is null) return Result.Failure<AccountDto>(AccountError.UserNotFound(email));

        var userAccountDto = mapper.Map<AccountDto>(userAccount);
        return Result.Success(userAccountDto);
    }

    public async Task<Result<AccountDto>> GetAccountInfoAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        var userAccount = await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId), cancellationToken);
        if (userAccount is null) return Result.Failure<AccountDto>(AccountError.UserNotFound(accountId));

        var userAccountDto = mapper.Map<AccountDto>(userAccount);
        return Result.Success(userAccountDto);
    }

    public async Task<Result<UpdateAccountEmailResult>> UpdateAccountEmailAsync(AccountId accountId,
        string newEmail, CancellationToken cancellationToken = default)
    {
        var userAccount = await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId), cancellationToken);
        if (userAccount is null) return Result.Failure<UpdateAccountEmailResult>(AccountError.UserNotFound(accountId));

        if (newEmail.Equals(userAccount.Email))
            return Result.Failure<UpdateAccountEmailResult>(AccountError.EmailAlreadyInUse);

        var newEmailUserAccount = await userManager.FindByEmailAsync(newEmail);
        if (newEmailUserAccount is not null)
            return Result.Failure<UpdateAccountEmailResult>(AccountError.EmailAlreadyInUse);

        userAccount.Email = newEmail;
        userAccount.UserName = GetEmailUsername(newEmail);
        var updateUser = await userManager.UpdateAsync(userAccount);
        if (updateUser.Succeeded) return Result.Success(UpdateAccountEmailResult.From(userAccount.EmailConfirmed));

        var errors = FormatIdentityErrors(updateUser.Errors);
        logger.LogInformation("Failed to update user email: {1}", errors);
        return Result.Failure<UpdateAccountEmailResult>(AccountError.FailedToUpdateEmail(errors));
    }

    public async Task<Result> UpdateAccountPasswordAsync(AccountId accountId, string currentPassword,
        string newPassword, CancellationToken cancellationToken = default)
    {
        var userAccount = await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId), cancellationToken);
        if (userAccount is null) return Result.Failure(AccountError.UserNotFound(accountId));

        var passwordsMatch = await userManager.CheckPasswordAsync(userAccount, currentPassword);
        if (!passwordsMatch)
            return Result.Failure(AccountError.FailedToUpdateUserInfo("Current password is incorrect."));

        var updatePassword = await userManager.ChangePasswordAsync(userAccount, currentPassword, newPassword);
        if (updatePassword.Succeeded) return Result.Success();

        var errors = FormatIdentityErrors(updatePassword.Errors);
        return Result.Failure(AccountError.FailedToUpdateUserInfo(errors));
    }

    public async Task<Result> DeleteAccountAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        var userAccount = await userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(accountId), cancellationToken);
        if (userAccount is null) return Result.Failure(AccountError.UserNotFound(accountId));

        var deleteUser = await userManager.DeleteAsync(userAccount);
        if (deleteUser.Succeeded) return Result.Success<Guardian>();

        var errors = FormatIdentityErrors(deleteUser.Errors);
        logger.LogInformation("Failed to delete user: {1}", errors);
        return Result.Failure<Guardian>(AccountError.FailedToDeleteUser(errors));
    }

    public async Task<Result<AccountDto>> CreateAccountAsync(CreateAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        var email = command.Email;
        var password = command.Password;
        var fullName = command.FullName;
        var phoneNumber = command.PhoneNumber;

        var userAccount = await userManager.FindByEmailAsync(email);
        if (userAccount is not null)
            return Result.Failure<AccountDto>(AccountError.EmailAlreadyInUse);

        userAccount = new Account
        {
            UserName = GetEmailUsername(email),
            Email = email,
            PhoneNumber = phoneNumber
        };

        var createUser = await userManager.CreateAsync(userAccount, password);
        if (!createUser.Succeeded)
        {
            var errors = FormatIdentityErrors(createUser.Errors);
            return Result.Failure<AccountDto>(AccountError.FailedToSignUpUser(errors));
        }

        var guardian = new Guardian(userAccount.Id, fullName);
        await guardianRepository.AddAsync(guardian, cancellationToken);

        return Result.Success(new AccountDto
        {
            Id = userAccount.Id,
            Email = userAccount.Email,
            IsEmailConfirmed = userAccount.EmailConfirmed,
            PhoneNumber = userAccount.PhoneNumber
        });
    }

    private string FormatIdentityErrors(IEnumerable<IdentityError> errors)
    {
        // All IdentityError descriptions end with a period.
        return string.Join(" ", errors.Select(e => e.Description));
    }

    private string GetEmailUsername(string email)
    {
        return email[..email.IndexOf('@')];
    }
}