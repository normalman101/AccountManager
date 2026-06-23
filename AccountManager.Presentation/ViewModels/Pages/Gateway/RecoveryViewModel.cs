using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
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
    [ObservableProperty] public partial string Email { get; set; } = string.Empty;
    [ObservableProperty] public partial string Password { get; set; } = string.Empty;
    [ObservableProperty] public partial string RepeatedPassword { get; set; } = string.Empty;

    [RelayCommand]
    private async Task Recover()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password)) return;

        if (Password != RepeatedPassword) return;

        var response = await accountRecoveryUseCase.Execute(new RecoveryRequest
            {
                Email = Email,
                Password = Password,
            }
        );

        if (response.IsFailure) return;

        WeakReferenceMessenger.Default.Send(new PageMessage(
            new AuthenticationViewModel(
                accountAuthenticationUseCase,
                accountRecoveryUseCase
            )
        ));
    }
}