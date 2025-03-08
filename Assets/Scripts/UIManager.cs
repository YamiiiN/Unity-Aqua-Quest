// using UnityEngine;

// public class UIManager : MonoBehaviour
// {
//     public GameObject LoginPanel;
//     public GameObject RegisterPanel;
//     public GameObject HomePanel;
//     public GameObject UploadPanel;
//     public GameObject AnalyticsPanel1;
//     public GameObject ProfilePanel;
//     public GameObject BillPanel;

//     private GameObject currentPanel;
//     private bool hasRedirectedToLogin = false; 

//     void Start()
//     {


//         ShowPanel(HomePanel); 
//     }

//     public void ShowPanel(GameObject panel)
//     {

//         if (panel != HomePanel && panel != LoginPanel && panel != RegisterPanel && !IsUserLoggedIn())
//         {
//             if (!hasRedirectedToLogin && currentPanel == HomePanel)
//             {
//                 Debug.Log("User not logged in. Redirecting to Login Panel.");
//                 panel = LoginPanel; 
//                 hasRedirectedToLogin = true; 
//             }
//         }

//         if (currentPanel != null)
//         {
//             // Debug.Log("Disabling panel: " + currentPanel.name);
//             currentPanel.SetActive(false); 
//         }

//         // Debug.Log("Enabling panel: " + panel.name);
//         panel.SetActive(true); 
//         currentPanel = panel; 

//         // Debug.Log("Successfully switched to panel: " + panel.name);
//     }

//     private bool IsUserLoggedIn()
//     {
//         return PlayerPrefs.HasKey("jwtToken");
//     }

    
//     public void OnUserLoggedIn()
//     {
//         hasRedirectedToLogin = false;
//     }

// }




using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject HomePanel;
    public GameObject UploadPanel;
    public GameObject AnalyticsPanel1;
    public GameObject ProfilePanel;
    public GameObject BillPanel;

    private GameObject currentPanel;
    private bool hasRedirectedToLogin = false; 

    void Start()
    {
        ShowPanel(HomePanel);  

        // ✅ Fetch data if user is logged in
        if (IsUserLoggedIn())
        {
            FetchUserData();
        }
    }

    public void ShowPanel(GameObject panel)
    {
        if (panel != HomePanel && panel != LoginPanel && panel != RegisterPanel && !IsUserLoggedIn())
        {
            if (!hasRedirectedToLogin && currentPanel == HomePanel)
            {
                Debug.Log("User not logged in. Redirecting to Login Panel.");
                panel = LoginPanel; 
                hasRedirectedToLogin = true; 
            }
        }

        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        panel.SetActive(true);
        currentPanel = panel;
    }

    private bool IsUserLoggedIn()
    {
        return PlayerPrefs.HasKey("jwtToken");
    }

    // ✅ New function to fetch all necessary data
    private void FetchUserData()
    {
        Debug.Log("🔄 Fetching user-related data...");

        BillManager billManager = FindObjectOfType<BillManager>();
        if (billManager != null)
        {
            StartCoroutine(billManager.FetchBills());  
        }

        Analytics analytics = FindObjectOfType<Analytics>();
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

        SaveWaterBill saveWaterBill = FindObjectOfType<SaveWaterBill>();
        if (saveWaterBill != null)
        {
            StartCoroutine(saveWaterBill.FetchMonthlySavedCost()); 
        }
    }

    // ✅ Call this when the user logs in
    public void OnUserLoggedIn()
    {
        hasRedirectedToLogin = false;
        FetchUserData(); // Fetch data immediately after login
    }
}
