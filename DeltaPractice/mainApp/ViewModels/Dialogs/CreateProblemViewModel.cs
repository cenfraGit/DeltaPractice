using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using core.classes;
using core.classes.context;
using core.classes.questions;

namespace mainApp.ViewModels.Windows;

public class CreateProblemViewModel : ObservableObject
{
  private Problem _problemData;
  public ObservableCollection<ObservableObject> ContextViewModels { get; set; } = [];
  public ObservableCollection<ObservableObject> QuestionViewModels { get; set; } = [];

  public CreateProblemViewModel(Problem problemData)
  {
    _problemData = problemData;

    ContextViewModels.Clear();
    QuestionViewModels.Clear();

    // add context viewmodels
    foreach (var ctxItem in problemData.Context.Elements)
    {
      if (ctxItem is ContextText ctxItemText)
        ContextViewModels.Add(new ContextTextViewModel(ctxItemText));
      if (ctxItem is ContextImage ctxItemImage)
        ContextViewModels.Add(new ContextImageViewModel(ctxItemImage));
    }

    // add question viewmodels
    foreach ((_, AQuestion qValue) in problemData.Questions)
    {
      if (qValue is QuestionTextBox qValueTextBox)
      {
        QuestionViewModels.Add(new QuestionTextBoxViewModel(qValueTextBox));
      }
    }
  }
}