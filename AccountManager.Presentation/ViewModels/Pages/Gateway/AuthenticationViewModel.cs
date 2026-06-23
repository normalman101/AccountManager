using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.UseCases;
using AccountManager.Presentation.DTOs;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Gateway;

public partial class AuthenticationViewModel(
    AccountAuthenticationUseCase accountAuthenticationUseCase,
    AccountRecoveryUseCase accountRecoveryUseCase
) : ViewModelBase
{
    [ObservableProperty] public partial string Email { get; set; } = string.Empty;

    [ObservableProperty] public partial string Password { get; set; } = string.Empty;

    [RelayCommand]
    private async Task Authenticate()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            return;
        }

        var result = await accountAuthenticationUseCase.Execute(new AuthenticateRequest(
            Email,
            Password
        ));

        if (result.IsFailure) return;

        WeakReferenceMessenger.Default.Send(new AccountLoggedInMessage(
            new AccountLoggedIn(
                result.Value!.Email,
                result.Value.Password,
                result.Value.Role
            )
        ));
    }

    [RelayCommand]
    private void Recover()
    {
        WeakReferenceMessenger.Default.Send(new PageMessage(
            new RecoveryViewModel(
                accountRecoveryUseCase,
                accountAuthenticationUseCase
            )
        ));
    }
}