using AccountManager.Application.UseCases;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Gateway;

public partial class RecoveryViewModel(
    AccountRecoveryUseCase accountRecoveryUseCase,
    AccountAuthenticationUseCase accountAuthenticationUseCase
) : ViewModelBase
{
    [ObservableProperty] public partial string Email { get; set; } = "";
    [ObservableProperty] public partial string Password { get; set; } = "";
    [ObservableProperty] public partial string RepeatedPassword { get; set; } = "";

    [RelayCommand]
    private void Recover()
    {
        WeakReferenceMessenger.Default.Send(
            new PageChangedMessage(new AuthenticationViewModel(accountAuthenticationUseCase, accountRecoveryUseCase))
        );
    }
}