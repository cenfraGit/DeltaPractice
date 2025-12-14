using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using core.classes;
using core.classes.context;
using core.classes.questions;

namespace mainApp.ViewModels.Windows;

public enum AnswerStatus
{
  Unknown,
  Correct,
  Incorrect
}

public partial class PracticeViewModel : ObservableObject
{
  private readonly DispatcherTimer _flashTimer;
  private bool IsChecked = false;

  public Practice Practice { get; set; }
  public PracticeMode PracticeMode { get => Practice.PracticeMode; }
  public int PracticeAmount { get => Practice.PracticeAmount; }
  public int Correct 
  { 
    get => Practice.Correct;
    set 
    {
      Practice.Correct = value;
      SBCorrect = value.ToString();
    }
  }
  public int Incorrect
  { 
    get => Practice.Incorrect;
    set
    {
      Practice.Incorrect = value;
      SBIncorrect = value.ToString();
    }
  }
  public int CurrentAmount { get => Practice.CurrentAmount; }

  public ObservableCollection<ObservableObject> ContextViewModels { get; set; } = [];
  public ObservableCollection<ObservableObject> QuestionViewModels { get; set; } = [];

  [ObservableProperty] Visibility _buttonNextProblemVisibility = Visibility.Hidden;
  [ObservableProperty] AnswerStatus _problemStatus = AnswerStatus.Unknown;
  [ObservableProperty] string _sBCorrect = "0";
  [ObservableProperty] string _sBIncorrect = "0";

  public PracticeViewModel(Practice practice)
  {
    this.Practice = practice;
    SetProblem(this.Practice.GetProblem());

    // set up timer. revert to unknown state after interval
    _flashTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.8) };
    _flashTimer.Tick += (sender, e) =>
    {
      ProblemStatus = AnswerStatus.Unknown;
      _flashTimer.Stop();
    };
  }

  /// <summary>
  /// Sets this instance's context and question's viewmodels based on problem data.
  /// </summary>
  /// <param name="problem"></param>
  public void SetProblem(Problem problem)
  {
    ContextViewModels.Clear();
    QuestionViewModels.Clear();

    // add context viewmodels
    foreach (var ctxItem in problem.Context.Elements)
    {
      if (ctxItem is ContextText ctxItemText)
        ContextViewModels.Add(new ContextTextViewModel(ctxItemText));
      if (ctxItem is ContextImage ctxItemImage)
        ContextViewModels.Add(new ContextImageViewModel(ctxItemImage));
    }

    // add question viewmodels
    foreach ((_, AQuestion qValue) in problem.Questions)
    {
      if (qValue is QuestionTextBox qValueTextBox)
      {
        QuestionViewModels.Add(new QuestionTextBoxViewModel(qValueTextBox));
      }
    }
  }

  [RelayCommand]
  public void OnCheckCurrentProblem()
  {
    bool allCorrect = true;
    foreach (var questionVM in this.QuestionViewModels)
    {
      if (questionVM is QuestionTextBoxViewModel questionTextBox)
      {
        if (!questionTextBox.Check())
          allCorrect = false;
      }
    }

    if (allCorrect)
    {
      if (!IsChecked)
      {
        Correct++;
      }
      ProblemStatus = AnswerStatus.Correct;
      _flashTimer.Start();
      LoadNewProblem();
    }
    else
    {
      if (!IsChecked)
      {
        Incorrect++;
        IsChecked = true;
      }
      ButtonNextProblemVisibility = Visibility.Visible;
      ProblemStatus = AnswerStatus.Incorrect;
      _flashTimer.Start();
    }
  }

  [RelayCommand]
  public void OnLoadNextProblem()
  {
    ButtonNextProblemVisibility = Visibility.Hidden;
    LoadNewProblem();
  }

  private void LoadNewProblem()
  {
    IsChecked = false;
    SetProblem(this.Practice.GetProblem());
  }

}