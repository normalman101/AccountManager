using System;
using AccountManager.Core.Errors;
using AccountManager.Core.Results;

namespace AccountManager.Core.ValueObjects;

public class Password
{
    private Password(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Password> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Result<Password>.Failure(new Error(
                ErrorCode.PasswordIsEmpty,
                "Пароль пустой"
            ));
        }

        return Result<Password>.Success(new Password(value));
    }
}