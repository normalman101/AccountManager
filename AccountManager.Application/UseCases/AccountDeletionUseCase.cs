using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Infrastructure.Repositories;

namespace AccountManager.Application.UseCases;

public class AccountDeletionUseCase(AccountRepository accountRepository)
{
    public async Task<bool> Execute(Account account)
    {
        return await accountRepository.Delete(account);
    }
}