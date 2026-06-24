using AccountManager.Core.Enums;

namespace AccountManager.Application.DTOs.Responses;

public sealed record AuthenticateResponse(
    string Email,
    string Password,
    Role Role
);