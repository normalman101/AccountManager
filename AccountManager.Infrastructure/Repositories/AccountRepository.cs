using System;
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
    public async Task Add(User user)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await connection.ExecuteAsync(
                sql: """
                     INSERT INTO table_users(id, email, role) 
                     VALUES (@Id, @Email, @Role);

                     INSERT INTO table_passwords(password, user_id)
                     VALUES (@Password, @UserId)
                     """,
                param: new
                {
                    Id = user.Id,
                    Email = user.Email.Value,
                    Role = user.Role,
                    Password = user.Password.Value,
                    UserId = user.Id
                },
                transaction
            );

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task<User?> GetById(Guid id)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var row = await connection.QueryFirstOrDefaultAsync(
            sql: """
                 SELECT table_users.id, table_users.email, table_users.role, table_passwords.password 
                 FROM table_users
                 JOIN table_passwords ON table_users.id = table_passwords.user_id
                 WHERE table_users.id = @Id AND table_users.is_deleted = FALSE;
                 """,
            param: new { Id = id }
        );

        if (row is null) return null;

        return new User
        {
            Id = row.id,
            Email = new Email { Value = row.email },
            Password = new Password { Value = row.password },
            Role = row.role
        };
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        await using var connection = new NpgsqlConnection(connectionString);

        var rows = await connection.QueryAsync(
            sql: """
                 SELECT table_users.id, table_users.email, table_users.role, table_passwords.password 
                 FROM table_users
                 JOIN table_passwords ON table_users.id = table_passwords.user_id
                 """
        );

        return rows.Select(row =>
            new User
            {
                Id = row.id,
                Email = new Email { Value = row.email },
                Password = new Password { Value = row.password },
                Role = row.role
            }
        ).ToList();
    }

    public async Task Update(User newUser)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await connection.ExecuteAsync(
                sql: """
                     UPDATE table_users
                     SET email = @Email, role = @Role
                     WHERE id = @Id;

                     UPDATE table_passwords
                     SET password = @Password
                     WHERE user_id = @UserId;
                     """,
                param: new
                {
                    Email = newUser.Email.Value,
                    Role = newUser.Role,
                    Id = newUser.Id,
                    Password = newUser.Password.Value,
                    UserId = newUser.Id
                },
                transaction
            );

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
        }
    }

    public async Task Delete(User user)
    {
        await using var connection = new NpgsqlConnection(connectionString);

        await connection.ExecuteAsync(
            sql: """
                 DELETE FROM table_users
                 WHERE id = @Id 
                 """,
            param: new { Id = user.Id }
        );
    }
}