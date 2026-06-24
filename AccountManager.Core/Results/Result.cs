using System;
using AccountManager.Core.Errors;

namespace AccountManager.Core.Results;

public sealed class Result<TValue>
{
    private Result(bool isSuccess, TValue? value, Error error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public TValue? Value { get; }
    public Error Error { get; }

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(true, value, new Error(ErrorCode.None, string.Empty));
    }

    public static Result<TValue> Failure(Error error)
    {
        return new Result<TValue>(false, default, error);
    }
}