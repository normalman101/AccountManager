using AccountManager.Core.Enums;

namespace AccountManager.Presentation.DTOs;

public sealed record AccountLoggedIn(
    string Email,
    string Password,
    Role Role
);