using ScreenshotManager.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ScreenshotManager.Utils {
  public static class TagModelsManager {
    private static ObservableCollection<TagModel> _tagModels = new();
    public static ObservableCollection<TagModel> TagModels {
      get => _tagModels;
      private set {
        _tagModels = value;
        NotifyStaticPropertyChanged();
      }
    }

    private static ObservableCollection<TagModel> _personTagModels = new();
    public static ObservableCollection<TagModel> PersonTagModels {
      get => _personTagModels;
      private set {
        _personTagModels = value;
        NotifyStaticPropertyChanged();
      }
    }

    public static void Initialize() {
      ImageModelsManager.StaticPropertyChanged += ImageModelsManager_StaticPropertyChanged;
    }

    private static void ImageModelsManager_StaticPropertyChanged(object sender, PropertyChangedEventArgs e) {
      // FIXME: weird coding...
      if (ImageModelsManager.IsModelsAvailable) {
        TagModels = GetTagModelsSorted();
        PersonTagModels = GetPersonTagModelsSorted();
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
      => new(ImageModelsManager.Models
        .SelectMany(model => model.Tags)
        .Distinct()
        .OrderBy(tag => tag)
        .Select(tag => new TagModel(tag)));

    public static ObservableCollection<TagModel> GetPersonTagModelsSorted()
      => new(ImageModelsManager.Models
        .SelectMany(model => model.PersonTags)
        .Distinct()
        .OrderBy(tag => tag)
        .Select(tag => new TagModel(tag)));

    public static ObservableSet<string> GetSelectedTags(ObservableSet<TagModel> models)
      => new(models.Where(model => model.IsSelected).Select(model => model.Name));

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "") {
      StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    }
  }
}
