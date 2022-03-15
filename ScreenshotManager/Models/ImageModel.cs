using Newtonsoft.Json;
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
  public class ImageModel : Observable {
    [JsonIgnore]
    public ICommand CopyImageToClipboardCommand => new AnotherCommandImplementation((obj) => CopyImageToClipboard());
    [JsonIgnore]
    public ICommand CopyPathToClipboardCommand => new AnotherCommandImplementation((obj) => CopyPathToClipboard());
    [JsonIgnore]
    public ICommand CopyFilenameToClipboardCommand => new AnotherCommandImplementation((obj) => CopyFilenameToClipboard());
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
    public ImageSource ImageSource => Screenshot.LoadBitmapImage(AbsolutePath);
    [JsonIgnore]
    public ImageSource Thumbnail { get; }
    [JsonIgnore]
    public string FolderName => Path.GetDirectoryName(AbsolutePath);
    [JsonIgnore]
    public string Filename => Path.GetFileName(AbsolutePath);
    [JsonProperty]
    public string AbsolutePath { get; private set; }
    private bool _isSelected;
    [JsonIgnore]
    public bool IsSelected {
      get => _isSelected;
      set => SetProperty(ref _isSelected, value);
    }
    [JsonProperty]
    public ObservableSet<string> Tags { get; set; } = new();

    const int THUMBNAIL_WIDTH = 320;
    const int THUMBNAIL_HEIGHT = 180;

    public ImageModel() { }

    public ImageModel(string path) {
      this.Thumbnail = Screenshot.LoadThumbnail(path, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT);
      this.AbsolutePath = path;
    }

    public ImageModel(string path, ObservableSet<string> tags) {
      this.Thumbnail = Screenshot.LoadThumbnail(path, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT);
      this.AbsolutePath = path;
      this.Tags = tags;
    }

    public void CopyImageToClipboard() {
      Clipboard.SetImage(ImageSource as BitmapSource);
    }

    public void CopyPathToClipboard() {
      Clipboard.SetText(AbsolutePath);
    }

    public void CopyFilenameToClipboard() {
      Clipboard.SetText(Filename);
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
