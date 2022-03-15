using ScreenshotManager.Hotkeys;
using ScreenshotManager.Models;
using ScreenshotManager.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenshotManager.ViewModels {
  public class MainWindowViewModel : Observable {
    public ICommand TakeOneCommand => new AnotherCommandImplementation(ExecuteTakeScreenshot);
    public ICommand TakeContinuousCommand => new AnotherCommandImplementation(ExecuteTakeScreenshots);
    public ICommand SearchTagCommand => new AnotherCommandImplementation(ExecuteSearchTag);
    public ICommand ImageModelSelectCommand => new AnotherCommandImplementation(ExecuteImageModelSelection);
    public ICommand ClosingCommand => new AnotherCommandImplementation(ExecuteClosing);

    public ObservableCollection<ScreenModel> AllScreens { get; private set; }

    private int _selectedScreenIndex;
    public int SelectedScreenIndex {
      get => _selectedScreenIndex;
      set {
        _selectedScreen = AllScreens[value];
        SettingsManager.SelectedScreenIndex = value;
        SetProperty(ref _selectedScreenIndex, value);
      }
    }

    private ScreenModel _selectedScreen;
    public ScreenModel SelectedScreen {
      get => _selectedScreen;
      set {
        _selectedScreenIndex = AllScreens.IndexOf(value);
        SettingsManager.SelectedScreenIndex = _selectedScreenIndex;
        SetProperty(ref _selectedScreen, value);
      }
    }

    private int _interval;
    public int Interval {
      get => _interval;
      set {
        SettingsManager.Interval = value;
        SetProperty(ref _interval, value);
      }
    }

    private int _seconds;
    public int Seconds {
      get => _seconds;
      set {
        SettingsManager.Seconds = value;
        SetProperty(ref _seconds, value);
      }
    }

    private bool _visibleSearchResults;
    public bool VisibleSearchResults {
      get => _visibleSearchResults;
      set => SetProperty(ref _visibleSearchResults, value);
    }

    private int _selectedSearchTagIndex;
    public int SelectedSearchTagIndex {
      get => _selectedSearchTagIndex;
      set => SetProperty(ref _selectedSearchTagIndex, value);
    }

    public TagModel SelectedSearchTagModel {
      get {
        // consider that index 0 is "please select"
        if (SelectedSearchTagIndex == 0) {
          return null;
        } else {
          return TagModelsManager.Models[SelectedSearchTagIndex - 1]; 
        }
      }
    }

    private ObservableCollection<ImageModel> _searchResutImageModels;
    public ObservableCollection<ImageModel> SearchResultImageModels {
      get => _searchResutImageModels;
      set => SetProperty(ref _searchResutImageModels, value);
    }

    private KeyboardHook _hotkey;

    public MainWindowViewModel() {
      SettingsManager.Initialize();
      ImageModelsManager.Initialize();
      TagModelsManager.Initialize();

      this.AllScreens = new ObservableCollection<ScreenModel>(ScreenModel.GetAllSorted());
      if (0 <= SettingsManager.SelectedScreenIndex && SettingsManager.SelectedScreenIndex < this.AllScreens.Count) {
        this.SelectedScreenIndex = SettingsManager.SelectedScreenIndex;
      } else {
        this.SelectedScreen = ScreenModel.GetPrimary();
      }
      this.Interval = SettingsManager.Interval;
      this.Seconds = SettingsManager.Seconds;

      _hotkey = new KeyboardHook();
      _hotkey.RegisterHotkey(Hotkeys.ModifierKeys.None, System.Windows.Forms.Keys.PrintScreen);
      _hotkey.KeyPressed += PrintScreen_KeyPressed;
    }

    private void PrintScreen_KeyPressed(object sender, KeyPressedEventArgs e) {
      ExecuteTakeScreenshot(null);
    }

    private async void ExecuteTakeScreenshot(object obj) {
      ImageModelsManager.Add(await TakeScreenshotAsync());
    }

    private async void ExecuteTakeScreenshots(object obj) {
      // TODO: take screenshots in background and show the results synchronously
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
        var path = Path.Combine(SettingsManager.ScreenshotFolder, filename);
        bmp.Save(path);
        return new ImageModel(bmp, path);
      });
    }

    private void ExecuteSearchTag(object obj) {
      if (SelectedSearchTagModel == null) {
        // Change ItemsSource to ImageModelsManager.Models
        VisibleSearchResults = false;
      } else {
        // Change ItemsSource to SearchResultImageModels
        VisibleSearchResults = true;
        var tagName = SelectedSearchTagModel.Name;
        var models = ImageModelsManager.Models.Where(model => model.Tags.Contains(tagName));
        SearchResultImageModels = new ObservableCollection<ImageModel>(models);
      }
    }

    private void ExecuteImageModelSelection(object obj) {
      System.Diagnostics.Debug.WriteLine("MainWindowViewModel: ImageModelSelectCommand");
      System.Diagnostics.Debug.WriteLine(obj.GetType());
      System.Diagnostics.Debug.WriteLine(obj);
      if (obj is ImageModel model) {
        model.IsSelected = !model.IsSelected;
      }
    }

    private void ExecuteClosing(object obj) {
      SettingsManager.Save();
      ImageModelsManager.Save();
      _hotkey.Dispose();
    }
  }
}
