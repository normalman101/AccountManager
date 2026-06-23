using AccountManager.Application.UseCases;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Gateway;

public partial class MenuViewModel(
    AccountAuthenticationUseCase accountAuthenticationUseCase,
    AccountRegistrationUseCase accountRegistrationUseCase,
    AccountRecoveryUseCase accountRecoveryUseCase
) : ViewModelBase
{
    [RelayCommand]
    private void GoToAuthentication()
    {
        WeakReferenceMessenger.Default.Send(new PageMessage(
            new AuthenticationViewModel(
                accountAuthenticationUseCase,
                accountRecoveryUseCase
            )
        ));
    }

    [RelayCommand]
    private void GoToRegistration()
    {
        WeakReferenceMessenger.Default.Send(new PageMessage(
            new RegistrationViewModel(accountRegistrationUseCase)
        ));
    }

    [RelayCommand]
    private void GoToRecovery()
    {
        WeakReferenceMessenger.Default.Send(new PageMessage(
            new RecoveryViewModel(
                accountRecoveryUseCase,
                accountAuthenticationUseCase
            )
        ));
    }
}