using ScreenshotManager.Models;
using System.Windows;
using System.Windows.Controls;

namespace ScreenshotManager.Views {
  public partial class ImageControl : UserControl {
    public ImageModel ImageModel {
      get { return (ImageModel)GetValue(ImageModelProperty); }
      set { SetValue(ImageModelProperty, value); }
    }
    public static readonly DependencyProperty ImageModelProperty =
        DependencyProperty.Register("ImageModel", typeof(ImageModel), typeof(ImageControl), new PropertyMetadata(null));

    public ImageControl() {
      InitializeComponent();
    }
  }
}
