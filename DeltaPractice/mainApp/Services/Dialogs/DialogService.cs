using mainApp.Views.Dialogs;
using mainApp.Views.Windows;

namespace mainApp.Services.Dialogs;

public class DialogService : IDialogService
{
  public void ShowDialog(object viewModel)
  {
    var dialog = new NewItemDialogWindow();
    dialog.DataContext = viewModel;
    dialog.ShowDialog();
  }

  public void ShowPractice(object viewModel)
  {
    var window = new PracticeWindow();
    window.DataContext = viewModel;
    window.ShowDialog();
  }
}