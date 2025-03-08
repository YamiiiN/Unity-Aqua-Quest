using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class PDFDownloader : MonoBehaviour
{
    // private string pdfUrl = "http://localhost:5000/api/pdf/download"; // ✅ Use your backend API URL
private string pdfUrl = "https://aqua-quest-backend-deployment.onrender.com/api/pdf/download"; // ✅ Use your backend API URL
public GameObject LoadingScreen;
    public void OnDownloadPDFButtonClick()
    {
        StartCoroutine(DownloadPDF());
    }

    IEnumerator DownloadPDF()
    {
        string token = PlayerPrefs.GetString("jwtToken", "");
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No token found, user needs to log in.");
            yield break;
        }

        string filePath;

        // ✅ Detect platform and set the correct Downloads folder
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Path.Combine("/storage/emulated/0/Downloads", "User_Report.pdf"); // Android Download folder
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads", "User_Report.pdf"); // Windows Download folder
        }
        else
        {
            filePath = Path.Combine(Application.persistentDataPath, "User_Report.pdf"); // Fallback
        }

        UnityWebRequest request = UnityWebRequest.Get(pdfUrl);
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.downloadHandler = new DownloadHandlerFile(filePath);
        LoadingScreen.SetActive(true);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LoadingScreen.SetActive(false);
            Debug.Log("PDF downloaded successfully: " + filePath);
            Application.OpenURL(filePath); // Open the downloaded PDF
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
}
