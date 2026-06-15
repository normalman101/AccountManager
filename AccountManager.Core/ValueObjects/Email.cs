namespace AccountManager.Core.ValueObjects;

public record Email
{
    public required string Value
    {
        init
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            if (!value.Contains('@') && value.Contains('.')) return;

            field = value;
        }
        get;
    }
}