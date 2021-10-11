using ScreenshotManager.Utils;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenshotManager.Models {
  public class ImageModel {
    public ICommand CopyImageToClipboardCommand => new AnotherCommandImplementation(ExecuteCopyImageToClipboard);
    public ICommand CopyPathToClipboardCommand => new AnotherCommandImplementation(ExecuteCopyPathToClipboard);
    public ICommand OpenImageCommand => new AnotherCommandImplementation(ExecuteOpenImage);
    public ICommand OpenFolderCommand => new AnotherCommandImplementation(ExecuteOpenFolder);

    public ImageSource ImageSource { get; }
    public string FolderName => AbsolutePath.Substring(0, AbsolutePath.Length - Filename.Length);
    public string Filename { get; }
    public string AbsolutePath { get; }

    public ImageModel(Bitmap bmp, string filename, string path) {
      this.ImageSource = Screenshot.BitmapToBitmapImage(bmp);
      this.Filename = filename;
      this.AbsolutePath = path;
    }

    public ImageModel(ImageSource image, string filename, string path) {
      this.ImageSource = image;
      this.Filename = filename;
      this.AbsolutePath = path;
    }

    public void CopyImageToClipboard() {
      Clipboard.SetImage(ImageSource as BitmapSource);
    }

    public void CopyPathToClipboard() {
      Clipboard.SetText(AbsolutePath);
    }

    private void ExecuteCopyImageToClipboard(object obj) {
      CopyImageToClipboard();
    }

    private void ExecuteCopyPathToClipboard(object obj) {
      CopyPathToClipboard();
    }

    private void ExecuteOpenImage(object obj) {
      var app = new ProcessStartInfo() {
        FileName = AbsolutePath,
        UseShellExecute = true
      };
      Process.Start(app);
    }

    private void ExecuteOpenFolder(object obj) {
      string arg = "/select, \"" + AbsolutePath + "\"";
      Process.Start("explorer", arg);
    }
  }
}
