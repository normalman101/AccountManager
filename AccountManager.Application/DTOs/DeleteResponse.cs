namespace AccountManager.Application.DTOs;

public record DeleteResponse
{
    public bool IsDeleted { init; get; }
}