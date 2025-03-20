// OG CODE WAG BURAHIN 
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
// using UnityEditor.Overlays;


public class LoginRegister : MonoBehaviour
{
    private string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api";
    // private string baseUrl = "http://localhost:5000/api";
    private string PythonbaseUrl = "https://aquaquest-flask.onrender.com/api";

    public TMP_InputField FirstNameInput;
    public TMP_InputField LastNameInput;    
    public TMP_InputField AddressInput;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public TMP_Text NotificationText;

    public GameObject HomePanel;
    public GameObject LoginPanel;
    public GameObject UploadPanel;
    public GameObject AnalyticsPanel1;
    public GameObject ProfilePanel;
    public GameObject BillPanel;
    public GameObject LoadingScreen;

    public TMP_Dropdown GenderDropdown;

    private string userInfoFilePath;

    // BAGO
    void Start()
    {
        userInfoFilePath = Path.Combine(Application.persistentDataPath, "userInfo.json");
        Debug.Log("User info file path: " + userInfoFilePath);

        // Ensure the file exists
        if (!File.Exists(userInfoFilePath))
        {
            File.WriteAllText(userInfoFilePath, "{}"); // Creates an empty JSON object
            // Debug.Log("Created new userInfo.json file");
        }

        // Load user info when the game starts
        LoadUserInfo();
    }

    public void OnRegisterButtonClick()
    {
        StartCoroutine(RegisterUser());
    }

    public void OnLoginButtonClick()
    {       
        StartCoroutine(LoginUser());
        // SendData.GetPlayerData();
        // GenerateFileAfterLogin.SaveData();
        
    }

    public void OnLogoutButtonClick()
    {
        LogoutUser();
        GenerateFileAfterLogin.DestroyFiles();
    }

