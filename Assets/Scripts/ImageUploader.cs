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
//     public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload"; 

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
//     public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";

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





// UPLOAD SA WEB (LATEST CODE)
// using System.Collections;
// using System.IO;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using SFB;

// public class ImageUploader : MonoBehaviour
// {
//     public Button UploadButton;
//     public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";
//     // public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";

//     public GameObject UploadPanel;
//     public GameObject BillPanel;

//     public BillManager billManager;
//     public Analytics analytics;

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

//    IEnumerator UploadImage(string filePath)
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
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 string responseText = request.downloadHandler.text;
//                 Debug.Log("Upload successful: " + responseText);

//                 UploadPanel.SetActive(false);
//                 BillPanel.SetActive(true);

//                 if (responseText.Contains("Duplicate bill detected"))
//                 {
//                     Debug.LogWarning("Duplicate bill detected! " + responseText);
//                 }
//                 else
//                 {
//                     StartCoroutine(billManager.FetchBills());
//                 }

//                 if (analytics != null)
//                 {
//                     StartCoroutine(analytics.FetchLatestBill());
//                     StartCoroutine(analytics.FetchMonthlyConsumption());
//                     StartCoroutine(analytics.FetchMonthlyCost());
//                     StartCoroutine(analytics.FetchPredictedMonthlyConsumption());
//                     StartCoroutine(analytics.FetchPredictedMonthlyCost());

//                 }
//                 else
//                 {
//                     Debug.LogError("Analytics script reference is missing.");
//                 }

//                 WaterSavingTipsManager fetchTips = FindObjectOfType<WaterSavingTipsManager>();
//                 if (fetchTips != null)
//                 {
//                     StartCoroutine(fetchTips.FetchWaterSavingTips()); 
//                 }

//                 SaveWaterBill saveWaterBiill = FindObjectOfType<SaveWaterBill>();
//                 if (saveWaterBiill != null)
//                 {
//                     StartCoroutine(saveWaterBiill.SavePredictedVsActual()); 
//                     StartCoroutine(saveWaterBiill.FetchMonthlySavedCost()); 
//                 }
//             }
//             else
//             {
//                 Debug.LogError("Upload failed: " + request.error);
//             }
//         }
//     }



// }




using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using TMPro;

[Serializable]
public class ErrorResponse
{
    public string error;
    public string message;
    public string details;
}

public class ImageUploader : MonoBehaviour
{
    public Button UploadButton;
    public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";
    //  public string uploadURL = "http://localhost:5000/api/waterBill/upload";
    public GameObject UploadPanel;
    public GameObject AnalyticsPanel1;
    public GameObject ErrorMessagePanel;  // Add this in Unity
    public TMP_Text ErrorMessageText;         // Add this in Unity

    public BillManager billManager;
    public Analytics analytics;
    public GameObject LoadingScreen;

    void Start()
    {
        UploadButton.onClick.AddListener(PickImageFromGallery);
        
        // Make sure error panel is initially hidden
        if (ErrorMessagePanel != null)
            ErrorMessagePanel.SetActive(false);
    }

    void PickImageFromGallery()
    {
        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

        if (permission == NativeGallery.Permission.ShouldAsk)
        {
            permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
        }

        if (permission == NativeGallery.Permission.Granted)
        {
            NativeGallery.GetImageFromGallery((path) =>
            {
                if (!string.IsNullOrEmpty(path))
                {
                    StartCoroutine(UploadImage(path));
                }
            }, "Select an image");
        }
        else
        {
            Debug.LogError("Permission denied.");
            ShowErrorMessage("Permission denied. Please allow access to your gallery.");
        }
    }

    IEnumerator UploadImage(string filePath)
    {
        string jwtToken = PlayerPrefs.GetString("jwtToken", "");
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is missing. Please log in.");
            ShowErrorMessage("Authentication error. Please log in again.");
            yield break;
        }

