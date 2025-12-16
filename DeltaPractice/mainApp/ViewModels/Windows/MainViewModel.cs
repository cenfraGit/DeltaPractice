using System.IO;
using CommunityToolkit.Mvvm.Input;
using core.classes;
using core.utils.files;
using mainApp.Models;
using mainApp.Models.Services;
using mainApp.ViewModels.Dialogs;
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
      Title = "Open practice file..."
    };

    if (openDialog.ShowDialog() == true)
    {
      Practice practiceData = FileUtils.ReadPracFile(openDialog.FileName);
      var practiceViewModel = new PracticeViewModel(practiceData);
      _dialogService.ShowDialog(practiceViewModel);
    }
  }

  [RelayCommand]
  private void OnOpenCreatePractice()
  {
    var createPracticeViewModel = new CreatePracticeViewModel(_dialogService, CreateMode.Create);
    _dialogService.ShowDialog(createPracticeViewModel);
  }

  [RelayCommand]
  private void OnOpenEditPractice()
  {
    var openDialog = new OpenFileDialog
    {
      Filter = "Practice files (*.prac)|*.prac",
      Title = "Edit practice file..."
    };

    if (openDialog.ShowDialog() == true)
    {
      // update viewmodel with existing data
      Practice practiceData = FileUtils.ReadPracFile(openDialog.FileName);
      CreatePracticeViewModel viewModel = new(_dialogService, CreateMode.Edit, practiceData, Path.GetFileNameWithoutExtension(openDialog.FileName));
      _dialogService.ShowDialog(viewModel);
    }
  }

}