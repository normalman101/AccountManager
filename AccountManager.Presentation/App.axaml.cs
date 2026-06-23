using AccountManager.Application.UseCases;
using AccountManager.Infrastructure.Repositories;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WindowView = AccountManager.Presentation.Views.WindowView;
using WindowViewModel = AccountManager.Presentation.ViewModels.WindowViewModel;

namespace AccountManager.Presentation;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        const string connectionString = "Host=localhost;Port=5430;Database=postgres;Username=postgres;Password=1234";

        var accountCommandRepository = new AccountCommandRepository(connectionString);
        var accountQueryRepository = new AccountQueryRepository(connectionString);

        var accountAuthenticationUseCase =
            new AccountAuthenticationUseCase(accountQueryRepository);
        var accountDeletionUseCase =
            new AccountDeletionUseCase(accountCommandRepository);
        var accountInformationUpdateUseCase =
            new AccountInformationUpdateUseCase(accountQueryRepository, accountCommandRepository);
        var accountRecoveryUseCase =
            new AccountRecoveryUseCase(accountQueryRepository, accountCommandRepository);
        var accountRegistrationUseCase =
            new AccountRegistrationUseCase(accountCommandRepository, accountQueryRepository);
        var getAllAccountsUseCase =
            new GetAllAccountsUseCase(accountQueryRepository);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new WindowView
            {
                DataContext = new WindowViewModel(
                    accountAuthenticationUseCase,
                    accountDeletionUseCase,
                    accountInformationUpdateUseCase,
                    accountRecoveryUseCase,
                    accountRegistrationUseCase,
                    getAllAccountsUseCase
                ),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}