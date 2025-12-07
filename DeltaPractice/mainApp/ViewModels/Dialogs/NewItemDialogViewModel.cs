using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;


namespace mainApp.ViewModels.Dialogs;

public partial class NewItemDialogViewModel : ObservableObject
{
  [ObservableProperty]
  private string _itemName;

  [ObservableProperty]
  private int _itemQuantity;

}