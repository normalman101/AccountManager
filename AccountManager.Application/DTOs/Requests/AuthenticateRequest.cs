namespace AccountManager.Application.DTOs.Requests;

public record AuthenticateRequest(
    string Email,
    string Password
);