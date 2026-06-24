using AccountManager.Application.DTOs;
using AccountManager.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AccountManager.Presentation.ViewModels.Pages.Workspace;

public sealed partial class AccountInformationViewModel(AccountDto accountDto) : ViewModelBase
{
    [ObservableProperty] public partial string Email { get; set; } = accountDto.Email;

    [ObservableProperty] public partial string Password { get; set; } = accountDto.Password;
}