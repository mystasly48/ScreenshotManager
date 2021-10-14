using ScreenshotManager.Utils;
using System.Windows;
using System.Windows.Input;

namespace ScreenshotManager.Models {
  public class ApplicationSettingModel {
    public int SelectedScreenIndex { get; set; } = -1;
    public int Interval { get; set; } = 100;
    public int Seconds { get; set; } = 3;
    // TODO
    public string ScreenshotFolder { get; set; } = SettingsManager.DefaultScreenshotFolder;
    public Size WindowSize { get; set; } = new Size(1200, 700);
    public Point WindowLocation { get; set; } = new Point(0, 0);
    public WindowState WindowState { get; set; } = WindowState.Normal;
    // TODO
    public Key GlobalHotKey { get; set; } = Key.PrintScreen;
  }
}
