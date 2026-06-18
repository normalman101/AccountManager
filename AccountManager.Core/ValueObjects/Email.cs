using System;

namespace AccountManager.Core.ValueObjects;

public record Email
{
    public required string Value
    {
        init
        {
            if (string.IsNullOrWhiteSpace(value)) throw new Exception("Почта отсутствует");

            if (!value.Contains('@') && value.Contains('.')) throw new Exception("Почта не валидна");

            field = value;
        }
        get;
    }
}