        byte[] imageData = File.ReadAllBytes(filePath);
        string fileName = Path.GetFileName(filePath);
        WWWForm form = new WWWForm();
        form.AddBinaryData("imageUrl", imageData, fileName, "image/png");

        using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            LoadingScreen.SetActive(true);
            yield return request.SendWebRequest();

            LoadingScreen.SetActive(false);

            // Log complete response for debugging
            Debug.Log("Server response: " + request.responseCode + " - " + request.downloadHandler.text);

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Upload response: " + responseText);
                
                // Handle duplicate bill warning
                if (responseText.Contains("Duplicate bill detected"))
                {
                    Debug.LogWarning("Duplicate bill detected! " + responseText);
                    ShowErrorMessage("Duplicate bill detected. This bill has already been uploaded.");
                }
                else
                {
                    // Successful upload with no warnings
                    UploadPanel.SetActive(false);
                    AnalyticsPanel1.SetActive(true);
                    
                    StartCoroutine(billManager.FetchBills());

                    if (analytics != null)
                    {
                        StartCoroutine(analytics.FetchLatestBill());
                        StartCoroutine(analytics.FetchMonthlyConsumption());
                        StartCoroutine(analytics.FetchMonthlyCost());
                        StartCoroutine(analytics.FetchPredictedMonthlyConsumption());
                        StartCoroutine(analytics.FetchPredictedMonthlyCost());
                    }
                    else
                    {
                        Debug.LogError("Analytics script reference is missing.");
                    }

                    WaterSavingTipsManager fetchTips = FindObjectOfType<WaterSavingTipsManager>();
                    if (fetchTips != null)
                    {
                        StartCoroutine(fetchTips.FetchWaterSavingTips());
                    }

                    SaveWaterBill saveWaterBill = FindObjectOfType<SaveWaterBill>();
                    if (saveWaterBill != null)
                    {
                        StartCoroutine(saveWaterBill.SavePredictedVsActual());
                        StartCoroutine(saveWaterBill.FetchMonthlySavedCost());
                    }
                }
            }
            else
            {
                // Default error message
                string errorMessage = "Upload failed. ";
                
                // Handle 400 Bad Request specifically
                if (request.responseCode == 400)
                {
                    // Try to parse the error message from the server
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        // First try JSON parsing
                        try
                        {
                            ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                            
                            if (!string.IsNullOrEmpty(errorResponse.message))
                            {
                                errorMessage = errorResponse.message;
                            }
                            else if (!string.IsNullOrEmpty(errorResponse.error))
                            {
                                errorMessage = errorResponse.error;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Error parsing JSON response: " + e.Message);
                            
                            // Fallback to simple string checking if JSON parse fails
                            string responseText = request.downloadHandler.text;
                            
                            if (responseText.Contains("Invalid water bill"))
                            {
                                errorMessage = "Invalid water bill. Please ensure your bill has clear information for amount, consumption, and date.";
                            }
                            else if (responseText.Contains("zero"))
                            {
                                errorMessage = "Your water bill could not be processed. The system detected zero values for important information.";
                            }
                        }
                    }
                    
                    // Show general message for invalid bill if nothing specific found
                    if (errorMessage == "Upload failed. ")
                    {
                        errorMessage = "Invalid water bill. Please upload a clear image of a valid water bill.";
                    }
                }
                else
                {
                    // For other error codes
                    errorMessage += "Please try again later. Error: " + request.error;
                }
                
                Debug.LogError("Upload failed: " + request.error + " - " + errorMessage);
                ShowErrorMessage(errorMessage);
            }
        }
    }
    
    void ShowErrorMessage(string message)
    {
        if (ErrorMessagePanel != null && ErrorMessageText != null)
        {
            ErrorMessageText.text = message;
            ErrorMessagePanel.SetActive(true);
            
            // Optional: Auto-hide after a few seconds
            StartCoroutine(HideErrorMessageAfterDelay(5f));
        }
        else
        {
            Debug.LogError("Error UI elements not assigned. Error message: " + message);
        }
    }
    
    IEnumerator HideErrorMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (ErrorMessagePanel != null)
            ErrorMessagePanel.SetActive(false);
    }
}




