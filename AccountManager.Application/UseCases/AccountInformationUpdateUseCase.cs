using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Infrastructure.Repositories;

namespace AccountManager.Application.UseCases;

public class AccountInformationUpdateUseCase(AccountRepository accountRepository)
{
    public async Task<bool> Execute(Account account)
    {
        return await accountRepository.Update(account);
    }
}