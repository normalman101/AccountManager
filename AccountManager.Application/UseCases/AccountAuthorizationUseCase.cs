using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;
using AccountManager.Infrastructure.Repositories;

namespace AccountManager.Application.UseCases;

public class AccountAuthorizationUseCase(AccountRepository accountRepository) : IExecutable<Account, Task<Account?>>
{
    public async Task<Account?> Execute(Account account)
    {
        var foundAccount = await accountRepository.GetByEmail(account.Email);
        
        if (foundAccount is null) return null;

        return account.Password.Value != foundAccount.Password.Value ? null : account;
    } 
}