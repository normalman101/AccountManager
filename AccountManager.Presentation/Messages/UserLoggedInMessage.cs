using AccountManager.Core.Entities;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AccountManager.Presentation.Messages;

public class UserLoggedInMessage(Account value) : ValueChangedMessage<Account>(value);