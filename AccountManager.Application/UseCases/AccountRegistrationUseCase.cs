using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;

namespace AccountManager.Application.UseCases;

public class AccountRegistrationUseCase(
    IAccountCommandRepository accountCommandRepository,
    IAccountQueryRepository accountQueryRepository
) : IExecutable<Account, Task<bool>>
{
    public async Task<bool> Execute(Account account)
    {
        var foundAccount = accountQueryRepository.GetByEmail(account.Email).Result;
        
        if (foundAccount is not null) return false;
        
        await accountCommandRepository.Add(account);

        return true;
    }
}