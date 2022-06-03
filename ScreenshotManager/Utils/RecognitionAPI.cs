using Newtonsoft.Json;
using ScreenshotManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace ScreenshotManager.Utils {
  public static class RecognitionAPI {
    public static List<FaceRecognitionResponse> FaceRecognition(string filepath) {
      string endpoint = "http://localhost:8080/api/face_recognition";
      var response_json = PostImageForRecognition(filepath, endpoint);
      Debug.WriteLine("Face Recognition Results" + Environment.NewLine + response_json);
      var response = JsonConvert.DeserializeObject<List<FaceRecognitionResponse>>(response_json);
      return response;
    }

    public static List<TextRecognitionResponse> TextRecognition(string filepath) {
      string endpoint = "http://localhost:8080/api/text_recognition";
      var response_json = PostImageForRecognition(filepath, endpoint);
      Debug.WriteLine("Text Recognition Results" + Environment.NewLine + response_json);
      var response = JsonConvert.DeserializeObject<List<TextRecognitionResponse>>(response_json);
      return response;
    }

    private static string PostImageForRecognition(string filepath, string endpoint) {
      var wc = new WebClient();
      var response = wc.UploadFile(endpoint, filepath);
      string response_json = Encoding.UTF8.GetString(response);
      return response_json;
    }
  }
}
