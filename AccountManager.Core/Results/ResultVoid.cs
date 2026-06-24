using System;
using AccountManager.Core.Errors;

namespace AccountManager.Core.Results;

public sealed class ResultVoid
{
    private ResultVoid(bool isSuccess, Error error)
    {
        if (isSuccess && error.Code != ErrorCode.None || !isSuccess && error.Code == ErrorCode.None)
        {
            throw new ArgumentException("Нерпавильные аргументы результата");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static ResultVoid Success()
    {
        return new ResultVoid(true, new Error(ErrorCode.None, string.Empty));
    }

    public static ResultVoid Failure(Error error)
    {
        return new ResultVoid(false, error);
    }
}