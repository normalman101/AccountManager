using System.Threading.Tasks;
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

public partial class RegistrationViewModel(AccountRegistrationUseCase accountRegistrationUseCase) : ViewModelBase
{
    [ObservableProperty] public partial string Email { get; set; } = string.Empty;
    [ObservableProperty] public partial string Password { get; set; } = string.Empty;
    [ObservableProperty] public partial string RepeatedPassword { get; set; } = string.Empty;

    [RelayCommand]
    private async Task SignUp()
    {
        if (Password != RepeatedPassword) return;

        var account = new Account
        {
            Email = new Email { Value = Email },
            Password = new Password { Value = Password },
            Role = Role.Normal
        };

        var hasExecuted = await accountRegistrationUseCase.Execute(account);

        if (!hasExecuted) return;

        WeakReferenceMessenger.Default.Send(new PageChangedMessage(new AccountInformationViewModel()));

        WeakReferenceMessenger.Default.Send(new UserLoggedInMessage(account));
    }
}