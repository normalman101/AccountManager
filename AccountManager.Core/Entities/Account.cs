using AccountManager.Core.Enums;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Core.Entities;

public sealed class Account
{
    private Account(Email email, Password password, Role role)
    {
        Email = email;
        Password = password;
        Role = role;
    }

    public Email Email { init; get; }
    public Password Password { init; get; }
    public Role Role { init; get; }

    public static Result<Account> Create(string email, string password, Role role)
    {
        var emailResult = Email.Create(email);
        var passwordResult = Password.Create(password);

        if (emailResult.IsFailure)
        {
            return Result<Account>.Failure(emailResult.Error);
        }

        if (passwordResult.IsFailure)
        {
            return Result<Account>.Failure(passwordResult.Error);
        }

        return Result<Account>.Success(new Account(
            emailResult.Value!,
            passwordResult.Value!,
            role
        ));
    }
}