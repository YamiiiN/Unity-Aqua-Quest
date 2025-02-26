// WORKING PERO WALANG USER 
// using System.Collections;
// using System.IO;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using SFB; // Standalone File Browser for PC/Mac

// public class ImageUploader : MonoBehaviour
// {
//     public Button UploadButton;
//     public string uploadURL = "http://localhost:5000/api/waterBill/upload"; 

//     void Start()
//     {
//         UploadButton.onClick.AddListener(OpenFileDialog);
//     }

//     void OpenFileDialog()
//     {
//         var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
//         string[] paths = StandaloneFileBrowser.OpenFilePanel("Select an Image", "", extensions, false);

//         if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
//         {
//             StartCoroutine(UploadImage(paths[0]));
//         }
//     }

//     IEnumerator UploadImage(string filePath)
//     {
//         byte[] imageData = File.ReadAllBytes(filePath);
//         string fileName = Path.GetFileName(filePath);

//         WWWForm form = new WWWForm();
//         form.AddBinaryData("imageUrl", imageData, fileName, "image/png"); 

//         using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
//         {
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("Upload successful: " + request.downloadHandler.text);
//             }
//             else
//             {
//                 Debug.LogError("Upload failed: " + request.error);
//             }
//         }
//     }
// }





// WORKING WITH LOGIN USER AND MULTIPLE IMAGE UPLOAD NA MAY KONTING BUG SA MULTIPLE UPLOAD
// using System.Collections;
// using System.IO;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using SFB; 

// public class ImageUploader : MonoBehaviour
// {
//     public Button UploadButton;
//     public string uploadURL = "http://localhost:5000/api/waterBill/upload";

//     void Start()
//     {
//         UploadButton.onClick.AddListener(OpenFileDialog);
//     }

//     void OpenFileDialog()
//     {
//         var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
//         string[] paths = StandaloneFileBrowser.OpenFilePanel("Select an Images", "", extensions, true);

//         if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
//         {
//             StartCoroutine(UploadImage(paths[0]));
//         }
//     }

//     IEnumerator UploadImage(string filePath)
//     {
//         string jwtToken = PlayerPrefs.GetString("jwtToken", "");

//         if (string.IsNullOrEmpty(jwtToken))
//         {
//             Debug.LogError("JWT token is missing. Please log in.");
//             yield break;
//         }

//         byte[] imageData = File.ReadAllBytes(filePath);
//         string fileName = Path.GetFileName(filePath);

//         WWWForm form = new WWWForm();
//         form.AddBinaryData("imageUrl", imageData, fileName, "image/png");

//         using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
//         {
//             // Add JWT token to request header
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

//             // Send request
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("Upload successful: " + request.downloadHandler.text);
//             }
//             else
//             {
//                 Debug.LogError("Upload failed: " + request.error);
//             }
//         }
//     }

// }





// WORKING REFRESH KAPAG MAY BAGONG UPLOAD NA WATER BILL (LATEST CODE)
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SFB;

public class ImageUploader : MonoBehaviour
{
    public Button UploadButton;
    public string uploadURL = "http://localhost:5000/api/waterBill/upload";

    public GameObject UploadPanel;
    public GameObject AnalyticsPanel1;

    public BillManager billManager;
    public Analytics analytics;

    void Start()
    {
        UploadButton.onClick.AddListener(OpenFileDialog);
    }

