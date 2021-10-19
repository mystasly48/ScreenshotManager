using ScreenshotManager.Utils;
using System.Windows;

namespace ScreenshotManager.Models {
  public class TagModel : Observable {
    private const string DUMMY_TAG_MODEL_NAME
      = "THIS_IS_A_DUMMY_TAG_MODEL_TO_AVOID_AN_ISSUE_THAT_UNSELECTS_ALL_ITEMS_WHEN_ALL_ITEMS_ARE_SELECTED";
    public static TagModel DummyTagModel => new TagModel(DUMMY_TAG_MODEL_NAME);

    private string _name;
    public string Name {
      get => _name;
      set {
        SetProperty(ref _name, value);
      }
    }

    private bool _isSelected;
    public bool IsSelected {
      get => _isSelected;
      set {
        if (value && Name == DUMMY_TAG_MODEL_NAME) {
          return;
        }
        SetProperty(ref _isSelected, value);
      }
    }

    public Visibility Visibility {
      get {
        if (Name == DUMMY_TAG_MODEL_NAME) {
          return Visibility.Collapsed;
        } else {
          return Visibility.Visible;
        }
      }
    }

    public TagModel(string name) {
      this.Name = name;
      this.IsSelected = false;
    }

    public TagModel(string name, bool selected) {
      this.Name = name;
      this.IsSelected = selected;
    }

    public override bool Equals(object obj) {
      if (obj is TagModel model) {
        return model.Name == Name;
      }
      return false;
    }

    public override int GetHashCode() => Name.GetHashCode();
    public static bool operator ==(TagModel a, TagModel b) => Equals(a, b);
    public static bool operator !=(TagModel a, TagModel b) => !Equals(a, b);
    public override string ToString() => $"(Name: {Name}, IsSelected: {IsSelected})";
  }
}
