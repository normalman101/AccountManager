using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Infrastructure.Repositories;

namespace AccountManager.Application.UseCases;

public class AccountRegistrationUseCase(AccountRepository accountRepository)
{
    public async Task Execute(Account account)
    {
        await accountRepository.Add(account);
    }
}