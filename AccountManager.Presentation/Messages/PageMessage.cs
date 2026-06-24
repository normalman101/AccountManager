using AccountManager.Presentation.ViewModels;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AccountManager.Presentation.Messages;

public sealed class PageMessage(ViewModelBase value) : ValueChangedMessage<ViewModelBase>(value);