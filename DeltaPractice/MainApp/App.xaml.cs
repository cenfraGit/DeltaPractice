using System.Windows;
using mainApp.Models.Services;
using mainApp.ViewModels.Windows;
using mainApp.Views.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace MainApp;

public partial class App : Application
{
  protected override void OnStartup(StartupEventArgs e)
  {
    var services = new ServiceCollection();
    services.AddSingleton<IDialogService, DialogService>();
    services.AddTransient<MainViewModel>();

    var serviceProvider = services.BuildServiceProvider();

    // set up dialog viewmodel mappings

    IDialogService dialogService = serviceProvider.GetRequiredService<IDialogService>();
    dialogService.RegisterDialog<PracticeViewModel, PracticeWindow>();

    // create mainWindow viewmodel and display view

    var mainWindow = new MainWindow
    {
      DataContext = serviceProvider.GetRequiredService<MainViewModel>()
    };
    mainWindow.Show();
  }
}
