using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;

namespace AccountManager.Application.UseCases;

public class AccountInformationUpdateUseCase(IAccountCommandRepository accountCommandRepository)
    : IExecutable<Account, Task<bool>>
{
    public async Task<bool> Execute(Account account)
    {
        return await accountCommandRepository.Update(account);
    }
}