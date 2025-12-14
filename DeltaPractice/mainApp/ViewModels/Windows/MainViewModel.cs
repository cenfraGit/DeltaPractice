using CommunityToolkit.Mvvm.Input;
using core.classes;
using core.utils.files;
using mainApp.Models.Services;
using Microsoft.Win32;

namespace mainApp.ViewModels.Windows;

public partial class MainViewModel(IDialogService dialogService)
{
  private readonly IDialogService _dialogService = dialogService;

  [RelayCommand]
  private void OnOpenDialog()
  {
    var openDialog = new OpenFileDialog
    {
      Filter = "Practice files (*.prac)|*.prac",
    };

    if (openDialog.ShowDialog() == true)
    {
      Practice practiceData = FileUtils.ReadPracFile(openDialog.FileName);
      OpenPracticeWindow(practiceData);
    }
  }

  private void OpenPracticeWindow(Practice practice)
  {
    var practiceViewModel = new PracticeViewModel(practice);
    _dialogService.ShowDialog(practiceViewModel);
  }
}