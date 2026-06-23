using AccountManager.Presentation.DTOs;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AccountManager.Presentation.Messages;

public class AccountLoggedInMessage(AccountLoggedIn value) : ValueChangedMessage<AccountLoggedIn>(value);