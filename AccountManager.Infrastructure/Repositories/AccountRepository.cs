using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.Core.Entities;
using AccountManager.Core.ValueObjects;
using Dapper;
using Npgsql;

namespace AccountManager.Infrastructure.Repositories;

[SuppressMessage("ReSharper", "RedundantAnonymousTypePropertyName")]
public class AccountRepository(string connectionString)
{
    public async Task<bool> Add(Account account)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var affectedRows = 0;
        
        try
        {
            affectedRows = await connection.ExecuteAsync(
                sql: """
                     INSERT INTO table_accounts(email, role) 
                     VALUES (@Email, @Role);

                     INSERT INTO table_passwords(password, account_email)
                     VALUES (@Password, @AccountEmail)
                     """,
                param: new
                {
                    Email = account.Email.Value,
                    Role = account.Role,
                    Password = account.Password.Value,
                    AccountEmail = account.Email.Value
                },
                transaction
            );

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }

        return affectedRows > 0;
    }

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

    public async Task<bool> Update(Account newAccount)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var affectedRows = 0;
        
        try
        {
            affectedRows = await connection.ExecuteAsync(
                sql: """
                     UPDATE table_accounts
                     SET email = @Email, role = @Role
                     WHERE email = @Email;

                     UPDATE table_passwords
                     SET password = @Password
                     WHERE account_email = @AccountEmail;
                     """,
                param: new
                {
                    Email = newAccount.Email.Value,
                    Role = newAccount.Role,
                    Password = newAccount.Password.Value,
                    AccountEmail = newAccount.Email.Value,
                },
                transaction
            );

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }

        return affectedRows > 0;
    }

    public async Task<bool> Delete(Account account)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var affectedRows = await connection.ExecuteAsync(
            sql: """
                 DELETE FROM table_accounts
                 WHERE email = @Email 
                 """,
            param: new { Email = account.Email.Value }
        );

        return affectedRows > 0;
    }
}