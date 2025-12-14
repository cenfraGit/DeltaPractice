using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using core.classes.questions;

namespace mainApp.ViewModels.Windows;

public partial class QuestionTextBoxViewModel : ObservableObject
{
  QuestionTextBox QuestionTextBox { get; set; }


  // contains viewmodels for simple text and textboxes.
  public ObservableCollection<ObservableObject> QuestionItems { get; set; } = [];

  public QuestionTextBoxViewModel(QuestionTextBox questionTextBox)
  {
    QuestionTextBox = questionTextBox;
    this.UpdateViewModels();
  }

  /// <summary>
  /// Separates question text into parts by textbox locations, and 
  /// adds the viewmodels to the QuestionItems ObservableCollection. 
  /// </summary>
  private void UpdateViewModels()
  {
    // the question text should have been updated with variable values at this point.

    QuestionItems.Clear();

    // the main question string will be separated by _ANSWERKEY_, using available answers.

    List<string> separatingStrings = [];
    List<string> textBoxOrder = [];

    // build the separatingStrings list and add the order in which textboxes appear
    foreach ((string ansName, string ansScript) in this.QuestionTextBox.AnswersScripts)
    {
      separatingStrings.Add($"_{ansName}_");
      textBoxOrder.Add(ansName);
    }

    // split text by textbox separators
    string[] textParts = this.QuestionTextBox.Text.Split(
      separatingStrings.ToArray(), 
      StringSplitOptions.RemoveEmptyEntries);

    // add viewmodels, interleaving object types
    // first insert text, then box (in order), repeating pattern
    int numberOfTextsAndBoxes = textParts.Length + textBoxOrder.Count;
    int counterTexts = 0;
    int counterBoxes = 0;
    for (int i = 0; i < numberOfTextsAndBoxes; i++)
    {
      // if even, add text item, if odd, add box item?
      if (i % 2 == 0)
        QuestionItems.Add(new QuestionTextBoxViewModelTextPart(textParts[counterTexts++]));
      else
        QuestionItems.Add(new QuestionTextBoxViewModelBoxInput(QuestionTextBox, textBoxOrder[counterBoxes++]));
    }
  }

  public bool Check()
  {
    bool allCorrect = true;
    foreach (var item in this.QuestionItems)
    {
      if (item is QuestionTextBoxViewModelBoxInput boxInput)
      {
        if (!boxInput.Check())
          allCorrect = false;
      }
    }
    return allCorrect;
  }

}

public partial class QuestionTextBoxViewModelTextPart : ObservableObject
{
  [ObservableProperty] string? _text;
  public QuestionTextBoxViewModelTextPart(string text)
  {
    Text = text;
  }
}

public partial class QuestionTextBoxViewModelBoxInput : ObservableObject
{
  private QuestionTextBox QuestionTextBox { get; set; }
  private string AnswerKey { get; set; }

  [ObservableProperty] string? _text; // user input answer
  [ObservableProperty] AnswerStatus _answerStatus = AnswerStatus.Unknown;

  public QuestionTextBoxViewModelBoxInput(QuestionTextBox questionTextBox, string answerKey)
  {
    QuestionTextBox = questionTextBox;
    AnswerKey = answerKey;
  }

  public bool Check()
  {
    bool result;

    if (this.Text is null)
      result = false;
    else
      result = QuestionTextBox.Check(this.AnswerKey, this.Text);

    AnswerStatus = result ? AnswerStatus.Correct : AnswerStatus.Incorrect;

    return result;
  }
  
}