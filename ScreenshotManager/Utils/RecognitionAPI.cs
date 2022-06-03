using Newtonsoft.Json;
using ScreenshotManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScreenshotManager.Utils {
  public static class RecognitionAPI {
    public static async Task<List<FaceRecognitionResponse>> FaceRecognitionAsync(string filepath) {
      string endpoint = "http://localhost:8080/api/face_recognition";
      var response_json = await PostImageForRecognitionAsync(filepath, endpoint);
      Debug.WriteLine("Face Recognition Results" + Environment.NewLine + response_json);
      var response = JsonConvert.DeserializeObject<List<FaceRecognitionResponse>>(response_json);
      return response;
    }

    public static async Task<List<TextRecognitionResponse>> TextRecognitionAsync(string filepath) {
      string endpoint = "http://localhost:8080/api/text_recognition";
      var response_json = await PostImageForRecognitionAsync(filepath, endpoint);
      Debug.WriteLine("Text Recognition Results" + Environment.NewLine + response_json);
      var response = JsonConvert.DeserializeObject<List<TextRecognitionResponse>>(response_json);
      return response;
    }

    private static async Task<string> PostImageForRecognitionAsync(string filepath, string endpoint) {
      var wc = new WebClient();
      var response = await wc.UploadFileTaskAsync(endpoint, filepath);
      string response_json = Encoding.UTF8.GetString(response);
      return response_json;
    }
  }
}