    IEnumerator RegisterUser()
    {
        if (FirstNameInput == null || LastNameInput == null || AddressInput == null || EmailInput == null || PasswordInput == null)
        {
            Debug.LogError("One or more InputFields are null! Assign them in the Inspector.");
            yield break;
        }

        string selectedGender = GenderDropdown.options[GenderDropdown.value].text;

        string jsonData = JsonUtility.ToJson(new RegisterData()
        {
            first_name = FirstNameInput.text,
            last_name = LastNameInput.text,
            address = AddressInput.text,
            email = EmailInput.text,
            password = PasswordInput.text,
            gender = selectedGender
        });


        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/register", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Registration Successful!");
                ShowNotification("Account successfully created!");
                ClearInputFields();
            }
            else
            {
                Debug.LogError("Registration Error: " + request.error);
            }
        }
    }

    // OG CODE (INCASE OF EMERGENCY)
    // IEnumerator LoginUser()
    // {
    //     if (EmailInput == null || PasswordInput == null)
    //     {
    //         Debug.LogError("EmailInput or PasswordInput is not assigned in the Inspector!");
    //         yield break;
    //     }

    //     if (string.IsNullOrEmpty(EmailInput.text) || string.IsNullOrEmpty(PasswordInput.text))
    //     {
    //         Debug.LogError("Email or password is empty.");
    //         ShowNotification("Email and password are required.");
    //         yield break;
    //     }

    //     string jsonData = JsonUtility.ToJson(new LoginData()
    //     {
    //         email = EmailInput.text,
    //         password = PasswordInput.text
    //     });

    //     byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

    //     using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/login", "POST"))
    //     {
    //         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //         request.downloadHandler = new DownloadHandlerBuffer();
    //         request.SetRequestHeader("Content-Type", "application/json");

    //         Debug.Log("Sending login request...");

    //         yield return request.SendWebRequest();

    //         Debug.Log("Request completed!");

    //         if (request.result == UnityWebRequest.Result.Success)
    //         {
    //             Debug.Log("Login Successful: " + request.downloadHandler.text);
    //             ShowNotification("Login successful!");

    //             LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                
    //             PlayerPrefs.SetString("jwtToken", response.token); // Store token for ImageUploader
    //             PlayerPrefs.Save(); 

    //             Debug.Log("Token saved: " + response.token);

    //             HomePanel.SetActive(true);
    //             LoginPanel.SetActive(false);
    //             EmailInput.text = "";
    //             PasswordInput.text = "";
    //         }
    //         else
    //         {
    //             Debug.LogError($"Login Error: {request.error}, Response: {request.downloadHandler.text}");
    //             ShowNotification($"Error: {request.error}");
    //         }
    //     }
    // }

    IEnumerator LoginUser()
    {
        Analytics analytics = FindObjectOfType<Analytics>();
        if (analytics != null)
        {
            analytics.ClearCharts();
            analytics.ClearLatestBill();
        }

        if (EmailInput == null || PasswordInput == null)
        {
            Debug.LogError("EmailInput or PasswordInput is not assigned in the Inspector!");
            yield break;
        }

        if (string.IsNullOrEmpty(EmailInput.text) || string.IsNullOrEmpty(PasswordInput.text))
        {
            Debug.LogError("Email or password is empty.");
            ShowNotification("Email and password are required.");
            yield break;
        }

        string jsonData = JsonUtility.ToJson(new LoginData()
        {
            email = EmailInput.text,
            password = PasswordInput.text
        });

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/login", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending login request...");
            LoadingScreen.SetActive(true);
            yield return request.SendWebRequest();

            Debug.Log("Request completed!");

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Successful: " + request.downloadHandler.text);
                LoadingScreen.SetActive(false);
                ShowNotification("Login successful!");

                // LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

                // PlayerPrefs.SetString("jwtToken", response.token);
                // PlayerPrefs.Save();

                LoginPanel.SetActive(false);
                HomePanel.SetActive(true);
                
                EmailInput.text = "";
                PasswordInput.text = "";
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                PlayerPrefs.SetString("jwtToken", response.token);
                PlayerPrefs.Save();

                // Extract User ID from the token
                string extractedUserId = GetUserIdFromToken(response.token);
                if (!string.IsNullOrEmpty(extractedUserId))
                {
                    SaveUserInfo(extractedUserId, response.token);
                    
                    Debug.Log("IMSENDINGHERE");
                    
                }


                Debug.Log("Token saved: " + response.token);

                

                BillManager billManager = FindObjectOfType<BillManager>();
                if (billManager != null)
                {
                    StartCoroutine(billManager.FetchBills());  
                }

                if (analytics != null)
                {
                    StartCoroutine(analytics.FetchLatestBill()); 
                    StartCoroutine(analytics.FetchMonthlyConsumption());
                    StartCoroutine(analytics.FetchMonthlyCost());
                    StartCoroutine(analytics.FetchPredictedMonthlyConsumption());
                    StartCoroutine(analytics.FetchPredictedMonthlyCost());
                }

                WaterSavingTipsManager fetchTips = FindObjectOfType<WaterSavingTipsManager>();
                if (fetchTips != null)
                {
                    StartCoroutine(fetchTips.FetchWaterSavingTips()); 
                }

                Profile profile = FindObjectOfType<Profile>();
                if (profile != null)
                {
                    StartCoroutine(profile.FetchUserProfile()); 
                }

                SaveWaterBill saveWaterBiill = FindObjectOfType<SaveWaterBill>();
                if (saveWaterBiill != null)
                {
                    StartCoroutine(saveWaterBiill.SavePredictedVsActual()); 
                    StartCoroutine(saveWaterBiill.FetchMonthlySavedCost()); 
                }
            }
            else
            {
                Debug.LogError($"Login Error: {request.error}, Response: {request.downloadHandler.text}");
                ShowNotification($"Error: {request.error}");
                LoadingScreen.SetActive(false);
            }
        }
    }

    // BAGO
    public static string GetUserIdFromToken(string jwtToken)
    {
        try
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                Debug.LogError("JWT token is empty or null.");
                return null;
            }

            string[] tokenParts = jwtToken.Split('.');
            if (tokenParts.Length != 3) // A valid JWT should have 3 parts
            {
                Debug.LogError("Invalid JWT token format: " + jwtToken);
                return null;
            }

            string payload = tokenParts[1];
            Debug.Log("JWT Payload (Base64 Encoded): " + payload);

            // Decode Base64 URL (convert it to standard Base64)
            payload = payload.Replace('-', '+').Replace('_', '/');
            while (payload.Length % 4 != 0) // Fix padding if needed
            {
                payload += "=";
            }

            byte[] decodedBytes = Convert.FromBase64String(payload);
            string decodedJson = Encoding.UTF8.GetString(decodedBytes);

            Debug.Log("Decoded JWT Payload: " + decodedJson);

            JObject payloadData = JObject.Parse(decodedJson);

            // Match the backend field for user ID
            string userId = payloadData.ContainsKey("id") ? payloadData["id"]?.ToString() :
                            payloadData.ContainsKey("_id") ? payloadData["_id"]?.ToString() : null;

            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("JWT payload does not contain 'id' or '_id' field.");
                return null;
            }

            Debug.Log("Extracted User ID: " + userId);
            return userId;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error decoding JWT token: {ex.Message}");
            return null;
        }
    }

    // Ensures Base64 padding is correct
    private static string PadBase64(string input)
    {
        int padding = 4 - (input.Length % 4);
        if (padding != 4) input += new string('=', padding);
        return input.Replace('-', '+').Replace('_', '/'); // Convert Base64 URL encoding to standard Base64
    }


    void SaveUserInfo(string userId, string token)
    {
        UserInfo userInfo = new UserInfo { userId = userId, token = token };
        string json = JsonUtility.ToJson(userInfo, true); 
        File.WriteAllText(userInfoFilePath, json);

        Debug.Log("User info saved: " + json);
    }

    void LoadUserInfo()
    {
        if (File.Exists(userInfoFilePath))
        {
            string json = File.ReadAllText(userInfoFilePath);
            if (!string.IsNullOrWhiteSpace(json)) 
            {
                UserInfo userInfo = JsonUtility.FromJson<UserInfo>(json);
                Debug.Log("Loaded User Info: " + json);
            }
        }
        else
        {
            Debug.Log("No user info file found. Creating an empty file.");
            File.WriteAllText(userInfoFilePath, "{}"); // Ensure the file exists
        }
    }


    // OG CODE (INCASE OF EMERGENCY)
    // public void LogoutUser()
    // {
    //     PlayerPrefs.DeleteKey("jwtToken");
    //     PlayerPrefs.Save();
    //     Debug.Log("User logged out. Token removed.");

    //     if (string.IsNullOrEmpty(PlayerPrefs.GetString("jwtToken", "")))
    //     {
    //         Debug.Log("Token successfully cleared.");
    //     }
    //     else
    //     {
    //         Debug.LogError("Token still exists after logout!");
    //     }

    //     HomePanel.SetActive(false);
    //     LoginPanel.SetActive(true);
    //     ShowNotification("You have been logged out.");
    // }

    public void LogoutUser()
    {
        // PlayerPrefs.DeleteKey("jwtToken");
        // PlayerPrefs.Save();
        // Debug.Log("User logged out. Token removed.");
        PlayerPrefs.DeleteKey("jwtToken");
        PlayerPrefs.Save();
        if (File.Exists(userInfoFilePath))
        {
            File.WriteAllText(userInfoFilePath, "{}"); // Empty the JSON file
        }

        BillManager billManager = FindObjectOfType<BillManager>();
        if (billManager != null)
        {
            billManager.ClearBills(); 
        }

        Analytics analytics = FindObjectOfType<Analytics>();
        if (analytics != null)
        {
            analytics.ClearCharts();
            analytics.ClearLatestBill(); 
        }

        WaterSavingTipsManager fetchTips = FindObjectOfType<WaterSavingTipsManager>();
        if (fetchTips != null)
        {
            fetchTips.ClearTips();
        }

        Profile profile = FindObjectOfType<Profile>();
        if (profile != null)
        {
            profile.ClearInputFields();
        }

        HomePanel.SetActive(false);
        UploadPanel.SetActive(false);
        AnalyticsPanel1.SetActive(false);
        ProfilePanel.SetActive(false);
        BillPanel.SetActive(false);
        LoginPanel.SetActive(true);

        ShowNotification("You have been logged out.");
        Debug.Log("User logged out. userInfo.json emptied.");
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

    void ClearInputFields()
    {
        FirstNameInput.text = "";
        LastNameInput.text = "";
        AddressInput.text = "";
        EmailInput.text = "";
        PasswordInput.text = "";
    }

    

    [System.Serializable]
    public class RegisterData
    {
        public string first_name;
        public string last_name;
        public string address;
        public string email;
        public string password;
        public string gender;
    }

    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string userId;
        public string token;
    }

    [System.Serializable]
    public class UserInfo
    {
        public string userId;
        public string token;
    }
}