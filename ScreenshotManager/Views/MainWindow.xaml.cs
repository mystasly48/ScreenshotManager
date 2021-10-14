using ScreenshotManager.ViewModels;
using System.Windows;

namespace ScreenshotManager.Views {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      this.DataContext = new MainWindowViewModel();
    }

    private void ScrollToTopButton_Click(object sender, RoutedEventArgs e) {
      ImageModelsScrollViewer.ScrollToTop();
    }

    private void ScrollToBottomButton_Click(object sender, RoutedEventArgs e) {
      ImageModelsScrollViewer.ScrollToBottom();
    }
  }
}
