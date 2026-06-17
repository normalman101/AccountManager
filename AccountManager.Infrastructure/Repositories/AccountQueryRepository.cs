using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;
using AccountManager.Core.ValueObjects;
using Dapper;
using Npgsql;

namespace AccountManager.Infrastructure.Repositories;

public class AccountQueryRepository(string connectionString) : IAccountQueryRepository
{
    public async Task<Account?> GetByEmail(Email email)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var row = await connection.QueryFirstOrDefaultAsync(
            sql: """
                 SELECT table_accounts.email, table_accounts.role, table_passwords.password 
                 FROM table_accounts
                 JOIN table_passwords ON table_accounts.email = table_passwords.account_email
                 WHERE table_accounts.email = @Email AND table_accounts.is_deleted = FALSE;
                 """,
            param: new { Email = email.Value }
        );

        if (row is null) return null;

        return new Account
        {
            Email = new Email { Value = row.email },
            Password = new Password { Value = row.password },
            Role = row.role
        };
    }

    public async Task<IEnumerable<Account>> GetAll()
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var rows = await connection.QueryAsync(
            sql: """
                 SELECT table_accounts.email, table_accounts.role, table_passwords.password 
                 FROM table_accounts
                 JOIN table_passwords ON table_accounts.email = table_passwords.account_email
                 """
        );

        return rows.Select(row =>
            new Account
            {
                Email = new Email { Value = row.email },
                Password = new Password { Value = row.password },
                Role = row.role
            }
        ).ToList();
    }
}