using AccountManager.Application.DTOs;
using AccountManager.Application.UseCases;
using AccountManager.Presentation.Messages;
using AccountManager.Presentation.ViewModels.Pages.Gateway;
using AccountManager.Presentation.ViewModels.Pages.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels;

public partial class WindowViewModel :
    ViewModelBase,
    IRecipient<AccountLoggedInMessage>,
    IRecipient<AccountLoggedOutMessage>
{
    public WindowViewModel(
        AccountAuthenticationUseCase accountAuthenticationUseCase,
        AccountDeletionUseCase accountDeletionUseCase,
        AccountInformationUpdateUseCase accountInformationUpdateUseCase,
        AccountRecoveryUseCase accountRecoveryUseCase,
        AccountRegistrationUseCase accountRegistrationUseCase,
        GetAllAccountsUseCase getAllAccountsUseCase
    )
    {
        _accountAuthenticationUseCase = accountAuthenticationUseCase;
        _accountDeletionUseCase = accountDeletionUseCase;
        _accountInformationUpdateUseCase = accountInformationUpdateUseCase;
        _accountRecoveryUseCase = accountRecoveryUseCase;
        _accountRegistrationUseCase = accountRegistrationUseCase;
        _getAllAccountsUseCase = getAllAccountsUseCase;
        
        CurrentPage = new GatewayViewModel(
            _accountAuthenticationUseCase,
            _accountRegistrationUseCase,
            _accountRecoveryUseCase
        );
        
        Account = null;

        WeakReferenceMessenger.Default.Register<AccountLoggedInMessage>(this);
        WeakReferenceMessenger.Default.Register<AccountLoggedOutMessage>(this);
    }

    private readonly AccountAuthenticationUseCase _accountAuthenticationUseCase;
    private readonly AccountDeletionUseCase _accountDeletionUseCase;
    private readonly AccountInformationUpdateUseCase _accountInformationUpdateUseCase;
    private readonly AccountRecoveryUseCase _accountRecoveryUseCase;
    private readonly AccountRegistrationUseCase _accountRegistrationUseCase;
    private readonly GetAllAccountsUseCase _getAllAccountsUseCase;
    
    [ObservableProperty] public partial ViewModelBase CurrentPage { get; set; }
    
    private AccountDto? Account { get; set; }

    public void Receive(AccountLoggedInMessage message)
    {
        Account = new AccountDto
        {
            Email = message.Value.Email,
            Password = message.Value.Password,
            Role = message.Value.Role
        };
        
        CurrentPage = new WorkspaceViewModel(
            Account, 
            _getAllAccountsUseCase,
            _accountInformationUpdateUseCase,
            _accountDeletionUseCase
        );
    }

    public void Receive(AccountLoggedOutMessage message)
    {
        Account = null;
        
        CurrentPage = new GatewayViewModel(
            _accountAuthenticationUseCase,
            _accountRegistrationUseCase,
            _accountRecoveryUseCase
        );
    }
}