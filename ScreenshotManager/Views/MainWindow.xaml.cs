using ScreenshotManager.ViewModels;
using System.Windows;

namespace ScreenshotManager.Views {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      this.DataContext = new MainWindowViewModel();
    }
  }
}
