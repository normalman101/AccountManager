using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.DTOs.Responses;
using AccountManager.Application.Interfaces;
using AccountManager.Core.Entities;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Application.UseCases;

public class AccountInformationUpdateUseCase(
    IAccountQueryRepository accountQueryRepository,
    IAccountCommandRepository accountCommandRepository
)
    : IExecutable<Task<Result<UpdateResponse>>, UpdateRequest>
{
    public async Task<Result<UpdateResponse>> Execute(UpdateRequest request)
    {
        var oldEmail = Email.Create(request.OldEmail);
        var newAccount = Account.Create(
            request.NewEmail,
            request.NewPassword,
            request.NewRole
        );

        if (oldEmail.IsFailure) return Result<UpdateResponse>.Failure(oldEmail.Error);
        if (newAccount.IsFailure) return Result<UpdateResponse>.Failure(newAccount.Error);

        var result = await accountQueryRepository.GetByEmail(oldEmail.Value!);

        if (result.IsFailure) return Result<UpdateResponse>.Failure(result.Error);

        await accountCommandRepository.Update(oldEmail.Value!, newAccount.Value!);

        return Result<UpdateResponse>.Success(new UpdateResponse(true));
    }
}