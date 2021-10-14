using System;
using System.Windows.Forms;

namespace ScreenshotManager.Hotkeys {
  public class KeyPressedEventArgs : EventArgs {
    internal KeyPressedEventArgs(ModifierKeys modifier, Keys key) {
      Modifier = modifier;
      Key = key;
    }

    public ModifierKeys Modifier { private set; get; }
    public Keys Key { private set; get; }
  }
}
