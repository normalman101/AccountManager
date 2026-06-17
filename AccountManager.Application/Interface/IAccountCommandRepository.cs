using System.Threading.Tasks;
using AccountManager.Core.Entities;

namespace AccountManager.Application.Interface;

public interface IAccountCommandRepository
{
    Task Add(Account account);

    Task<bool> Update(Account account);

    Task<bool> Delete(Account account);
}