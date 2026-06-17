using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;

namespace AccountManager.Application.UseCases;

public class AccountRegistrationUseCase(IAccountCommandRepository accountCommandRepository)
    : IExecutable<Account, Task>
{
    public async Task Execute(Account account)
    {
        await accountCommandRepository.Add(account);
    }
}