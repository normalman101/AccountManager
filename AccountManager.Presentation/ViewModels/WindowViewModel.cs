using AccountManager.Application.UseCases;
using AccountManager.Core.Entities;
using AccountManager.Presentation.Messages;
using AccountManager.Presentation.ViewModels.Pages.Gateway;
using AccountManager.Presentation.ViewModels.Pages.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels;

public partial class WindowViewModel : ViewModelBase,
    IRecipient<PageChangedMessage>,
    IRecipient<UserLoggedInMessage>
{
    public WindowViewModel(
        AccountAuthenticationUseCase accountAuthenticationUseCase,
        AccountDeletionUseCase accountDeletionUseCase,
        AccountInformationUpdateUseCase accountInformationUpdateUseCase,
        AccountRecoveryUseCase accountRecoveryUseCase,
        AccountRegistrationUseCase accountRegistrationUseCase
    )
    {
        
        _accountAuthenticationUseCase = accountAuthenticationUseCase;
        _accountDeletionUseCase = accountDeletionUseCase;
        _accountInformationUpdateUseCase = accountInformationUpdateUseCase;
        _accountRecoveryUseCase = accountRecoveryUseCase;
        _accountRegistrationUseCase = accountRegistrationUseCase;

        CurrentPage = new MenuViewModel(
            _accountAuthenticationUseCase,
            _accountRegistrationUseCase,
            _accountRecoveryUseCase
        );

        Account = null;
        
        WeakReferenceMessenger.Default.Register<PageChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<UserLoggedInMessage>(this);
    }

    [ObservableProperty]
    public partial ViewModelBase CurrentPage { get; set; }

    private AccountAuthenticationUseCase _accountAuthenticationUseCase;
    private AccountDeletionUseCase _accountDeletionUseCase;
    private AccountInformationUpdateUseCase _accountInformationUpdateUseCase;
    private AccountRecoveryUseCase _accountRecoveryUseCase;
    private AccountRegistrationUseCase _accountRegistrationUseCase;
    [ObservableProperty] public partial Account? Account { get; set; }

    [ObservableProperty] public partial bool IsLogOutButtonVisible { get; set; }

    [ObservableProperty] public partial bool IsAccountsButtonVisible { get; set; }

    [ObservableProperty] public partial bool IsAccountButtonVisible { get; set; }

    [RelayCommand]
    private void LogOut()
    {
        Account = null;
        IsLogOutButtonVisible = false;
        IsAccountsButtonVisible = false;
        IsAccountButtonVisible = false;
        CurrentPage = new MenuViewModel(
            _accountAuthenticationUseCase,
            _accountRegistrationUseCase,
            _accountRecoveryUseCase
        );
    }

    [RelayCommand]
    private void GoToAccounts()
    {
        CurrentPage = new AccountsViewModel();
    }

    [RelayCommand]
    private void GoToAccount()
    {
        CurrentPage = new AccountInformationViewModel();
    }

    public void Receive(PageChangedMessage message)
    {
        CurrentPage = message.Value;
    }

    public void Receive(UserLoggedInMessage message)
    {
        Account = message.Value;
        IsLogOutButtonVisible = true;
        IsAccountsButtonVisible = true;
        IsAccountButtonVisible = true;
    }
}