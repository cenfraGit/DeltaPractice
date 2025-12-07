using mainApp.Views.Dialogs;

namespace mainApp.Services.Dialogs;

public class DialogService : IDialogService
{
  public void ShowDialog(object viewModel)
  {
    var dialog = new NewItemDialogWindow();
    dialog.DataContext = viewModel;
    dialog.ShowDialog();
  }
}