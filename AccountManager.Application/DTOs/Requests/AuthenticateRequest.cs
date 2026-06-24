namespace AccountManager.Application.DTOs.Requests;

public sealed record AuthenticateRequest(
    string Email,
    string Password
);