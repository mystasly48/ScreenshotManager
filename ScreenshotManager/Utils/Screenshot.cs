using ScreenshotManager.Models;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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
      return $"{dt:yyyy-MM-dd_HH-mm-ss_ffff}.jpg";
    }

    /// <summary>
    /// 画像のファイルパスから BitmapImage を読み込む
    /// </summary>
    /// <param name="url">画像のファイルパス</param>
    /// <returns>BitmapImage</returns>
    public static BitmapImage LoadBitmapImage(string url) {
      var bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      bitmapImage.UriSource = new Uri(url);
      bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
      bitmapImage.EndInit();
      bitmapImage.Freeze();
      return bitmapImage;
    }

    /// <summary>
    /// 画像のファイルパスから Bitmap を読み込む
    /// </summary>
    /// <param name="url">画像のファイルパス</param>
    /// <returns>Bitmap</returns>
    public static Bitmap LoadBitmap(string url) {
      var bitmap = new Bitmap(url);
      return bitmap;
    }

    /// <summary>
    /// 指定のサイズの拡大/縮小したサムネイル用画像を読み込む
    /// </summary>
    /// <param name="url">画像のファイルパス</param>
    /// <param name="maxWidth">横幅上限</param>
    /// <param name="maxHeight">縦幅上限</param>
    /// <returns></returns>
    public static BitmapImage LoadThumbnail(string url, int maxWidth, int maxHeight) {
      var bitmap = LoadBitmap(url);
      var resized = ResizeBitmap(bitmap, maxWidth, maxHeight);
      var bitmapImage = BitmapToBitmapImage(resized);
      return bitmapImage;
    }

    /// <summary>
    /// Bitmap を BitmapImage に変換する
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <returns>BitmapImage</returns>
    public static BitmapImage BitmapToBitmapImage(Bitmap bitmap) {
      using (var memory = new MemoryStream()) {
        bitmap.Save(memory, ImageFormat.Jpeg);
        memory.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = memory;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.CreateOptions = BitmapCreateOptions.None;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
      }
    }

    /// <summary>
    /// Bitmap を指定の倍率で拡大/縮小する
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <param name="ratio">倍率</param>
    /// <returns>拡大/縮小した Bitmap</returns>
    public static Bitmap ResizeBitmap(Bitmap bitmap, double ratio) {
      int maxWidth = (int)(bitmap.Width * ratio);
      int maxHeight = (int)(bitmap.Height * ratio);
      return ResizeBitmap(bitmap, maxWidth, maxHeight);
    }

    /// <summary>
    /// Bitmap を指定のサイズを上限として縦横比を維持したまま拡大/縮小する
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <param name="maxWidth">横幅上限</param>
    /// <param name="maxHeight">縦幅上限</param>
    /// <returns>拡大/縮小した Bitmap</returns>
    public static Bitmap ResizeBitmap(Bitmap bitmap, int maxWidth, int maxHeight) {
      // maxHeight は達成できているが、maxWidth より大きい可能性が残っている
      int resizeHeight = maxHeight;
      int resizeWidth = (int)(bitmap.Width * (resizeHeight / (double)bitmap.Height));

      // maxWidth が達成しなかった場合、逆の方法でやる（これで確実に縦横比を維持できる）
      if (maxWidth < resizeWidth) {
        resizeWidth = maxWidth;
        resizeHeight = (int)(bitmap.Height * (resizeWidth / (double)bitmap.Width));
      }

      var resizedBitmap = new Bitmap(resizeWidth, resizeHeight);
      var g = Graphics.FromImage(resizedBitmap);
      g.InterpolationMode = InterpolationMode.HighQualityBicubic;
      g.DrawImage(bitmap, 0, 0, resizeWidth, resizeHeight);
      g.Dispose();

      return resizedBitmap;
    }
  }
}
