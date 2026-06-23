using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.DTOs.Responses;
using AccountManager.Application.Interfaces;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Application.UseCases;

public class AccountDeletionUseCase(IAccountCommandRepository accountCommandRepository)
    : IExecutable<Task<Result<DeleteResponse>>, DeleteRequest>
{
    public async Task<Result<DeleteResponse>> Execute(DeleteRequest request)
    {
        var email = Email.Create(request.Email);

        if (email.IsFailure) return Result<DeleteResponse>.Failure(email.Error);

        var result = await accountCommandRepository.Delete(email.Value!);

        return result.IsSuccess
            ? Result<DeleteResponse>.Success(new DeleteResponse(true))
            : Result<DeleteResponse>.Failure(result.Error);
    }
}