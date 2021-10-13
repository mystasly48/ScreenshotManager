using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ScreenshotManager.Utils {
  public class ObservableSet<T> : ObservableCollection<T> {
    public ObservableSet() : base() { }
    public ObservableSet(HashSet<T> items) : base(items) { }

    protected override void InsertItem(int index, T item) {
      if (!Contains(item)) {
        base.InsertItem(index, item);
      }
    }

    protected override void SetItem(int index, T item) {
      if (!Contains(item)) {
        base.InsertItem(index, item);
      }
    }

    public void AddAll(ICollection<T> items) {
      foreach (var item in items) {
        Add(item);
      }
    }
  }
}
