using AccountManager.Core.Enums;

namespace AccountManager.Presentation.DTOs;

public record AccountLoggedIn(
    string Email,
    string Password,
    Role Role
);