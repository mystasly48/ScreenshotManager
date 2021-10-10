using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScreenshotManager.Utils {
  public class Observable : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
      if (Equals(storage, value)) {
        return false;
      }

      storage = value;

      OnPropertyChanged(propertyName);

      return true;
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
