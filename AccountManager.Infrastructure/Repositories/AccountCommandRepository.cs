using System.Threading.Tasks;
using AccountManager.Application.Interface;
using AccountManager.Core.Entities;
using Dapper;
using Npgsql;

namespace AccountManager.Infrastructure.Repositories;

public class AccountCommandRepository(string connectionString) : IAccountCommandRepository
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