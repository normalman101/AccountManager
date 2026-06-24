using AccountManager.Core.Enums;

namespace AccountManager.Application.DTOs.Requests;

public sealed record UpdateRequest(
    string OldEmail,
    string NewEmail,
    string NewPassword,
    Role NewRole
);