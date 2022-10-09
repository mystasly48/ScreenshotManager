using ScreenshotManager.Models;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace ScreenshotManager.Utils {
  public static class Screenshot {
    public static int PrimaryScreenX => Screen.PrimaryScreen.Bounds.X;
    public static int PrimaryScreenY => Screen.PrimaryScreen.Bounds.Y;
    public static int PrimaryScreenWidth => Screen.PrimaryScreen.Bounds.Width;
    public static int PrimaryScreenHeight => Screen.PrimaryScreen.Bounds.Height;

    public static int FullscreenX => (int)SystemParameters.VirtualScreenLeft;
    public static int FullscreenY => (int)SystemParameters.VirtualScreenTop;
    public static int FullscreenWidth => (int)SystemParameters.VirtualScreenWidth;
    public static int FullscreenHeight => (int)SystemParameters.VirtualScreenHeight;

    public static Bitmap Take(int x, int y, int width, int height) {
      Bitmap bmp = new Bitmap(width, height);
      using (Graphics g = Graphics.FromImage(bmp)) {
        g.CopyFromScreen(x, y, 0, 0, bmp.Size);
      }
      return bmp;
    }

    public static Bitmap Take(Rectangle rectangle) {
      return Take(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }

    public static Bitmap Take(Screen screen) {
      return Take(screen.Bounds);
    }

    public static Bitmap Take(ScreenModel screen) {
      return Take(screen.X, screen.Y, screen.Width, screen.Height);
    }

    public static Bitmap TakePrimary() {
      return Take(PrimaryScreenX, PrimaryScreenY, PrimaryScreenWidth, PrimaryScreenHeight);
    }

    public static Bitmap TakeFull() {
      return Take(FullscreenX, FullscreenY, FullscreenWidth, FullscreenHeight);
    }

    public static string CreateFilename() {
      return CreateFilename(DateTime.Now);
    }

    public static string CreateFilename(DateTime dt) {
      return $"{dt:yyyy-MM-dd_HH-mm-ss_ffff}.png";
    }
  }
}
