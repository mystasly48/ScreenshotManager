using ScreenshotManager.ViewModels;

namespace ScreenshotManager.Views {
  public partial class ImageDialog {
    public ImageDialog(string image_absolute_path) {
      InitializeComponent();
      this.DataContext = new ImageDialogViewModel(image_absolute_path);
    }
  }
}
