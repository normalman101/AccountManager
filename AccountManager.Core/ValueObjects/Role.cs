using System;

namespace AccountManager.Core.ValueObjects;

public record Role()
{
    public required string Value
    {
        init
        {
            if (string.IsNullOrWhiteSpace(value)) throw new Exception("Роль отсутствует");

            field = value;
        }
        get;
    }
}