    void OpenFileDialog()
    {
        var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select an Images", "", extensions, true);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            StartCoroutine(UploadImage(paths[0]));
        }
    }

    // OG CODE (lastest na ginagamit ko )
    // IEnumerator UploadImage(string filePath)
    // {
    //     string jwtToken = PlayerPrefs.GetString("jwtToken", "");
    //     if (string.IsNullOrEmpty(jwtToken))
    //     {
    //         Debug.LogError("JWT token is missing. Please log in.");
    //         yield break;
    //     }

    //     byte[] imageData = File.ReadAllBytes(filePath);
    //     string fileName = Path.GetFileName(filePath);
    //     WWWForm form = new WWWForm();
    //     form.AddBinaryData("imageUrl", imageData, fileName, "image/png");

    //     using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
    //     {

    //         request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

    //         yield return request.SendWebRequest();

    //         if (request.result == UnityWebRequest.Result.Success)
    //         {
    //             Debug.Log("Upload successful: " + request.downloadHandler.text);
                
    //             StartCoroutine(billManager.FetchBills()); 
                
    //         }
    //         else
    //         {
    //             Debug.LogError("Upload failed: " + request.error);
    //         }
    //     }
    // }

    // MAGDDUPLICATE KAPAG SAME DATEBILL (WORKING NA)
   IEnumerator UploadImage(string filePath)
    {
        string jwtToken = PlayerPrefs.GetString("jwtToken", "");
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is missing. Please log in.");
            yield break;
        }

        byte[] imageData = File.ReadAllBytes(filePath);
        string fileName = Path.GetFileName(filePath);
        WWWForm form = new WWWForm();
        form.AddBinaryData("imageUrl", imageData, fileName, "image/png");

        using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Upload successful: " + responseText);

                UploadPanel.SetActive(false);
                AnalyticsPanel1.SetActive(true);

                if (responseText.Contains("Duplicate bill detected"))
                {
                    Debug.LogWarning("Duplicate bill detected! " + responseText);
                }
                else
                {
                    StartCoroutine(billManager.FetchBills());
                }

                if (analytics != null)
                {
                    StartCoroutine(analytics.FetchLatestBill());
                    StartCoroutine(analytics.FetchMonthlyConsumption());
                    StartCoroutine(analytics.FetchMonthlyCost());
                }
                else
                {
                    Debug.LogError("Analytics script reference is missing.");
                }
            }
            else
            {
                Debug.LogError("Upload failed: " + request.error);
            }
        }
    }


}




// UPLOAD WATER BILL SA ANDROID (TRIAL)
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using System.IO;
// using System.Threading.Tasks;
// using UnityEngine.Networking; // Required for UnityWebRequest
// // No need for NativeGalleryNamespace; NativeGallery is in default namespace

// public class FileUploader : MonoBehaviour
// {
//     private string uploadURL = "http://localhost:5000/api/waterBill/upload"; // Change this to your API endpoint

//     public void PickAndUploadFile()
//     {
//         StartCoroutine(PickFile());
//     }

//     private IEnumerator PickFile()
//     {
//         if (!NativeGallery.IsMediaPickerBusy())
//         {
//             // 🔄 FIX: Replace GetPermission with CheckPermission
//             NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

//             if (permission == NativeGallery.Permission.ShouldAsk)
//             {
//                 permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

//             }

//             if (permission == NativeGallery.Permission.Granted)
//             {
//                 // 🔄 FIX: Replace GetMediaFromGallery with GetImageFromGallery or GetVideoFromGallery
//                 NativeGallery.GetImageFromGallery((path) =>
//                 {
//                     if (!string.IsNullOrEmpty(path))
//                     {
//                         StartCoroutine(UploadFile(path));
//                     }
//                 }, "Select an image");

//                 // If selecting videos, use:
//                 // NativeGallery.GetVideoFromGallery((path) => { /* Handle video */ }, "Select a video");
//             }
//             else
//             {
//                 Debug.LogError("Permission denied.");
//             }
//         }
//         yield return null;
//     }

//     private IEnumerator UploadFile(string filePath)
//     {
//         byte[] fileData = File.ReadAllBytes(filePath);
//         string fileName = Path.GetFileName(filePath);

//         WWWForm form = new WWWForm();
//         form.AddBinaryData("file", fileData, fileName);

//         using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
//         {
//             request.SetRequestHeader("Authorization", "Bearer YOUR_ACCESS_TOKEN"); // Optional authentication

//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("File uploaded successfully: " + request.downloadHandler.text);
//             }
//             else
//             {
//                 Debug.LogError("Upload failed: " + request.error);
//             }
//         }
//     }
// }
