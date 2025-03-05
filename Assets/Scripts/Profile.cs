
//     private string baseUrl = "http://localhost:5000/api";

// using System.Collections;
// using UnityEngine;
// using UnityEngine.Networking;
// using TMPro;
// using SFB;
// using System.IO;
// using UnityEngine.UI;


// public class Profile : MonoBehaviour
// {
//     private string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api";

//     public TMP_InputField FirstNameInput;
//     public TMP_InputField LastNameInput;
//     public TMP_InputField AddressInput;
//     public TMP_InputField EmailInput;
//     public TMP_InputField PasswordInput;
//     public TMP_Text NotificationText;

//     public GameObject HomePanel;
//     public GameObject ProfilePanel;
//     public GameObject LoginPanel;

//     private string selectedFilePath; // Stores the selected file path

//     void Start()
//     {
//         StartCoroutine(FetchUserProfile());
//     }

//     IEnumerator FetchUserProfile()
//     {
//         string token = PlayerPrefs.GetString("jwtToken", "");

//         if (string.IsNullOrEmpty(token))
//         {
//             Debug.LogError("No token found, user needs to log in.");
//             ShowNotification("Session expired. Please log in.");
//             HomePanel.SetActive(true);
//             ProfilePanel.SetActive(false);
//             yield break;
//         }

//         using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/profile"))
//         {
//             request.downloadHandler = new DownloadHandlerBuffer();
//             request.SetRequestHeader("Authorization", "Bearer " + token);

//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("Profile fetched successfully: " + request.downloadHandler.text);
//                 UserProfile userProfile = JsonUtility.FromJson<UserProfile>(request.downloadHandler.text);

//                 FirstNameInput.text = userProfile.first_name;
//                 LastNameInput.text = userProfile.last_name;
//                 AddressInput.text = userProfile.address;
//                 EmailInput.text = userProfile.email;
//                 PasswordInput.text = "";
//             }
//             else
//             {
//                 Debug.LogError($"Error fetching profile: {request.error}, Response: {request.downloadHandler.text}");
//                 ShowNotification("Failed to fetch profile.");
//             }
//         }
//     }

//     // 🔹 BUTTON CLICK: Select File (Triggers File Picker)
//     public void OnSelectFileButtonClick()
//     {
//         var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
//         var paths = StandaloneFileBrowser.OpenFilePanel("Select Profile Image", "", extensions, false);

//         if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]) && File.Exists(paths[0]))
//         {
//             selectedFilePath = paths[0];
//             Debug.Log("Selected File: " + selectedFilePath);
//             ShowNotification("File selected: " + Path.GetFileName(selectedFilePath));
//         }
//         else
//         {
//             Debug.LogWarning("No valid file selected.");
//         }
//     }

//     // 🔹 BUTTON CLICK: Save Profile (Updates Details + Uploads Image)
//     public void OnUpdateProfileButtonClick()
//     {
//         StartCoroutine(UpdateUserProfile());
//     }

//     IEnumerator UpdateUserProfile()
//     {
//         string token = PlayerPrefs.GetString("jwtToken", "");

//         if (string.IsNullOrEmpty(token))
//         {
//             Debug.LogError("No token found, user needs to log in.");
//             ShowNotification("Session expired. Please log in.");
//             yield break;
//         }

//         WWWForm form = new WWWForm();
//         form.AddField("first_name", FirstNameInput.text);
//         form.AddField("last_name", LastNameInput.text);
//         form.AddField("address", AddressInput.text);
//         form.AddField("email", EmailInput.text);

//         if (!string.IsNullOrWhiteSpace(PasswordInput.text))
//         {
//             form.AddField("password", PasswordInput.text);
//         }

//         // 🔹 Upload the selected image (if user selected one)
//         if (!string.IsNullOrEmpty(selectedFilePath) && File.Exists(selectedFilePath))
//         {
//             byte[] fileData = File.ReadAllBytes(selectedFilePath);
//             form.AddBinaryData("images", fileData, Path.GetFileName(selectedFilePath), "image/png");
//         }

//         using (UnityWebRequest request = UnityWebRequest.Post($"{baseUrl}/update", form)) // ✅ FIXED LINE
//         {
//             request.SetRequestHeader("Authorization", "Bearer " + token);

//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("Profile updated successfully: " + request.downloadHandler.text);
//                 ShowNotification("Profile updated successfully!");
//                 PasswordInput.text = "";
//             }
//             else
//             {
//                 Debug.LogError($"Error updating profile: {request.error}, Response: {request.downloadHandler.text}");
//                 ShowNotification("Failed to update profile.");
//             }
//         }
//     }


//     void ShowNotification(string message)
//     {
//         NotificationText.text = message;
//         NotificationText.gameObject.SetActive(true);
//         StartCoroutine(HideNotification());
//     }

//     IEnumerator HideNotification()
//     {
//         yield return new WaitForSeconds(3f);
//         NotificationText.gameObject.SetActive(false);
//     }

