﻿using Newtonsoft.Json;
using ScreenshotManager.Utils;
using ScreenshotManager.Views;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenshotManager.Models {
  public class ImageModel : Observable {
    [JsonIgnore]
    public ICommand CopyImageToClipboardCommand => new AnotherCommandImplementation((obj) => CopyImageToClipboard());
    [JsonIgnore]
    public ICommand CopyFilepathToClipboardCommand => new AnotherCommandImplementation((obj) => CopyFilepathToClipboard());
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
    public ImageSource Thumbnail {
      get {
        return _thumbnail;
      }
      private set {
        SetProperty(ref _thumbnail, value);
      }
    }
    private ImageSource _thumbnail = ImageHelper.DefaultThumbnail;

    [JsonIgnore]
    public string FolderName => Path.GetDirectoryName(AbsolutePath);
    [JsonIgnore]
    public string Filename => Path.GetFileName(AbsolutePath);
    [JsonProperty]
    public string AbsolutePath { get; private set; }
    [JsonProperty]
    public bool Liked { get; set; }
    [JsonProperty]
    public ObservableSet<string> Tags { get; set; } = new();
    [JsonProperty]
    public ObservableSet<string> PersonTags { get; private set; } = new();
    [JsonProperty]
    public string AutoCaption { get; private set; }
    [JsonProperty]
    public string AutoCaptionKana { get; private set; }
    [JsonProperty]
    public List<FaceRecognitionResponse> FaceRecognitionResults { get; private set; }
    [JsonProperty]
    public List<TextRecognitionResponse> TextRecognitionResults { get; private set; }
    [JsonIgnore]
    public string ToolTip {
      get {
        string personTagsStr = string.Join(",", PersonTags);
        string captionStr = string.Join("\n", TextRecognitionResults.Select(x => x.Text));
        return $"{{{personTagsStr}}}\n{captionStr}";
      }
    }

    public ImageModel() { }

    public ImageModel(string path) {
      this.AbsolutePath = path;
      ThreadPool.QueueUserWorkItem(LoadImages);
    }

    public ImageModel(ImageModel _model) {
      this.AbsolutePath = _model.AbsolutePath;
      this.Liked = _model.Liked;
      this.Tags = _model.Tags;
      this.PersonTags = _model.PersonTags;
      this.AutoCaption = _model.AutoCaption;
      this.AutoCaptionKana = _model.AutoCaptionKana;
      this.FaceRecognitionResults = _model.FaceRecognitionResults;
      this.TextRecognitionResults = _model.TextRecognitionResults;
      ThreadPool.QueueUserWorkItem(LoadImages);
    }

    private void LoadImages(object obj) {
      this.Thumbnail = ImageHelper.LoadThumbnail(AbsolutePath);
      this.Image = ImageHelper.LoadBitmapImage(AbsolutePath);
      ProcessByAI();
    }

    public async void CopyImageToClipboard() {
      Clipboard.SetImage(await ImageHelper.LoadBitmapImageAsync(AbsolutePath));
    }

    public void CopyFilepathToClipboard() {
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
      HandyControl.Controls.Dialog.Show(new ImageDialog(AbsolutePath));
    }

    public void ShowEditTagsDialog() {
      HandyControl.Controls.Dialog.Show(new TagsDialog(this));
    }

    private void ProcessByAI() {
      if (FaceRecognitionResults == null) {
        ExecuteFaceRecognition();
      }
      if (TextRecognitionResults == null) {
        ExecuteTextRecognition();
      }
    }

    private void ExecuteFaceRecognition() {
      FaceRecognitionResults = RecognitionAPI.FaceRecognition(AbsolutePath);
      PersonTags = new();
      foreach (var person in FaceRecognitionResults) {
        // reject under 50%
        if (person.Confidence < 0.5) {
          continue;
        }
        PersonTags.Add(person.Name);
      }
    }

    private void ExecuteTextRecognition() {
      TextRecognitionResults = RecognitionAPI.TextRecognition(AbsolutePath);
      AutoCaption = "";
      AutoCaptionKana = "";
      foreach (var caption in TextRecognitionResults) {
        AutoCaption += caption.Text;
        AutoCaptionKana += caption.Kana;
      }
    }
  }
}
