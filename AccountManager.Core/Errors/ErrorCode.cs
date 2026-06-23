namespace AccountManager.Core.Errors;

public enum ErrorCode
{
    None,
    EmailIsEmpty,
    EmailIsNotValid,
    PasswordIsEmpty,
    AccountHasNotBeenAdded,
    AccountHasNotBeenUpdated,
    AccountHasNotBeenRecovered,
    AccountHasNotBeenDeleted,
    AccountHasNotBeenFound,
    AccountAlreadyExists,
    IncorrectPassword,
}