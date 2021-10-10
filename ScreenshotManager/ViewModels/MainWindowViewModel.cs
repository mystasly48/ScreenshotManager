using ScreenshotManager.Models;
using ScreenshotManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace ScreenshotManager.ViewModels {
  public class MainWindowViewModel : Observable {
    public ICommand TakeOneCommand => new AnotherCommandImplementation(ExecuteTakeScreenshotCommand);
    public ICommand TakeContinuousCommand => new AnotherCommandImplementation(ExecuteTakeScreenshotsCommand);

    private ImageSource _takenScreenshot;
    public ImageSource TakenScreenshot {
      get => _takenScreenshot;
      set => SetProperty(ref _takenScreenshot, value);
    }

    public ObservableCollection<ScreenModel> AllScreens { get; private set; }
    public ScreenModel TargetScreen { get; set; } = ScreenModel.GetPrimary();

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
    }

    private void ExecuteTakeScreenshotCommand(object obj) {
      var bmp = TargetScreen.Take();
      TakenScreenshot = Screenshot.BitmapToBitmapImage(bmp);
      var filename = Screenshot.CreateFilename();
      bmp.Save(filename);
    }

    private void ExecuteTakeScreenshotsCommand(object obj) {
      TakeScreenshotsAsync(Interval, Seconds);
    }

    private async void TakeScreenshotsAsync(int interval, int seconds) {
      int count = (int)(seconds / (interval * 0.001));
      await Task.Run(() => {
        for (int i = 0; i < count; i++) {
          TargetScreen.SaveAsync();
          if (i + 1 < count) {
            Thread.Sleep(interval);
          }
        }
      });
    }
  }
}
