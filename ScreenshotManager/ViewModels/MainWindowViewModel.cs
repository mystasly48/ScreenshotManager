using ScreenshotManager.Models;
using ScreenshotManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScreenshotManager.ViewModels {
  public class MainWindowViewModel : Observable {
    public ICommand TakeOneCommand => new AnotherCommandImplementation(ExecuteTakeScreenshot);
    public ICommand TakeContinuousCommand => new AnotherCommandImplementation(ExecuteTakeScreenshots);

    public static string ProductName => "ScreenshotManager";
    public static string TargetFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), ProductName);

    public ObservableCollection<ScreenModel> AllScreens { get; private set; }
    public ScreenModel TargetScreen { get; set; } = ScreenModel.GetPrimary();

    private ObservableCollection<ImageModel> _imageModels;
    public ObservableCollection<ImageModel> ImageModels {
      get => _imageModels;
      set => SetProperty(ref _imageModels, value);
    }

    private int _interval = 100;
    public int Interval {
      get => _interval;
      set => SetProperty(ref _interval, value);
    }

    private int _seconds = 3;
    public int Seconds {
      get => _seconds;
      set => SetProperty(ref _seconds, value);
    }

    public MainWindowViewModel() {
      this.AllScreens = new ObservableCollection<ScreenModel>(ScreenModel.GetAllSorted());
      this.ImageModels = new ObservableCollection<ImageModel>();
      UpdateImageModelsToLocalAsync();
    }

    private async void UpdateImageModelsToLocalAsync() {
      await Task.Run(() => {
        string[] files = GetLocalImageFiles();
        foreach (string file in files) {
          string filename = file.Substring(TargetFolder.Length);
          var model = new ImageModel(Screenshot.UrlToBitmapImage(file), filename, file);
          Application.Current.Dispatcher.Invoke(() => ImageModels.Add(model));
        }
      });
    }

    private string[] GetLocalImageFiles() {
      return Directory.GetFiles(TargetFolder, "*.jpg");
    }

    private async void ExecuteTakeScreenshot(object obj) {
      ImageModels.Add(await TakeScreenshotAsync());
    }

    private async void ExecuteTakeScreenshots(object obj) {
      foreach (ImageModel m in await TakeScreenshotsAsync(Interval, Seconds)) {
        ImageModels.Add(m);
      }
    }

    private async Task<List<ImageModel>> TakeScreenshotsAsync(int interval, int seconds) {
      int count = (int)(seconds / (interval * 0.001));
      return await Task.Run(async () => {
        List<ImageModel> models = new List<ImageModel>();
        for (int i = 0; i < count; i++) {
          models.Add(await TakeScreenshotAsync());
          if (i + 1 < count) {
            Thread.Sleep(interval);
          }
        }
        return models;
      });
    }

    private async Task<ImageModel> TakeScreenshotAsync() {
      return await Task.Run(() => {
        var bmp = TargetScreen.Take();
        var filename = Screenshot.CreateFilename();
        var path = Path.Combine(TargetFolder, filename);
        bmp.Save(path);
        return new ImageModel(bmp, filename, path);
      });
    }
  }
}
