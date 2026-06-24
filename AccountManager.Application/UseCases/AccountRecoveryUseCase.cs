using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.DTOs.Responses;
using AccountManager.Application.Interfaces;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Application.UseCases;

public sealed class AccountRecoveryUseCase(
    IAccountQueryRepository accountQueryRepository,
    IAccountCommandRepository accountCommandRepository
) : IExecutable<Task<Result<RecoveryResponse>>, RecoveryRequest>
{
    public async Task<Result<RecoveryResponse>> Execute(RecoveryRequest request)
    {
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);

        if (email.IsFailure) return Result<RecoveryResponse>.Failure(email.Error);

        if (password.IsFailure) return Result<RecoveryResponse>.Failure(password.Error);

        var foundAccount = await accountQueryRepository.GetByEmail(email.Value!);

        if (foundAccount.IsFailure) return Result<RecoveryResponse>.Failure(foundAccount.Error);

        var result = await accountCommandRepository.Recover(
            email.Value!,
            password.Value!
        );

        return result.IsSuccess
            ? Result<RecoveryResponse>.Success(new RecoveryResponse(true))
            : Result<RecoveryResponse>.Failure(result.Error);
    }
}