//     [System.Serializable]
//     public class UserProfile
//     {
//         public string first_name;
//         public string last_name;
//         public string address;
//         public string email;
//         public string password;
//     }
// }
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using SFB;
using System.IO;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    private string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api";
    // private string baseUrl = "http://localhost:5000/api";

    public TMP_InputField FirstNameInput;
    public TMP_InputField LastNameInput;
    public TMP_InputField AddressInput;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public TMP_Text NotificationText;
    public Image ProfileImage; // 🔹 Add this in the Unity Inspector

    public GameObject HomePanel;
    public GameObject ProfilePanel;
    public GameObject LoginPanel;

    private string selectedFilePath; // Stores the selected file path

    void Start()
    {
        StartCoroutine(FetchUserProfile());
    }

    public IEnumerator FetchUserProfile()
    {
        string token = PlayerPrefs.GetString("jwtToken", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No token found, user needs to log in.");
            ShowNotification("Session expired. Please log in.");
            HomePanel.SetActive(true);
            ProfilePanel.SetActive(false);
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/profile"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Profile fetched successfully: " + request.downloadHandler.text);
                UserProfile userProfile = JsonUtility.FromJson<UserProfile>(request.downloadHandler.text);

                FirstNameInput.text = userProfile.first_name;
                LastNameInput.text = userProfile.last_name;
                AddressInput.text = userProfile.address;
                EmailInput.text = userProfile.email;
                PasswordInput.text = "";

                if (!string.IsNullOrEmpty(userProfile.images))
                {
                    StartCoroutine(LoadProfileImage(userProfile.images));
                }
            }
            else
            {
                Debug.LogError($"Error fetching profile: {request.error}, Response: {request.downloadHandler.text}");
                ShowNotification("Failed to fetch profile.");
            }
        }
    }

    IEnumerator LoadProfileImage(string imageUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                ProfileImage.sprite = SpriteFromTexture(texture);
            }
            else
            {
                Debug.LogError($"Failed to load image: {request.error}");
            }
        }
    }

    private Sprite SpriteFromTexture(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void OnSelectFileButtonClick()
    {
        var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
        var paths = StandaloneFileBrowser.OpenFilePanel("Select Profile Image", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]) && File.Exists(paths[0]))
        {
            selectedFilePath = paths[0];
            Debug.Log("Selected File: " + selectedFilePath);
            ShowNotification("File selected: " + Path.GetFileName(selectedFilePath));

            StartCoroutine(LoadImageFromFile(selectedFilePath));
        }
        else
        {
            Debug.LogWarning("No valid file selected.");
        }
    }

   
    public void OnUpdateProfileButtonClick()
    {
        StartCoroutine(UpdateUserProfile());
    }

    IEnumerator UpdateUserProfile()
    {
        string token = PlayerPrefs.GetString("jwtToken", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No token found, user needs to log in.");
            ShowNotification("Session expired. Please log in.");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("first_name", FirstNameInput.text);
        form.AddField("last_name", LastNameInput.text);
        form.AddField("address", AddressInput.text);
        form.AddField("email", EmailInput.text);

        if (!string.IsNullOrWhiteSpace(PasswordInput.text))
        {
            form.AddField("password", PasswordInput.text);
        }

        // 🔹 Upload the selected image (if user selected one)
        if (!string.IsNullOrEmpty(selectedFilePath) && File.Exists(selectedFilePath))
        {
            byte[] fileData = File.ReadAllBytes(selectedFilePath);
            form.AddBinaryData("images", fileData, Path.GetFileName(selectedFilePath), "image/png");
        }

        using (UnityWebRequest request = UnityWebRequest.Post($"{baseUrl}/update", form)) // ✅ FIXED LINE
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Profile updated successfully: " + request.downloadHandler.text);
                ShowNotification("Profile updated successfully!");
                PasswordInput.text = "";
            }
            else
            {
                Debug.LogError($"Error updating profile: {request.error}, Response: {request.downloadHandler.text}");
                ShowNotification("Failed to update profile.");
            }
        }
    }

    IEnumerator LoadImageFromFile(string filePath)
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        ProfileImage.sprite = SpriteFromTexture(texture);
        yield return null;
    }
    
    void ShowNotification(string message)
    {
        NotificationText.text = message;
        NotificationText.gameObject.SetActive(true);
        StartCoroutine(HideNotification());
    }

    IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(3f);
        NotificationText.gameObject.SetActive(false);
    }

    [System.Serializable]
    public class UserProfile
    {
        public string first_name;
        public string last_name;
        public string address;
        public string email;
        public string password;
        public string images;
    }

    public void ClearInputFields()
    {
        FirstNameInput.text = "";
        LastNameInput.text = "";
        AddressInput.text = "";
        EmailInput.text = "";
        PasswordInput.text = "";
    }
}
