using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using core.classes;
using mainApp.Models;
using mainApp.Models.Services;
using mainApp.ViewModels.Windows;

namespace mainApp.ViewModels.Dialogs;

public partial class CreatePracticeViewModel : ObservableObject
{
  IDialogService _dialogService;

  [ObservableProperty] string _windowTitle;

  [ObservableProperty] string _practiceTitle;

  [ObservableProperty] ObservableCollection<ProblemViewModel> _problemsCollection = new();

  public CreatePracticeViewModel(IDialogService dialogService, CreateMode createMode, Practice? practiceData = null, string? practiceName = null)
  {
    _dialogService = dialogService;

    switch (createMode)
    {
      case CreateMode.Create:
        {
          WindowTitle = "Create practice file...";
          break;
        }
      case CreateMode.Edit:
        {
          WindowTitle = "Edit practice file...";
          if (practiceName is not null)
            PracticeTitle = practiceName;
          if (practiceData is null)
            break;
          foreach ((string probName, Problem probData) in practiceData.Problems)
          {
            ProblemsCollection.Add(new ProblemViewModel(_dialogService, probName, probData));
          }
          break;
        }
      default:
        throw new Exception("Invalid create mode.");
    }
  }
}