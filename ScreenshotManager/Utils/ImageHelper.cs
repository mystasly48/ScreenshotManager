using ScreenshotManager.Properties;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScreenshotManager.Utils {
  public static class ImageHelper {
    public static readonly int THUMBNAIL_WIDTH = 320;
    public static readonly int THUMBNAIL_HEIGHT = 180;
    // TODO: ImageControl 側で適当なテキストを表示すればいいから不要かもしれない
    public static ImageSource DefaultThumbnail { get; }

    static ImageHelper() {
      DefaultThumbnail = ResizeToThumbnail(Resources.NoImage);
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
    /// 非同期で画像のファイルパスから BitmapImage を読み込む
    /// </summary>
    /// <param name="url">画像のファイルパス</param>
    /// <returns>Task<BitmapImage></BitmapImage></returns>
    public static Task<BitmapImage> LoadBitmapImageAsync(string url) {
      return Task.Run(() => LoadBitmapImage(url));
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
    /// 規定のサイズに拡大/縮小したサムネイル用画像を読み込む
    /// </summary>
    /// <param name="url">画像のファイルパス</param>
    /// <returns>サムネイル画像</returns>
    public static BitmapImage LoadThumbnail(string url) {
      return LoadThumbnail(url, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT);
    }

    /// <summary>
    /// 指定のサイズに拡大/縮小したサムネイル用画像を読み込む
    /// </summary>
    /// <param name="url">画像のファイルパス</param>
    /// <param name="maxWidth">横幅上限</param>
    /// <param name="maxHeight">縦幅上限</param>
    /// <returns>サムネイル画像</returns>
    public static BitmapImage LoadThumbnail(string url, int maxWidth, int maxHeight) {
      var bitmap = LoadBitmap(url);
      var bitmapImage = ResizeToThumbnail(bitmap, maxWidth, maxHeight);
      return bitmapImage;
    }

    /// <summary>
    /// サムネイル用画像として規定のサイズに拡大/縮小する
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <returns>サムネイル画像</returns>
    public static BitmapImage ResizeToThumbnail(Bitmap bitmap) {
      return ResizeToThumbnail(bitmap, THUMBNAIL_WIDTH, THUMBNAIL_HEIGHT);
    }

    /// <summary>
    /// サムネイル用画像として指定のサイズに拡大/縮小する
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <param name="maxWidth">横幅上限</param>
    /// <param name="maxHeight">縦幅上限</param>
    /// <returns>サムネイル画像</returns>
    public static BitmapImage ResizeToThumbnail(Bitmap bitmap, int maxWidth, int maxHeight) {
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
