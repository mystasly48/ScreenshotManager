using Newtonsoft.Json;
using ScreenshotManager.Utils;
using ScreenshotManager.Views;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenshotManager.Models {
  public class ImageModel {
    [JsonIgnore]
    public ICommand CopyImageToClipboardCommand => new AnotherCommandImplementation((obj) => CopyImageToClipboard());
    [JsonIgnore]
    public ICommand CopyPathToClipboardCommand => new AnotherCommandImplementation((obj) => CopyPathToClipboard());
    [JsonIgnore]
    public ICommand EditTagsCommand => new AnotherCommandImplementation((obj) => ShowEditTagsDialog());
    [JsonIgnore]
    public ICommand OpenImageCommand => new AnotherCommandImplementation((obj) => OpenImage());
    [JsonIgnore]
    public ICommand OpenFolderCommand => new AnotherCommandImplementation((obj) => OpenFolder());
    [JsonIgnore]
    public ICommand RemoveImageCommand => new AnotherCommandImplementation((obj) => RemoveImage());
    [JsonIgnore]
    public ICommand ShowImageCommand => new AnotherCommandImplementation((obj) => ShowImageDialog());

    [JsonIgnore]
    public ImageSource ImageSource { get; }
    [JsonIgnore]
    public string FolderName => Path.GetDirectoryName(AbsolutePath);
    [JsonIgnore]
    public string Filename => Path.GetFileName(AbsolutePath);
    public string AbsolutePath { get; }
    public HashSet<string> Tags { get; set; } = new();

    public ImageModel(string path) {
      this.ImageSource = Screenshot.UrlToBitmapImage(path);
      this.AbsolutePath = path;
    }

    public ImageModel(Bitmap bmp, string path) {
      this.ImageSource = Screenshot.BitmapToBitmapImage(bmp);
      this.AbsolutePath = path;
    }

    public ImageModel(ImageSource image, string path) {
      this.ImageSource = image;
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

    public void ShowImageDialog() {
      HandyControl.Controls.Dialog.Show(new ImageDialog(ImageSource));
    }

    public void ShowEditTagsDialog() {
      HandyControl.Controls.Dialog.Show(new TagsDialog(this));
    }
  }
}
