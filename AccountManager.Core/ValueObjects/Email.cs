using AccountManager.Core.Errors;
using AccountManager.Core.Results;

namespace AccountManager.Core.ValueObjects;

public sealed class Email
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Result<Email>.Failure(new Error(
                ErrorCode.EmailIsEmpty,
                "Почта пустая"
            ));
        }

        if (!(value.Contains('@') && value.Contains('.')))
        {
            Result<Email>.Failure(new Error(
                ErrorCode.EmailIsNotValid,
                "Почта невалидна"
            ));
        }

        return Result<Email>.Success(new Email(value));
    }
}