using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Infrastructure.Repositories;

namespace AccountManager.Application.UseCases;

public class AccountRegistrationUseCase(AccountRepository accountRepository)
{
    public async Task<bool> Execute(Account account)
    {
        return await accountRepository.Add(account);
    }
}