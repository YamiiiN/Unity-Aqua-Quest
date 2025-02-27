// OG CODE WAG BURAHIN 
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LoginRegister : MonoBehaviour
{
    private string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api";
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

    public void OnRegisterButtonClick()
    {
        StartCoroutine(RegisterUser());
    }

    public void OnLoginButtonClick()
    {       
        StartCoroutine(LoginUser());
    }

    public void OnLogoutButtonClick()
    {
        LogoutUser();
    }

    IEnumerator RegisterUser()
    {
        if (FirstNameInput == null || LastNameInput == null || AddressInput == null || EmailInput == null || PasswordInput == null)
        {
            Debug.LogError("One or more InputFields are null! Assign them in the Inspector.");
            yield break;
        }

        string jsonData = JsonUtility.ToJson(new RegisterData()
        {
            first_name = FirstNameInput.text,
            last_name = LastNameInput.text,
            address = AddressInput.text,
            email = EmailInput.text,
            password = PasswordInput.text
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

    // OG CODE
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

    // LATEST NA GAMIT
    // IEnumerator LoginUser()
    // {
    //     Analytics analytics = FindObjectOfType<Analytics>();
    //     if (analytics != null)
    //     {
    //         analytics.ClearLatestBill();
    //     }

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
                
    //             PlayerPrefs.SetString("jwtToken", response.token);
    //             PlayerPrefs.Save(); 

    //             Debug.Log("Token saved: " + response.token);

    //             HomePanel.SetActive(true);
    //             LoginPanel.SetActive(false);
    //             EmailInput.text = "";
    //             PasswordInput.text = "";

    //             BillManager billManager = FindObjectOfType<BillManager>();
    //             if (billManager != null)
    //             {
    //                 StartCoroutine(billManager.FetchBills());  
    //             }

    //             if (analytics != null)
    //             {
    //                 StartCoroutine(analytics.FetchLatestBill()); 
    //                 StartCoroutine(analytics.FetchMonthlyConsumption());
    //                 StartCoroutine(analytics.FetchMonthlyCost());
                   
    //             }
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

            yield return request.SendWebRequest();

            Debug.Log("Request completed!");

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Successful: " + request.downloadHandler.text);
                ShowNotification("Login successful!");

                LoginResponse response = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

                PlayerPrefs.SetString("jwtToken", response.token);
                PlayerPrefs.Save();

                Debug.Log("Token saved: " + response.token);

                HomePanel.SetActive(true);
                LoginPanel.SetActive(false);
                EmailInput.text = "";
                PasswordInput.text = "";

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
                }
            }
            else
            {
                Debug.LogError($"Login Error: {request.error}, Response: {request.downloadHandler.text}");
                ShowNotification($"Error: {request.error}");
            }
        }
    }



    // OG CODE
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

    // LATEST NA GAMIT
    // public void LogoutUser()
    // {
    //     PlayerPrefs.DeleteKey("jwtToken");
    //     PlayerPrefs.Save();
    //     Debug.Log("User logged out. Token removed.");

    //     BillManager billManager = FindObjectOfType<BillManager>();
    //     if (billManager != null)
    //     {
    //         billManager.ClearBills(); 
    //     }

    //     Analytics analytics = FindObjectOfType<Analytics>();
    //     if (analytics != null)
    //     {
    //         analytics.ClearLatestBill(); 
    //     }

    //     if (string.IsNullOrEmpty(PlayerPrefs.GetString("jwtToken", "")))
    //     {
    //         Debug.Log("Token successfully cleared.");
    //     }
    //     else
    //     {
    //         Debug.LogError("Token still exists after logout!");
    //     }

    //     HomePanel.SetActive(false);
    //     UploadPanel.SetActive(false);
    //     AnalyticsPanel1.SetActive(false);
    //     ProfilePanel.SetActive(false);
    //     LoginPanel.SetActive(true);
    //     ShowNotification("You have been logged out.");
    // }

    public void LogoutUser()
    {
        PlayerPrefs.DeleteKey("jwtToken");
        PlayerPrefs.Save();
        Debug.Log("User logged out. Token removed.");

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

        HomePanel.SetActive(false);
        UploadPanel.SetActive(false);
        AnalyticsPanel1.SetActive(false);
        ProfilePanel.SetActive(false);
        BillPanel.SetActive(false);
        LoginPanel.SetActive(true);

        ShowNotification("You have been logged out.");
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
        public string token;
    }
}
