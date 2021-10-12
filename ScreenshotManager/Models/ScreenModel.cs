using ScreenshotManager.Utils;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ScreenshotManager.Models {
  public class ScreenModel {
    private static ScreenModel[] _allSorted;

    public bool Primary { get; }
    public int Index { get; }
    public string IndexedName => $"Screen #{Index}";
    public string OriginalName { get; }
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public ScreenModel(Screen screen, int index) {
      this.Primary = screen.Primary;
      this.Index = index;
      this.OriginalName = screen.DeviceName;
      this.X = screen.Bounds.X;
      this.Y = screen.Bounds.Y;
      this.Width = screen.Bounds.Width;
      this.Height = screen.Bounds.Height;
    }

    public Bitmap Take() {
      return Screenshot.Take(this);
    }

    public static ScreenModel GetPrimary() {
      return GetPrimary(_allSorted ?? GetAllSorted());
    }

    public static ScreenModel GetPrimary(ScreenModel[] allSorted) {
      return allSorted.FirstOrDefault(x => x.Primary);
    }

    public static ScreenModel[] GetAllSorted() {
      var sortedScreens = Screen.AllScreens.OrderBy(s => s.Bounds.X).ThenBy(s => s.Bounds.Y).ToArray();
      var result = new ScreenModel[sortedScreens.Length];
      for (int i = 0; i < result.Length; i++) {
        result[i] = new ScreenModel(sortedScreens[i], i + 1);
      }
      _allSorted = result;
      return result;
    }
  }
}
