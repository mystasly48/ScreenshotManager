using System;
using System.IO;

namespace ScreenshotManager.Utils {
  public static class Settings {
#if DEBUG
    public static string ProductName => "ScrenshotManagerDebug";
#else
    public static string ProductName => "ScreenshotManager";
#endif

    public static string ScreenshotFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), ProductName);
    public static string SettingsFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ProductName);
    
    public static string ImageModelsSettingFilename => "Images.json";
    public static string ImageModelsSettingFilePath => Path.Combine(SettingsFolder, ImageModelsSettingFilename);
  }
}
