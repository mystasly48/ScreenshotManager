using Newtonsoft.Json;
using ScreenshotManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenshotManager.Utils {
  public static class ImageModelsManager {
    private static ObservableCollection<ImageModel> _models = new();
    public static ObservableCollection<ImageModel> Models {
      get => _models;
      set {
        _models = value;
        NotifyStaticPropertyChanged();
      }
    }

    public static void Add(ImageModel model) {
      Models.Add(model);
    }

    public static bool Remove(ImageModel model) {
      var res = Models.Remove(model);
      return res;
    }

    public static bool Contains(ImageModel model) => Models.Contains(model);

    public static void Initialize() {
      Directory.CreateDirectory(Settings.SettingsFolder);
      UpdateImageModelsToLocalAsync();
    }

    public async static void UpdateImageModelsToLocalAsync() {
      await Task.Run(() => {
        string[] files = GetLocalImageFiles();
        foreach (string file in files) {
          var model = new ImageModel(file);
          // FIXME: Crash here when close the app while this is running
          Application.Current.Dispatcher.Invoke(() => Add(model));
        }
      });
    }

    public static string[] GetLocalImageFiles() {
      return Directory.GetFiles(Settings.ScreenshotFolder, "*.jpg");
    }

    private static void LoadImageModelsSetting() {
      using (var reader = new StreamReader(Settings.ImageModelsSettingFilePath, Encoding.UTF8)) {
        var json = reader.ReadToEnd();
        var models = JsonConvert.DeserializeObject<List<ImageModel>>(json);
        Models = new ObservableCollection<ImageModel>(models);
      }
    }

    public static void Save() {
      using (var writer = new StreamWriter(Settings.ImageModelsSettingFilePath, false, Encoding.UTF8)) {
        var models = new List<ImageModel>(Models);
        var json = JsonConvert.SerializeObject(models, Formatting.Indented);
        writer.WriteLine(json);
      }
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "") {
      StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    }
  }
}
