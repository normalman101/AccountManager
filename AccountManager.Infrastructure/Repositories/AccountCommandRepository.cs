using System.Threading.Tasks;
using AccountManager.Application.Interfaces;
using AccountManager.Core.Entities;
using AccountManager.Core.Enums;
using AccountManager.Core.Errors;
using AccountManager.Core.Results;
using AccountManager.Core.ValueObjects;
using Dapper;
using Npgsql;

namespace AccountManager.Infrastructure.Repositories;

public sealed class AccountCommandRepository(string connectionString) : IAccountCommandRepository
{
    public async Task<ResultVoid> Add(Account account)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        bool isAdded;

        try
        {
            isAdded = await connection.ExecuteScalarAsync<bool>(
                sql: "SELECT function_add_account(@Email, @Password, @Role);",
                param: new
                {
                    Email = account.Email.Value,
                    Password = account.Password.Value,
                    Role = Role.Normal
                },
                transaction
            );

            await transaction.CommitAsync();
        }
        catch (NpgsqlException)
        {
            await transaction.RollbackAsync();
            throw;
        }

        if (!isAdded)
        {
            return ResultVoid.Failure(new Error(
                ErrorCode.AccountHasNotBeenAdded,
                "Аккаунт не добавился"
            ));
        }

        return ResultVoid.Success();
    }

    public async Task Update(Email oldAccountEmail, Account newAccount)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await connection.ExecuteAsync(
                sql: "CALL procedure_update_account(@OldEmail, @NewEmail, @NewPassword, @NewRole);",
                param: new
                {
                    OldEmail = oldAccountEmail.Value,
                    NewEmail = newAccount.Email.Value,
                    NewPassword = newAccount.Password.Value,
                    NewRole = newAccount.Role
                },
                transaction
            );

            await transaction.CommitAsync();
        }
        catch (NpgsqlException)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ResultVoid> Recover(Email email, Password newPassword)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var isRecovered = await connection.ExecuteScalarAsync<bool>(
            sql: "SELECT function_recover_account(@Email, @NewPassword);",
            param: new
            {
                Email = email.Value,
                NewPassword = newPassword.Value
            }
        );

        if (!isRecovered)
        {
            return ResultVoid.Failure(new Error(
                ErrorCode.AccountHasNotBeenRecovered,
                "Аккаунт не восстановился"
            ));
        }

        return ResultVoid.Success();
    }

    public async Task<ResultVoid> Delete(Email email)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var affectedRows = await connection.ExecuteAsync(
            sql: """
                 DELETE FROM table_accounts
                 WHERE email = @Email 
                 """,
            param: new { Email = email.Value }
        );

        return affectedRows > 0
            ? ResultVoid.Success()
            : ResultVoid.Failure(new Error(
                ErrorCode.AccountHasNotBeenDeleted,
                "Аккаунт не удалился"
            ));
    }
}