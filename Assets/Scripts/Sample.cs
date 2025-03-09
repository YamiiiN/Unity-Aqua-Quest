// UPLOAD WATER BILL SA ANDROID (TRIAL)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking; // Required for UnityWebRequest
// No need for NativeGalleryNamespace; NativeGallery is in default namespace

public class FileUploader : MonoBehaviour
{
    // private string uploadURL = "https://aqua-quest-backend-deployment.onrender.com/api/waterBill/upload"; // Change this to your API endpoint
    private string uploadURL = "http://localhost:5000/waterBill/upload";
    public void PickAndUploadFile()
    {
        StartCoroutine(PickFile());
    }

    private IEnumerator PickFile()
    {
        if (!NativeGallery.IsMediaPickerBusy())
        {
            // 🔄 FIX: Replace GetPermission with CheckPermission
            NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

            if (permission == NativeGallery.Permission.ShouldAsk)
            {
                permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

            }

            if (permission == NativeGallery.Permission.Granted)
            {
                // 🔄 FIX: Replace GetMediaFromGallery with GetImageFromGallery or GetVideoFromGallery
                NativeGallery.GetImageFromGallery((path) =>
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        StartCoroutine(UploadFile(path));
                    }
                }, "Select an image");

                // If selecting videos, use:
                // NativeGallery.GetVideoFromGallery((path) => { /* Handle video */ }, "Select a video");
            }
            else
            {
                Debug.LogError("Permission denied.");
            }
        }
        yield return null;
    }

    private IEnumerator UploadFile(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        string fileName = Path.GetFileName(filePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileData, fileName);

        using (UnityWebRequest request = UnityWebRequest.Post(uploadURL, form))
        {
            request.SetRequestHeader("Authorization", "Bearer YOUR_ACCESS_TOKEN"); // Optional authentication

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Upload failed: " + request.error);
            }
        }
    }
}
