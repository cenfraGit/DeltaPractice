using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using core.classes;
using mainApp.Models.Services;

namespace mainApp.ViewModels.Windows;

public partial class ProblemViewModel : ObservableObject
{

  IDialogService _dialogService;
  private Problem _problemData;

  [ObservableProperty] string _name;
  //[ObservableProperty] ObservableCollection<ObservableObject> _variableItems = [];
  //[ObservableProperty] ObservableCollection<ObservableObject> _contextItems = [];
  //[ObservableProperty] ObservableCollection<ObservableObject> _questionItems = [];

  public ProblemViewModel(IDialogService dialogService, string name, Problem problemData)
  {
    _dialogService = dialogService;
    Name = name;
    _problemData = problemData;
  }

  [RelayCommand]
  private void OnCreatorEdit()
  {
    var viewModel = new CreateProblemViewModel(_problemData);
    _dialogService.ShowDialog(viewModel);
  }

  [RelayCommand]
  private void OnCreatorRemove()
  {

  }
}