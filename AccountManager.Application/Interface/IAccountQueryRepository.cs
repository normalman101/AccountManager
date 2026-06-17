using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Application.Interface;

public interface IAccountQueryRepository
{
    Task<Account?> GetByEmail(Email email);
    
    Task<IEnumerable<Account>> GetAll();
}