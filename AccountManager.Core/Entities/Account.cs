using AccountManager.Core.Enums;
using AccountManager.Core.ValueObjects;

namespace AccountManager.Core.Entities;

public class Account
{
    public required Email Email { init; get; }
    public required Password Password { init; get; }
    public required Role Role { init; get; }
}