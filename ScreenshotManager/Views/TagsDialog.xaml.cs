using ScreenshotManager.Models;
using ScreenshotManager.ViewModels;

namespace ScreenshotManager.Views {
  public partial class TagsDialog {
    public TagsDialog(ImageModel model) {
      InitializeComponent();
      this.DataContext = new TagsDialogViewModel(model);
    }
  }
}