// MAKE SURE TO ATTACH LOADING SCREEN IN THE INSPECTOR BEFORE UNCOMMENTING THIS ======================================
// MOBILE UPLOAD (WORKING)


// using System.Collections;
// using System.IO;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;

// public class ImageUploader : MonoBehaviour
// {
//     public Button UploadButton;
//     public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";
//     // private string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";
//     public GameObject UploadPanel;
//     public GameObject AnalyticsPanel1;

//     public BillManager billManager;
//     public Analytics analytics;
//     public GameObject LoadingScreen;

//     void Start()
//     {
//         UploadButton.onClick.AddListener(PickImageFromGallery);
//     }

//     void PickImageFromGallery()
//     {
//         NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

//         if (permission == NativeGallery.Permission.ShouldAsk)
//         {
//             permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
//         }

//         if (permission == NativeGallery.Permission.Granted)
//         {
//             NativeGallery.GetImageFromGallery((path) =>
//             {
//                 if (!string.IsNullOrEmpty(path))
//                 {
//                     StartCoroutine(UploadImage(path));
//                 }
//             }, "Select an image");
//         }
//         else
//         {
//             Debug.LogError("Permission denied.");
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
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
//             LoadingScreen.SetActive(true);
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 // 
//                 string responseText = request.downloadHandler.text;
//                 Debug.Log("Upload successful: " + responseText);
//                 LoadingScreen.SetActive(false);
//                 UploadPanel.SetActive(false);
//                 AnalyticsPanel1.SetActive(true);

//                 if (responseText.Contains("Duplicate bill detected"))
//                 {
//                     Debug.LogWarning("Duplicate bill detected! " + responseText);
//                 }
//                 else
//                 {
//                     StartCoroutine(billManager.FetchBills());
//                 }

//                 if (analytics != null)
//                 {
//                     StartCoroutine(analytics.FetchLatestBill());
//                     StartCoroutine(analytics.FetchMonthlyConsumption());
//                     StartCoroutine(analytics.FetchMonthlyCost());
//                     StartCoroutine(analytics.FetchPredictedMonthlyConsumption());
//                     StartCoroutine(analytics.FetchPredictedMonthlyCost());
//                 }
//                 else
//                 {
//                     Debug.LogError("Analytics script reference is missing.");
//                 }

//                 WaterSavingTipsManager fetchTips = FindObjectOfType<WaterSavingTipsManager>();
//                 if (fetchTips != null)
//                 {
//                     StartCoroutine(fetchTips.FetchWaterSavingTips());
//                 }

//                 SaveWaterBill saveWaterBiill = FindObjectOfType<SaveWaterBill>();
//                 if (saveWaterBiill != null)
//                 {
//                     StartCoroutine(saveWaterBiill.SavePredictedVsActual());
//                     StartCoroutine(saveWaterBiill.FetchMonthlySavedCost());
//                 }
//             }
//             else
//             {
//                 Debug.LogError("Upload failed: " + request.error);
//                 LoadingScreen.SetActive(false);
//             }
//         }
//     }
// }



// MOBILE MULTIPLE IMAGE UPLOAD
// using System.Collections;
// using System.IO;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using System.Collections.Generic;

// public class ImageUploader : MonoBehaviour
// {
//     public Button UploadButton;
//     // public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";
//     public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";
//     public GameObject UploadPanel;
//     public GameObject AnalyticsPanel1;

//     public BillManager billManager;
//     public Analytics analytics;
//     public GameObject BillPanel;

//     void Start()
//     {
//         UploadButton.onClick.AddListener(PickImagesFromGallery);
//     }

//     void PickImagesFromGallery()
//     {
//         NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

