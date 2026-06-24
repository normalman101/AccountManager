using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Application.DTOs;
using AccountManager.Application.Interfaces;

namespace AccountManager.Application.UseCases;

public sealed class GetAllAccountsUseCase(IAccountQueryRepository accountQueryRepository)
{
    public async Task<IEnumerable<AccountDto>> Execute()
    {
        var accounts = await accountQueryRepository.GetAll();

        return accounts.Select(a => new AccountDto
        {
            Email = a.Email.Value,
            Password = a.Password.Value,
            Role = a.Role
        });
    }
}