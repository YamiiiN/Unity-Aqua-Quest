using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json.Linq;
public class PDFDownloader : MonoBehaviour
{
    // private string pdfUrl = "https://aqua-quest-backend-deployment.onrender.com/api/pdf/download"; // ✅ Use your backend API URL
private string pdfUrl = "https://aqua-quest-backend-deployment.onrender.com/api/pdf/download"; // ✅ Use your backend API URL
public GameObject LoadingScreen;
private string userDataPath;
    void Start()
    {
          userDataPath = Path.Combine(UnityEngine.Application.persistentDataPath, "userInfo.json");
    }
    public void OnDownloadPDFButtonClick()
    {
        StartCoroutine(DownloadPDF());
    }
    
    IEnumerator DownloadPDF()
    {
        string jsonContent = File.ReadAllText(userDataPath);
        JObject playerData = JObject.Parse(jsonContent);
        string token = playerData["token"]?.ToString() ?? "";
        string userId = playerData["userId"]?.ToString() ?? "";
        string pdfFile = Path.Combine(Application.persistentDataPath, $"User_Report_{userId}.pdf");
            // Extract values safely
            
          
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No token found, user needs to log in.");
            yield break;
        }


        UnityWebRequest request = UnityWebRequest.Get(pdfUrl);
        request.SetRequestHeader("Authorization", "Bearer " + token);
        LoadingScreen.SetActive(true);
        request.downloadHandler = new DownloadHandlerFile(pdfFile);
        
        // string filePath = Path.Combine(Application.persistentDataPath, $"User_Report_{userId}.pdf");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LoadingScreen.SetActive(false);
            if (Application.platform == RuntimePlatform.Android)
            {
                OpenPDFAndroid(pdfFile);
            }
            else
            {
                Application.OpenURL(pdfFile);
            }
            // Debug.Log("PDF downloaded successfully: " + filePath);
            // Application.OpenURL(filePath); // Open the downloaded PDF
        }
        else
        {
            LoadingScreen.SetActive(false);
            Debug.LogError($"Error downloading PDF: {request.error}");
        }
    }

    // IEnumerator DownloadPDF()
    // {
    //     string token = PlayerPrefs.GetString("jwtToken", "");

    //     if (string.IsNullOrEmpty(token))
    //     {
    //         Debug.LogError("No token found, user needs to log in.");
    //         yield break;
    //     }

    //     string filePath = Path.Combine(Application.persistentDataPath, "User_Report.pdf");

    //     if (File.Exists(filePath))
    //     {
    //         Debug.Log("Deleting existing PDF file...");
    //         File.Delete(filePath);
    //     }

    //     UnityWebRequest request = UnityWebRequest.Get(pdfUrl);
    //     request.SetRequestHeader("Authorization", "Bearer " + token); // ✅ Include JWT token
    //     request.downloadHandler = new DownloadHandlerFile(filePath); // ✅ Save file directly

    //     yield return request.SendWebRequest();

    //     if (request.result == UnityWebRequest.Result.Success)
    //     {
    //         Debug.Log("PDF downloaded successfully: " + filePath);
    //         Application.OpenURL(filePath); // ✅ Open the downloaded PDF
    //     }
    //     else
    //     {
    //         Debug.LogError($"Error downloading PDF: {request.error}");
    //     }
    // }

    void OpenPDFAndroid(string filePath)
    {
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
        intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));

        AndroidJavaObject file = new AndroidJavaObject("java.io.File", filePath);
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("fromFile", file);

        intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/pdf");
        intent.Call<AndroidJavaObject>("addFlags", 1); // FLAG_GRANT_READ_URI_PERMISSION

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("startActivity", intent);
    }

}
