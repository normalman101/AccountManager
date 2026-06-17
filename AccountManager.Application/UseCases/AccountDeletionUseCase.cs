using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;
using AccountManager.Infrastructure.Repositories;

namespace AccountManager.Application.UseCases;

public class AccountDeletionUseCase(AccountRepository accountRepository) : IExecutable<Account, Task<bool>>
{
    public async Task<bool> Execute(Account account)
    {
        return await accountRepository.Delete(account);
    }
}