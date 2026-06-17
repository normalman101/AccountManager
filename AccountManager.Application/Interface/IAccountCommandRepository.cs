using System.Threading.Tasks;
using AccountManager.Core.Entities;

namespace AccountManager.Application.Interface;

public interface IAccountCommandRepository
{
    Task<bool> Add(Account account);

    Task<bool> Update(Account account);

    Task<bool> Delete(Account account);
}