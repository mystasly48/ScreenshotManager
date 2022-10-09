using System;
using System.Diagnostics;

namespace ScreenshotManager.Utils {
  public static class Logger {
    public static void WriteLine(string message) {
      WriteLine(DateTime.Now, message);
    }

    public static void WriteLine(DateTime dateTime, string message) {
      Debug.WriteLine($"[{dateTime}]: {message}");
    }

    public static void Write(string message) {
      Debug.Write(message);
    }
  }
}
