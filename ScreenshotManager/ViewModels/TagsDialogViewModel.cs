using ScreenshotManager.Models;
using ScreenshotManager.Utils;
using System.Windows.Input;

namespace ScreenshotManager.ViewModels {
  public class TagsDialogViewModel : Observable {
    public ObservableSet<string> Tags {
      get => _imageModel.Tags;
      set => _imageModel.Tags = value;
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
    }

    private void ExecuteAddTag(object obj) {
      if (!string.IsNullOrEmpty(TagName)) {
        Tags.Add(TagName);
      }
      TagName = "";
    }
  }
}
