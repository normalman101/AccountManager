using System.Threading.Tasks;
using AccountManager.Application.DTOs;
using AccountManager.Application.UseCases;
using AccountManager.Core.Enums;
using AccountManager.Presentation.DTOs;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Workspace;

public sealed partial class WorkspaceViewModel :
    ViewModelBase,
    IRecipient<PageMessage>
{
    public WorkspaceViewModel(
        AccountDto accountDto,
        GetAllAccountsUseCase getAllAccountsUseCase,
        AccountInformationUpdateUseCase accountInformationUpdateUseCase,
        AccountDeletionUseCase accountDeletionUseCase
    )
    {
        _getAllAccountsUseCase = getAllAccountsUseCase;
        _accountInformationUpdateUseCase = accountInformationUpdateUseCase;
        _accountDeletionUseCase = accountDeletionUseCase;

        Account = accountDto;

        CurrentPage = new AccountInformationViewModel(Account);

        IsAccountsButtonVisible = Account.Role is Role.Admin;

        WeakReferenceMessenger.Default.Register(this);
    }

    private readonly GetAllAccountsUseCase _getAllAccountsUseCase;
    private readonly AccountInformationUpdateUseCase _accountInformationUpdateUseCase;
    private readonly AccountDeletionUseCase _accountDeletionUseCase;

    private AccountDto Account { get; set; }

    [ObservableProperty] public partial ViewModelBase CurrentPage { get; set; }

    [ObservableProperty] public partial bool IsAccountsButtonVisible { get; set; }

    [RelayCommand]
    private void LogOut()
    {
        WeakReferenceMessenger.Default.Send(new AccountLoggedOutMessage(new AccountLoggedOut()));
    }

    [RelayCommand]
    private async Task GoToAccounts()
    {
        var accounts = await _getAllAccountsUseCase.Execute();

        CurrentPage = new AccountsViewModel(
            _accountInformationUpdateUseCase,
            _accountDeletionUseCase,
            accounts
        );
    }

    [RelayCommand]
    private void GoToAccount()
    {
        Receive(new PageMessage(
            new AccountInformationViewModel(Account!)
        ));
    }

    public void Receive(PageMessage message)
    {
        CurrentPage = message.Value;
    }
}