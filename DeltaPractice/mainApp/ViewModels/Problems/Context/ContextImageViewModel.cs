using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using core.classes.context;

namespace mainApp.ViewModels.Windows;

public partial class ContextImageViewModel : ObservableObject
{
  private ContextImage ContextImage { get; set; }
  [ObservableProperty] string? _value;
  [ObservableProperty] ImageSource? _image;

  public ContextImageViewModel(ContextImage contextImage)
  {
    ContextImage = contextImage;
    Value = ContextImage.Value;

    if (!string.IsNullOrEmpty(Value))
    {
      byte[] imageBytes = Convert.FromBase64String(Value);

      using (var ms = new MemoryStream(imageBytes))
      {
        var image = new BitmapImage();

        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = ms;
        image.EndInit();
        image.Freeze();
        Image = image;
      }
    }

  }
}