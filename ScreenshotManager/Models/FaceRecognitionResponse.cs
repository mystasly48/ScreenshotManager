using Newtonsoft.Json;

namespace ScreenshotManager.Models {
  public class FaceRecognitionResponse {
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("confidence")]
    public double Confidence { get; set; }
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
