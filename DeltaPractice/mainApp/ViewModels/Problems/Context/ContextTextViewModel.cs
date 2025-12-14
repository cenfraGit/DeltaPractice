using CommunityToolkit.Mvvm.ComponentModel;
using core.classes.context;

namespace mainApp.ViewModels.Windows;

public partial class ContextTextViewModel : ObservableObject
{
  private ContextText ContextText { get; set; }
  [ObservableProperty] public string? _value;

  public ContextTextViewModel(ContextText contextText)
  {
    ContextText = contextText;
    Value = ContextText.Value;
  }
}