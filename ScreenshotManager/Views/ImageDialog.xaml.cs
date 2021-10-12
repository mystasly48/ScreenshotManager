using System.Windows.Media;

namespace ScreenshotManager.Views {
  public partial class ImageDialog {
    public ImageSource Image { get; }

    public ImageDialog(ImageSource image) {
      InitializeComponent();
      this.DataContext = this;
      this.Image = image;
    }
  }
}
