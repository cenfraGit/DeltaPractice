using CommunityToolkit.Mvvm.Input;
using core.classes;
using core.utils.files;
using mainApp.Services.Dialogs;
using mainApp.ViewModels.Dialogs;
using mainApp.Views.Windows;
using Microsoft.Win32;

namespace mainApp.ViewModels.Windows;

public partial class MainViewModel
{
  private readonly IDialogService _dialogService;

  public MainViewModel(IDialogService dialogService)
  {
    _dialogService = dialogService;
  }

  [RelayCommand]
  private void OnOpenDialog()
  {
    //var newItemViewModel = new NewItemDialogViewModel();
    //_dialogService.ShowDialog(newItemViewModel);
    var openDialog = new OpenFileDialog
    {
      Filter = "Prob files (*.prob)|*.xml",
    };

    bool? result = openDialog.ShowDialog();

    if (result == null || result == false) return;




  }

  private void OpenProblemWindow(Problem problem)
  {
    var problemWindow = new ProblemWindow();
    problemWindow.Show();
  }
}
