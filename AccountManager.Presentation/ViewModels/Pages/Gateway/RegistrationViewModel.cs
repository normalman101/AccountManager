using System.Threading.Tasks;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.UseCases;
using AccountManager.Core.Enums;
using AccountManager.Presentation.DTOs;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Gateway;

public partial class RegistrationViewModel(AccountRegistrationUseCase accountRegistrationUseCase) : ViewModelBase
{
    [ObservableProperty] public partial string Email { get; set; } = string.Empty;
    [ObservableProperty] public partial string Password { get; set; } = string.Empty;
    [ObservableProperty] public partial string RepeatedPassword { get; set; } = string.Empty;

    [RelayCommand]
    private async Task Register()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password)) return;

        if (Password != RepeatedPassword) return;

        var response = await accountRegistrationUseCase.Execute(new RegisterRequest
            {
                Email = Email,
                Password = Password
            }
        );

        if (response.IsFailure) return;

        var accountDto = new AccountLoggedIn(
            Email,
            Password,
            Role.Normal
        );

        WeakReferenceMessenger.Default.Send(new AccountLoggedInMessage(accountDto));
    }
}