//         if (permission == NativeGallery.Permission.ShouldAsk)
//         {
//             permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
//         }

//         if (permission == NativeGallery.Permission.Granted)
//         {
//             NativeGallery.GetImagesFromGallery((paths) =>
//             {
//                 if (paths != null && paths.Length > 0)
//                 {
//                     foreach (string path in paths)
//                     {
//                         if (!string.IsNullOrEmpty(path))
//                         {
//                             StartCoroutine(UploadImage(path));
//                         }
//                     }
//                 }
//             }, "Select images");
//         }
//         else
//         {
//             Debug.LogError("Permission denied.");
//         }
//     }

//    IEnumerator UploadImage(string filePath)
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
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 string responseText = request.downloadHandler.text;
//                 Debug.Log("Upload successful: " + responseText);

//                 UploadPanel.SetActive(false);
//                 BillPanel.SetActive(true);

//                 if (responseText.Contains("Duplicate bill detected"))
//                 {
//                     Debug.LogWarning("Duplicate bill detected! " + responseText);
//                 }
//                 else
//                 {
//                     StartCoroutine(billManager.FetchBills());
//                 }

//                 if (analytics != null)
//                 {
//                     StartCoroutine(analytics.FetchLatestBill());
//                     StartCoroutine(analytics.FetchMonthlyConsumption());
//                     StartCoroutine(analytics.FetchMonthlyCost());
//                 }
//                 else
//                 {
//                     Debug.LogError("Analytics script reference is missing.");
//                 }

//                 WaterSavingTipsManager fetchTips = FindObjectOfType<WaterSavingTipsManager>();
//                 if (fetchTips != null)
//                 {
//                     StartCoroutine(fetchTips.FetchWaterSavingTips()); 
//                 }
//             }
//             else
//             {
//                 Debug.LogError("Upload failed: " + request.error);
//             }
//         }
//     }
// }




// using System.Collections;
// using System.IO;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Networking;
// using System.Collections.Generic;

// public class ImageUploader : MonoBehaviour
// {
//     public Button UploadButton;
//     public string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload";

//     public GameObject UploadPanel;
//     public GameObject AnalyticsPanel1;

//     public BillManager billManager;
//     public Analytics analytics;

//     void Start()
//     {
//         UploadButton.onClick.AddListener(PickImagesFromGallery);
//     }

//     void PickImagesFromGallery()
//     {
//         NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

//         if (permission == NativeGallery.Permission.ShouldAsk)
//         {
//             permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
//         }

//         if (permission == NativeGallery.Permission.Granted)
//         {
//             NativeGallery.GetImagesFromGallery((paths) =>
//             {
//                 if (paths != null && paths.Length > 0)
//                 {
//                     foreach (string path in paths)
//                     {
//                         if (!string.IsNullOrEmpty(path))
//                         {
//                             StartCoroutine(UploadImage(path));
//                         }
//                     }
//                 }
//             }, "Select images");
//         }
//         else
//         {
//             Debug.LogError("Permission denied.");
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
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 string responseText = request.downloadHandler.text;
//                 Debug.Log("Upload successful: " + responseText);

//                 UploadPanel.SetActive(false);
//                 AnalyticsPanel1.SetActive(true);

//                 if (responseText.Contains("Duplicate bill detected"))
//                 {
//                     Debug.LogWarning("Duplicate bill detected! " + responseText);
//                 }
//                 else
//                 {
//                     StartCoroutine(billManager.FetchBills());
//                 }

//                 if (analytics != null)
//                 {
//                     StartCoroutine(analytics.FetchLatestBill());
//                     StartCoroutine(analytics.FetchMonthlyConsumption());
//                     StartCoroutine(analytics.FetchMonthlyCost());
//                 }
//                 else
//                 {
//                     Debug.LogError("Analytics script reference is missing.");
//                 }
//             }
//             else
//             {
//                 Debug.LogError("Upload failed: " + request.error);
//             }
//         }
//     }
// }