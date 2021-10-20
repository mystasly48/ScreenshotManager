using Newtonsoft.Json;
using ScreenshotManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenshotManager.Utils {
  public static class ImageModelsManager {
    private static ObservableCollection<ImageModel> _models = new();
    public static ObservableCollection<ImageModel> Models {
      get => _models;
      private set {
        _models = value;
        NotifyStaticPropertyChanged();
      }
    }

    private static bool _isModelsAvailable = false;
    public static bool IsModelsAvailable {
      get => _isModelsAvailable;
      private set {
        _isModelsAvailable = value;
        NotifyStaticPropertyChanged();
      }
    }

    private static readonly string[] AcceptedImageExtensions = new []{ ".jpg", ".jpeg", ".png" };

    public static void Add(ImageModel model) => Models.Add(model);
    public static bool Remove(ImageModel model) => Models.Remove(model);
    public static bool Contains(ImageModel model) => Models.Contains(model);

    public static void AddAll(List<ImageModel> models) {
      foreach (var model in models) {
        Models.Add(model);
      }
    }

    public static void Initialize() {
      Directory.CreateDirectory(SettingsManager.ScreenshotFolder);
      UpdateImageModelsToLocalAsync();
    }

    // FIXME: Crash here when you close the app while this is running
    public async static void UpdateImageModelsToLocalAsync() {
      await Task.Run(() => {
        IsModelsAvailable = false;
        List<ImageModel> modelsFromJson = Load();
        string[] files = GetLocalImageFiles();
        // Add to Models every 50 files
        List<ImageModel> tempModels = new List<ImageModel>();
        const int MODELS_COUNT = 50;
        foreach (var file in files) {
          var matchedModel = modelsFromJson.FirstOrDefault(model => model.AbsolutePath == file);
          if (matchedModel == null) {
            // newly added
            var model = new ImageModel(file);
            tempModels.Add(model);
          } else {
            // already exists
            var model = new ImageModel(matchedModel.AbsolutePath, matchedModel.Tags);
            tempModels.Add(model);
          }
          if (tempModels.Count == MODELS_COUNT) {
            Application.Current.Dispatcher.Invoke(() => AddAll(tempModels));
            tempModels.Clear();
          }
        }
        if (tempModels.Count > 0) {
          Application.Current.Dispatcher.Invoke(() => AddAll(tempModels));
        }
        IsModelsAvailable = true;
        Save();
      });
    }

    public static string[] GetLocalImageFiles() {
      List<string> result = new();
      foreach (var extension in AcceptedImageExtensions) {
        result.AddRange(Directory.GetFiles(SettingsManager.ScreenshotFolder, $"*{extension}").Where(file => file.EndsWith(extension)));
      }
      return result.OrderBy(x => x).ToArray();
    }

    // Note: ImageSource is dead, thus you need to re-instantiate it using AbsolutePath.
    private static List<ImageModel> Load() {
      if (!File.Exists(SettingsManager.ImageModelsSettingFilePath)) {
        return new List<ImageModel>();
      }
      using (var reader = new StreamReader(SettingsManager.ImageModelsSettingFilePath, Encoding.UTF8)) {
        var json = reader.ReadToEnd();
        var modelsFromJson = JsonConvert.DeserializeObject<List<ImageModel>>(json);
        return modelsFromJson;
      }
    }

    public static void Save() {
      using (var writer = new StreamWriter(SettingsManager.ImageModelsSettingFilePath, false, Encoding.UTF8)) {
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
