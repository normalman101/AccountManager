using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Application.Interfaces;
using AccountManager.Core.Entities;
using AccountManager.Core.Errors;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;
using AccountManager.Infrastructure.DTOs;
using Dapper;
using Npgsql;

namespace AccountManager.Infrastructure.Repositories;

public sealed class AccountQueryRepository(string connectionString) : IAccountQueryRepository
{
    public async Task<Result<Account>> GetByEmail(Email email)
    {
        try
        {
            await using var connection = new NpgsqlConnection(connectionString);

            var account = await connection.QueryFirstOrDefaultAsync<AccountDto>(
                sql: """
                     SELECT view_accounts.email, view_accounts.role, view_accounts.password 
                     FROM view_accounts
                     WHERE email = @Email
                     """,
                param: new { Email = email.Value }
            );

            return account is null
                ? Result<Account>.Failure(new Error(
                    ErrorCode.AccountHasNotBeenFound,
                    "Аккаунт не найден"
                ))
                : Result<Account>.Success(Account.Create(
                        account.Email,
                        account.Password,
                        account.Role
                    )
                    .Value!);
        }
        catch (NpgsqlException)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Account>> GetAll()
    {
        try
        {
            await using var connection = new NpgsqlConnection(connectionString);

            var accounts = await connection.QueryAsync<AccountDto>(
                sql: """
                     SELECT email, role, password 
                     FROM view_accounts
                     """
            );

            return accounts.Select(a => Account.Create(
                    a.Email,
                    a.Password,
                    a.Role
                )
                .Value!);
        }
        catch (NpgsqlException)
        {
            throw;
        }
    }
}