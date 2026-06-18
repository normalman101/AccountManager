namespace AccountManager.Application.DTOs;

public record RecoveryResponse
{
    public bool IsRecovered { get; init; }
}