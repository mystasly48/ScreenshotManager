using ScreenshotManager.Utils;
using System.Drawing;
using System.Windows.Media;

namespace ScreenshotManager.Models {
  public class ImageModel {
    public ImageSource ImageSource { get; }
    public string Filename { get; }

    public ImageModel(Bitmap bmp, string filename) {
      this.ImageSource = Screenshot.BitmapToBitmapImage(bmp);
      this.Filename = filename;
    }

    public ImageModel(ImageSource image, string filename) {
      this.ImageSource = image;
      this.Filename = filename;
    }
  }
}
