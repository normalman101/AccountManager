namespace AccountManager.Core.ValueObjects;

public record Password()
{
    public required string Value
    {
        init
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            field = value;
        }
        get;
    }
}