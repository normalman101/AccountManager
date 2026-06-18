using AccountManager.Core.Enums;

namespace AccountManager.Application.DTOs;

public record UpdateRequest
{
    public required string Email { init; get; }
    public required string Password { init; get; }
    public Role Role { init; get; }
};