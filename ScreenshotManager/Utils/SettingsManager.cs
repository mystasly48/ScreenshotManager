using Newtonsoft.Json;
using ScreenshotManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ScreenshotManager.Utils {
  public static class SettingsManager {
#if DEBUG
    public static string ProductName => "ScrenshotManagerDebug";
#else
    public static string ProductName => "ScreenshotManager";
#endif

#if DEBUG
    public static string ProductTitle => "Screenshot Manager - Debug";
#else
    public static string ProductTitle => "Screenshot Manager";
#endif

    public static string DefaultScreenshotFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), ProductName);
    public static string SettingsFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ProductName);
    public static string ImageModelsSettingFilename => "Images.json";
    public static string ImageModelsSettingFilePath => Path.Combine(SettingsFolder, ImageModelsSettingFilename);
    public static string ApplicationSettingFilename => "Application.json";
    public static string ApplicationSettingFilePath => Path.Combine(SettingsFolder, ApplicationSettingFilename);
    public static string AutoCategorizedTagsFilename => "AutoCategorizedTags.json";
    public static string AutoCategorizedTagsFilePath => Path.Combine(SettingsFolder, AutoCategorizedTagsFilename);

    public static int SelectedScreenIndex {
      get => _model.SelectedScreenIndex;
      set => _model.SelectedScreenIndex = value;
    }
    public static int Interval {
      get => _model.Interval;
      set => _model.Interval = value;
    }
    public static int Seconds {
      get => _model.Seconds;
      set => _model.Seconds = value;
    }
    public static string ScreenshotFolder {
      get => _model.ScreenshotFolder;
      set => _model.ScreenshotFolder = value;
    }
    public static Size WindowSize {
      get => _model.WindowSize;
      set => _model.WindowSize = value;
    }
    public static Point WindowLocation {
      get => _model.WindowLocation;
      set => _model.WindowLocation = value;
    }
    public static WindowState WindowState {
      get => _model.WindowState;
      set => _model.WindowState = value;
    }
    public static Key GlobalHotKey {
      get => _model.GlobalHotKey;
      set => _model.GlobalHotKey = value;
    }

    private static Dictionary<string, List<string>> _autoTags;
    public static Dictionary<string, List<string>> AutoCategorizedTags => _autoTags;

    private static ApplicationSettingModel _model;

    public static void Initialize() {
      Directory.CreateDirectory(SettingsFolder);
      Load();
    }

    private static void Load() {
      _autoTags = LoadAutoTags();
      if (!File.Exists(ApplicationSettingFilePath)) {
        _model = new ApplicationSettingModel();
        Save();
        return;
      }
      using (var reader = new StreamReader(ApplicationSettingFilePath, Encoding.UTF8)) {
        var json = reader.ReadToEnd();
        _model = JsonConvert.DeserializeObject<ApplicationSettingModel>(json);
      }
    }

    public static void Save() {
      using (var writer = new StreamWriter(ApplicationSettingFilePath, false, Encoding.UTF8)) {
        var json = JsonConvert.SerializeObject(_model, Formatting.Indented);
        writer.WriteLine(json);
      }
    }

    private static string DecodeName(string encodedName) {
      var bytes = Encoding.UTF8.GetBytes(encodedName);
      var decodedName = Encoding.UTF8.GetString(bytes);
      return decodedName;
    }

    public static Dictionary<string, List<string>> LoadAutoTags() {
      var dict = new Dictionary<string, List<string>>();
      if (!File.Exists(AutoCategorizedTagsFilePath)) {
        return dict;
      }
      using (var reader = new StreamReader(AutoCategorizedTagsFilePath, Encoding.UTF8)) {
        var json = reader.ReadToEnd();
        dynamic raw = JsonConvert.DeserializeObject(json);
        foreach (var image in raw) {
          string filename = image[0];
          foreach (var result in image[1]) {
            double confidence = result[1];
            // reject under 50% prob
            if (confidence < 0.5) {
              continue;
            }
            string name = result[0];

            if (!dict.ContainsKey(name)) {
              dict[name] = new List<string>();
            }
            dict[name].Add(filename);
          }
        }
      }
      return dict;
    }
  }
}
