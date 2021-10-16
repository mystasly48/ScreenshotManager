using ScreenshotManager.Models;
using ScreenshotManager.ViewModels;

namespace ScreenshotManager.Views {
  public partial class TagsDialog {
    private TagsDialogViewModel _viewModel;
    public TagsDialog(ImageModel model) {
      InitializeComponent();
      _viewModel = new TagsDialogViewModel(model);
      this.DataContext = _viewModel;
    }

    private void CheckComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      _viewModel.UpdateTags();
    }
  }
}
