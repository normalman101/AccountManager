using AccountManager.Presentation.DTOs;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AccountManager.Presentation.Messages;

public sealed class AccountLoggedOutMessage(AccountLoggedOut value) : ValueChangedMessage<AccountLoggedOut>(value);