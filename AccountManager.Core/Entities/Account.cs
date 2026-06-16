using System;
using AccountManager.Core.Enums;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Core.Entities;

public class User
{
    public Guid Id { init; get; }
    public required Email Email { init; get; }
    public required Password Password { init; get; }
    public Role Role { init; get; }
}