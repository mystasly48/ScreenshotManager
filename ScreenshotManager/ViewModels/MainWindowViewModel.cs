using ScreenshotManager.Models;
using ScreenshotManager.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenshotManager.ViewModels {
  public class MainWindowViewModel : Observable {
    public ICommand TakeOneCommand => new AnotherCommandImplementation(ExecuteTakeScreenshot);
    public ICommand TakeContinuousCommand => new AnotherCommandImplementation(ExecuteTakeScreenshots);

    public ObservableCollection<ScreenModel> AllScreens { get; private set; }

    private int _selectedScreenIndex;
    public int SelectedScreenIndex {
      get => _selectedScreenIndex;
      set {
        _selectedScreenIndex = value;
        SetProperty(ref _selectedScreenIndex, value);
      }
    }

    private ScreenModel _selectedScreen;
    public ScreenModel SelectedScreen {
      get => _selectedScreen;
      set {
        _selectedScreen = value;
        SetProperty(ref _selectedScreen, value);
        SelectedScreenIndex = AllScreens.IndexOf(value);
      }
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
      this.SelectedScreen = ScreenModel.GetPrimary();
      ImageModelsManager.Initialize();
    }

    private async void ExecuteTakeScreenshot(object obj) {
      ImageModelsManager.Add(await TakeScreenshotAsync());
    }

    private async void ExecuteTakeScreenshots(object obj) {
      foreach (ImageModel m in await TakeScreenshotsAsync(Interval, Seconds)) {
        ImageModelsManager.Add(m);
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
        var bmp = SelectedScreen.Take();
        var filename = Screenshot.CreateFilename();
        var path = Path.Combine(Settings.ScreenshotFolder, filename);
        bmp.Save(path);
        return new ImageModel(bmp, path);
      });
    }
  }
}
