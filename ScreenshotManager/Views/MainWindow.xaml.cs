using ScreenshotManager.Utils;
using ScreenshotManager.ViewModels;
using System.Windows;

namespace ScreenshotManager.Views {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      this.DataContext = new MainWindowViewModel();

      if (IsInsideScreens(SettingsManager.WindowLocation, SettingsManager.WindowSize)) {
        this.Left = SettingsManager.WindowLocation.X;
        this.Top = SettingsManager.WindowLocation.Y;
        this.Width = SettingsManager.WindowSize.Width;
        this.Height = SettingsManager.WindowSize.Height;
      }
      if (SettingsManager.WindowState != WindowState.Minimized) {
        this.WindowState = SettingsManager.WindowState;
      }
    }

    private bool IsInsideScreens(Point point, Size size) {
      if (point.X >= Screenshot.FullscreenX && point.Y >= Screenshot.FullscreenY) {
        if (point.X + size.Width <= Screenshot.FullscreenWidth && point.Y + size.Height <= Screenshot.FullscreenHeight) {
          return true;
        }
      }
      return false;
    }

    private void ScrollToTopButton_Click(object sender, RoutedEventArgs e) {
      ImageModelsScrollViewer.ScrollToTop();
    }

    private void ScrollToBottomButton_Click(object sender, RoutedEventArgs e) {
      ImageModelsScrollViewer.ScrollToBottom();
    }

    private void Window_Closed(object sender, System.EventArgs e) {
      SettingsManager.WindowLocation = new Point(this.Left, this.Top);
      SettingsManager.WindowSize = new Size(this.Width, this.Height);
      SettingsManager.WindowState = this.WindowState;
      SettingsManager.Save();
    }
  }
}
