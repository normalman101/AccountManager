using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Infrastructure.Repositories;

namespace AccountManager.Application.UseCases;

public class AccountRecoveryUseCase(AccountRepository accountRepository)
{
    public async Task<Account?> Execute(Account account)
    {
        var foundAccount = await accountRepository.GetByEmail(account.Email);

        if (foundAccount is null) return null;

        await accountRepository.Update(account);

        return account;
    }
}