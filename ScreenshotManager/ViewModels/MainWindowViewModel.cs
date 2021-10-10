using ScreenshotManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ScreenshotManager.ViewModels {
  public class MainWindowViewModel : Observable {
    public ICommand TakeScreenshotCommand => new AnotherCommandImplementation(ExecuteTakeScreenshotCommand);
    private ImageSource _takenScreenshot;
    public ImageSource TakenScreenshot {
      get => _takenScreenshot;
      set => SetProperty(ref _takenScreenshot, value);
    }

    public MainWindowViewModel() {

    }

    private void ExecuteTakeScreenshotCommand(object obj) {
      var bmp = Screenshot.TakePrimary();
      TakenScreenshot = Screenshot.BitmapToBitmapImage(bmp);
      var filename = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.jpg";
      bmp.Save(filename);
    }
  }
}
