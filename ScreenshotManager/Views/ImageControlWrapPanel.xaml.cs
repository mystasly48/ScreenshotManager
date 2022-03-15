using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace ScreenshotManager.Views {
  public partial class ImageControlWrapPanel : UserControl {
    public IEnumerable ItemsSource {
      get { return (IEnumerable)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ImageControlWrapPanel), new PropertyMetadata(null));

    public ImageControlWrapPanel() {
      InitializeComponent();
      this.DataContext = this;
    }
  }
}
