using ScreenshotManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ScreenshotManager.Utils {
  public static class TagModelsManager {
    private static ObservableCollection<TagModel> _models = new();
    public static ObservableCollection<TagModel> Models {
      get => _models;
      private set {
        _models = value;
        NotifyStaticPropertyChanged();
      }
    }

    private static ObservableCollection<TagModel> _autoModels = new();
    public static ObservableCollection<TagModel> AutoModels {
      get => _autoModels;
      private set {
        _autoModels = value;
        NotifyStaticPropertyChanged();
      }
    }

    public static void Initialize() {
      ImageModelsManager.StaticPropertyChanged += ImageModelsManager_StaticPropertyChanged;
    }

    private static void ImageModelsManager_StaticPropertyChanged(object sender, PropertyChangedEventArgs e) {
      // FIXME: weird coding...
      if (ImageModelsManager.IsModelsAvailable) {
        Models = GetTagModelsSorted();
        AutoModels = GetAutoTagModelsSorted();
      }
    }

    public static ObservableSet<TagModel> GetTagModels(ObservableSet<string> tags) {
      return new ObservableSet<TagModel>(
        ImageModelsManager.Models
        .SelectMany(model => model.Tags)
        .Distinct()
        .OrderBy(tag => tag)
        .Select(tag => {
          if (tags.Contains(tag)) {
            return new TagModel(tag, true);
          } else {
            return new TagModel(tag);
          }
        }));
    }

    public static ObservableCollection<TagModel> GetTagModelsSorted()
      => new ObservableCollection<TagModel>(ImageModelsManager.Models
        .SelectMany(model => model.Tags)
        .Distinct()
        .OrderBy(tag => tag)
        .Select(tag => new TagModel(tag)));

    public static ObservableCollection<TagModel> GetAutoTagModelsSorted()
      => new ObservableCollection<TagModel>(SettingsManager.AutoCategorizedTags.Keys
        .OrderBy(name => name)
        .Select(name => new TagModel(name)));

    public static ObservableSet<string> GetSelectedTags(ObservableSet<TagModel> models)
      => new ObservableSet<string>(models.Where(model => model.IsSelected).Select(model => model.Name));

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "") {
      StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    }
  }
}
