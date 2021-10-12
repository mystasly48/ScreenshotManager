using ScreenshotManager.Utils;
using ScreenshotManager.Views;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenshotManager.Models {
  public class ImageModel {
    public ICommand CopyImageToClipboardCommand => new AnotherCommandImplementation((obj) => CopyImageToClipboard());
    public ICommand CopyPathToClipboardCommand => new AnotherCommandImplementation((obj) => CopyPathToClipboard());
    public ICommand OpenImageCommand => new AnotherCommandImplementation((obj) => OpenImage());
    public ICommand OpenFolderCommand => new AnotherCommandImplementation((obj) => OpenFolder());
    public ICommand RemoveImageCommand => new AnotherCommandImplementation((obj) => RemoveImage());
    public ICommand ShowImageCommand => new AnotherCommandImplementation((obj) => ShowImage());

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

    public void OpenImage() {
      var app = new ProcessStartInfo() {
        FileName = AbsolutePath,
        UseShellExecute = true
      };
      Process.Start(app);
    }

    public void OpenFolder() {
      string arg = "/select, \"" + AbsolutePath + "\"";
      Process.Start("explorer", arg);
    }

    public void RemoveImage() {
      if (ImageModelsManager.Contains(this)) {
        ImageModelsManager.Remove(this);
      }
      if (File.Exists(AbsolutePath)) {
        File.Delete(AbsolutePath);
      }
    }

    public void ShowImage() {
      HandyControl.Controls.Dialog.Show(new ImageDialog(ImageSource));
    }
  }
}
