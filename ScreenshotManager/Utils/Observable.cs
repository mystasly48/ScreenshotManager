using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScreenshotManager.Utils {
  public class Observable : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
      storage = value;
      OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected void OnPropertyChanged(string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
