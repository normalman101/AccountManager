using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.DTOs.Responses;
using AccountManager.Application.Interfaces;
using AccountManager.Core.Errors;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Application.UseCases;

public sealed class AccountAuthenticationUseCase(IAccountQueryRepository accountQueryRepository)
    : IExecutable<Task<Result<AuthenticateResponse>>, AuthenticateRequest>
{
    public async Task<Result<AuthenticateResponse>> Execute(AuthenticateRequest request)
    {
        var email = Email.Create(request.Email);

        if (email.IsFailure) return Result<AuthenticateResponse>.Failure(email.Error);

        var foundAccount = await accountQueryRepository.GetByEmail(email.Value!);

        if (foundAccount.IsFailure) return Result<AuthenticateResponse>.Failure(foundAccount.Error);

        if (foundAccount.Value!.Password.Value != request.Password)
        {
            return Result<AuthenticateResponse>.Failure(new Error(
                ErrorCode.IncorrectPassword,
                "Неверный пароль"
            ));
        }

        return Result<AuthenticateResponse>.Success(new AuthenticateResponse(
            foundAccount.Value.Email.Value,
            foundAccount.Value.Password.Value,
            foundAccount.Value.Role
        ));
    }
}