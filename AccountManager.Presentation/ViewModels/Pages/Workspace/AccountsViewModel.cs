using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AccountManager.Application.DTOs;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.UseCases;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Workspace;

public sealed partial class AccountsViewModel(
    AccountInformationUpdateUseCase accountInformationUpdateUseCase,
    AccountDeletionUseCase accountDeletionUseCase,
    IEnumerable<AccountDto> accounts
) : ViewModelBase
{
    public ObservableCollection<AccountDto> Accounts { set; get; } = new(accounts);

    [ObservableProperty] public partial AccountDto? Account { get; set; } = null;

    [RelayCommand]
    private void Edit()
    {
        if (Account is not null)
        {
            WeakReferenceMessenger.Default.Send(new PageMessage(
                new AccountInformationEditorViewModel(
                    this,
                    accountInformationUpdateUseCase,
                    Account
                )
            ));
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        if (Account is not null)
        {
            await accountDeletionUseCase.Execute(new DeleteRequest { Email = Account.Email });

            Accounts.Remove(Account);
        }
    }
}