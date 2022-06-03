using ScreenshotManager.Utils;
using System.Windows.Media;

namespace ScreenshotManager.ViewModels {
  public class ImageDialogViewModel : Observable {
    private ImageSource _image;
    public ImageSource Image {
      get => _image;
      private set {
        _image = value;
        SetProperty(ref _image, value);
      }
    }

    public ImageDialogViewModel(string image_absolute_path) {
      LoadImageAsync(image_absolute_path);
    }

    private async void LoadImageAsync(string image_absolute_path) {
      this.Image = await Screenshot.LoadBitmapImageAsync(image_absolute_path);
    }
  }
}
