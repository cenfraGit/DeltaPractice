using System.Windows;

namespace mainApp.Models.Services;

// --------------------------------------------------------------------------------
// IDialogService
// --------------------------------------------------------------------------------

public interface IDialogService
{
  void RegisterDialog<TViewModel, TView>() where TView : Window;
  bool? ShowDialog<TViewModel>(TViewModel viewModel);
}

// --------------------------------------------------------------------------------
// DialogService
// --------------------------------------------------------------------------------

public class DialogService : IDialogService
{
  /// <summary>
  /// This dict will hold the mappings between the viewmodels and views, so
  /// the service knows which view to display after passing the viewmodel to 
  /// ShowDialog.
  /// </summary>
  private readonly Dictionary<Type, Type> _mappings = [];

  public void RegisterDialog<TViewModel, TView>() where TView : Window
  {
    _mappings[typeof(TViewModel)] = typeof(TView);
  }

  public bool? ShowDialog<TViewModel>(TViewModel viewModel)
  {
    Type viewModelType = typeof(TViewModel);

    if (!_mappings.TryGetValue(viewModelType, out Type? viewType))
    {
      throw new ArgumentException($"No dialog VIEW registered for VIEWMODEL type: {viewModelType.Name}");
    }

    if (viewType is null)
    {
      throw new ArgumentNullException($"Dialog VIEW for {viewModelType.Name} is null.");
    }

    object dialogViewInstance = Activator.CreateInstance(viewType) ?? 
      throw new Exception($"Activator.CreateInstance returned null for {viewModelType.Name}");

    Window dialog = (Window)dialogViewInstance;
    dialog.DataContext = viewModel;
    return dialog.ShowDialog();
  }
}