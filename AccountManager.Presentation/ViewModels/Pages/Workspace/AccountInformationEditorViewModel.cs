using System.Threading.Tasks;
using AccountManager.Application.DTOs;
using AccountManager.Application.DTOs.Requests;
using AccountManager.Application.UseCases;
using AccountManager.Core.Enums;
using AccountManager.Presentation.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AccountManager.Presentation.ViewModels.Pages.Workspace;

public partial class AccountInformationEditorViewModel(
    AccountsViewModel accountsViewModel,
    AccountInformationUpdateUseCase accountInformationUpdateUseCase,
    AccountDto accountDto
) : ViewModelBase
{
    [ObservableProperty] public partial string Email { get; set; } = accountDto.Email;
    [ObservableProperty] public partial string Password { get; set; } = accountDto.Password;
    [ObservableProperty] public partial Role Role { get; set; } = accountDto.Role;

    [RelayCommand]
    private async Task Save()
    {
        var result = await accountInformationUpdateUseCase.Execute(new UpdateRequest(
            accountDto.Email,
            Email,
            Password,
            Role
        ));

        if (result.IsSuccess) WeakReferenceMessenger.Default.Send(new PageMessage(accountsViewModel));
    }

    [RelayCommand]
    private void Cancel()
    {
        WeakReferenceMessenger.Default.Send(new PageMessage(accountsViewModel));
    }
}