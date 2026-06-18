using AccountManager.Core.Enums;

namespace AccountManager.Application.DTOs;

public record RegisterResponse
{
    public required string Email { init; get; }
    public required string Password { init; get; }
    public required Role Role { init; get; }
}