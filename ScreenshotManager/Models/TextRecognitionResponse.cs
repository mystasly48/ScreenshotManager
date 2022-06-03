using Newtonsoft.Json;

namespace ScreenshotManager.Models {
  public class TextRecognitionResponse {
    [JsonProperty("text")]
    public string Text { get; set; }
    [JsonProperty("kana")]
    public string Kana { get; set; }
    [JsonProperty("top")]
    public int Top { get; set; }
    [JsonProperty("left")]
    public int Left { get; set; }
    [JsonProperty("bottom")]
    public int Bottom { get; set; }
    [JsonProperty("right")]
    public int Right { get; set; }
  }
}
