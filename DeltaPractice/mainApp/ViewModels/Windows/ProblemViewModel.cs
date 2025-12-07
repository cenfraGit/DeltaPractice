using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mainApp.ViewModels.Windows;

public class ProblemViewModel : ObservableObject
{
  public ObservableCollection<object> ContentItems { get; set; } = new()
  {
    new TextViewModel {Text = "this is an example"},
    new TextViewModel {Text = "this is another example"}
  };
}

public partial class TextViewModel : ObservableObject
{
  [ObservableProperty] public string _text;

  [ObservableProperty] string _textItem;
}