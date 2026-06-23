using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Application.Interfaces;

public interface IAccountCommandRepository
{
    Task<ResultVoid> Add(Account account);

    Task Update(Email oldAccountEmail, Account newAccount);

    Task<ResultVoid> Recover(Email email, Password newPassword);

    Task<ResultVoid> Delete(Email email);
}