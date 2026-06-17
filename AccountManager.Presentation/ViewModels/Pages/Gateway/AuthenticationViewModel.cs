using AccountManager.Application.UseCases;
using AccountManager.Core.Entities;
using AccountManager.Core.Enums;
using AccountManager.Core.ValueObjects;
using AccountManager.Presentation.Messages;
using AccountManager.Presentation.ViewModels.Pages.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Gateway;

public partial class AuthenticationViewModel(
    AccountAuthenticationUseCase accountAuthenticationUseCase,
    AccountRecoveryUseCase accountRecoveryUseCase)
    : ViewModelBase
{
    [ObservableProperty] public partial string Email { get; set; } = string.Empty;
    [ObservableProperty] public partial string Password { get; set; } = string.Empty;

    [RelayCommand]
    private void Recover()
    {
        WeakReferenceMessenger.Default.Send(
            new PageChangedMessage(new RecoveryViewModel(accountRecoveryUseCase, accountAuthenticationUseCase))
        );
    }

    [RelayCommand]
    private void Authenticate()
    {
        var account = accountAuthenticationUseCase.Execute(new Account
        {
            Email = new Email { Value = Email },
            Password = new Password { Value = Password },
            Role = Role.Normal
        }).Result;

        if (account is null) return;

        if (account.Password.Value != Password) return;

        WeakReferenceMessenger.Default.Send(new UserLoggedInMessage(account));
        WeakReferenceMessenger.Default.Send(new PageChangedMessage(new AccountInformationViewModel()));
    }
}