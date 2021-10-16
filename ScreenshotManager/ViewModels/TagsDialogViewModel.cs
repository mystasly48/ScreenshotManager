using ScreenshotManager.Models;
using ScreenshotManager.Utils;
using System.Linq;
using System.Windows.Input;

namespace ScreenshotManager.ViewModels {
  public class TagsDialogViewModel : Observable {
    private ObservableSet<TagModel> _tags;
    public ObservableSet<TagModel> Tags {
      get => _tags;
      set => SetProperty(ref _tags, value);
    }
    private string _tagName;
    public string TagName {
      get => _tagName;
      set => SetProperty(ref _tagName, value);
    }
    public ICommand AddTagCommand => new AnotherCommandImplementation((obj) => ExecuteAddTag(obj));

    private ImageModel _imageModel;

    public TagsDialogViewModel(ImageModel model) {
      _imageModel = model;
      this.Tags = TagModel.GetTagModels(model.Tags);
      this.Tags.Add(TagModel.DummyTagModel);
    }

    public void UpdateTags() {
      _imageModel.Tags = TagModel.GetSelectedTags(Tags);
      ImageModelsManager.Save();
    }

    private void ExecuteAddTag(object obj) {
      if (obj is string tagName && !string.IsNullOrEmpty(tagName)) {
        var matchedTag = Tags.FirstOrDefault(model => model.Name == tagName);
        if (matchedTag != null) {
          matchedTag.IsSelected = true;
        } else {
          // FIXME: CheckComboBox isn't refreshed until I click
          Tags.Add(new TagModel(tagName, true));
        }
      }
      TagName = "";
    }
  }
}
