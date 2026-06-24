using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.DTOs.Responses;
using AccountManager.Application.Interfaces;
using AccountManager.Core.Entities;
using AccountManager.Core.Enums;
using AccountManager.Core.Errors;
using AccountManager.Core.Results;

namespace AccountManager.Application.UseCases;

public sealed class AccountRegistrationUseCase(
    IAccountCommandRepository accountCommandRepository,
    IAccountQueryRepository accountQueryRepository
) : IExecutable<Task<Result<RegisterResponse>>, RegisterRequest>
{
    public async Task<Result<RegisterResponse>> Execute(RegisterRequest request)
    {
        var account = Account.Create(
            request.Email,
            request.Password,
            Role.Normal
        );

        if (account.IsFailure) return Result<RegisterResponse>.Failure(account.Error);

        var foundAccount = await accountQueryRepository.GetByEmail(account.Value!.Email);

        if (foundAccount.IsSuccess)
        {
            return Result<RegisterResponse>.Failure(new Error(
                ErrorCode.AccountAlreadyExists,
                "Аккаунт с такой почтой уже существует"
            ));
        }

        var result = await accountCommandRepository.Add(account.Value);

        return result.IsSuccess
            ? Result<RegisterResponse>.Success(new RegisterResponse(true))
            : Result<RegisterResponse>.Failure(result.Error);
    }
}