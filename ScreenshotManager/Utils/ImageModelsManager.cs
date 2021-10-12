using ScreenshotManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScreenshotManager.Utils {
  public static class ImageModelsManager {
    private static ObservableCollection<ImageModel> _models;
    public static ObservableCollection<ImageModel> Models {
      get => _models;
      set {
        _models = value;
        NotifyStaticPropertyChanged();
      }
    }

    public static void Add(ImageModel model) => Models.Add(model);
    public static bool Remove(ImageModel model) => Models.Remove(model);
    public static bool Contains(ImageModel model) => Models.Contains(model);

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "") {
      StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    }
  }
}
