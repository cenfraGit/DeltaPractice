using System.Configuration;
using System.Data;
using System.Windows;
using mainApp.Services.Dialogs;
using mainApp.ViewModels.Windows;
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

    var mainWindow = new MainWindow
    {
      DataContext = serviceProvider.GetRequiredService<MainViewModel>()
    };
    mainWindow.Show();
  }
}
