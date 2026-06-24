using AccountManager.Application.UseCases;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Gateway;

public sealed partial class GatewayViewModel :
    ViewModelBase,
    IRecipient<PageMessage>
{
    public GatewayViewModel(
        AccountAuthenticationUseCase accountAuthenticationUseCase,
        AccountRegistrationUseCase accountRegistrationUseCase,
        AccountRecoveryUseCase accountRecoveryUseCase
    )
    {
        _accountAuthenticationUseCase = accountAuthenticationUseCase;
        _accountRegistrationUseCase = accountRegistrationUseCase;
        _accountRecoveryUseCase = accountRecoveryUseCase;

        CurrentPage = new MenuViewModel(
            _accountAuthenticationUseCase,
            _accountRegistrationUseCase,
            _accountRecoveryUseCase
        );

        WeakReferenceMessenger.Default.Register(this);
    }

    private readonly AccountAuthenticationUseCase _accountAuthenticationUseCase;
    private readonly AccountRegistrationUseCase _accountRegistrationUseCase;
    private readonly AccountRecoveryUseCase _accountRecoveryUseCase;

    [ObservableProperty] public partial ViewModelBase CurrentPage { get; set; }
    [ObservableProperty] public partial bool IsUndoButtonVisible { get; set; } = false;

    [RelayCommand]
    private void Undo()
    {
        CurrentPage = new MenuViewModel(
            _accountAuthenticationUseCase,
            _accountRegistrationUseCase,
            _accountRecoveryUseCase
        );

        IsUndoButtonVisible = false;
    }

    public void Receive(PageMessage message)
    {
        CurrentPage = message.Value;

        IsUndoButtonVisible = true;
    }
}