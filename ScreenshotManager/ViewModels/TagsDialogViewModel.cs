using ScreenshotManager.Models;
using ScreenshotManager.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ScreenshotManager.ViewModels {
  public class TagsDialogViewModel : Observable {
    public ObservableCollection<string> Tags { get; set; }
    private string _tagName;
    public string TagName {
      get => _tagName;
      set => SetProperty(ref _tagName, value);
    }
    public ICommand AddTagCommand => new AnotherCommandImplementation((obj) => ExecuteAddTag(obj));

    public TagsDialogViewModel(ImageModel model) {
      Tags = new ObservableCollection<string>(model.Tags);
    }

    private void ExecuteAddTag(object obj) {
      if (!string.IsNullOrEmpty(TagName)) {
        Tags.Add(TagName);
      }
      TagName = "";
    }
  }
}
