using AccountManager.Presentation.ViewModels;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AccountManager.Presentation.Messages;

public class PageChangedMessage(ViewModelBase value) : ValueChangedMessage<ViewModelBase>(value);