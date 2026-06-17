using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;

namespace AccountManager.Application.UseCases;

public class AccountRecoveryUseCase(
    IAccountQueryRepository accountQueryRepository,
    IAccountCommandRepository accountCommandRepository
) : IExecutable<Account, Task<Account?>>
{
    public async Task<Account?> Execute(Account account)
    {
        var foundAccount = await accountQueryRepository.GetByEmail(account.Email);

        if (foundAccount is null) return null;

        await accountCommandRepository.Update(account);

        return account;
    }
}