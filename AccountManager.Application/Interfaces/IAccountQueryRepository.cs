using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Application.Interfaces;

public interface IAccountQueryRepository
{
    Task<Result<Account>> GetByEmail(Email email);
    
    Task<IEnumerable<Account>> GetAll();
}