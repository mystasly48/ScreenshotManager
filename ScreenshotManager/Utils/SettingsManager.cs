using Newtonsoft.Json;
using ScreenshotManager.Models;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ScreenshotManager.Utils {
  public static class SettingsManager {
#if DEBUG
    public static string ProductName => "ScreenshotManagerDebug";
#else
    public static string ProductName => "ScreenshotManager";
#endif

    public static string DefaultScreenshotFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), ProductName);
    public static string SettingsFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ProductName);
    public static string ImageModelsSettingFilename => "Images.json";
    public static string ImageModelsSettingFilePath => Path.Combine(SettingsFolder, ImageModelsSettingFilename);
    public static string ApplicationSettingFilename => "Application.json";
    public static string ApplicationSettingFilePath => Path.Combine(SettingsFolder, ApplicationSettingFilename);

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

    private static ApplicationSettingModel _model;

    public static void Initialize() {
      Directory.CreateDirectory(SettingsFolder);
      Load();
    }

    private static void Load() {
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
  }
}
