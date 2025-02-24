// BASIC SWITCHING PANELS
// using UnityEngine;

// public class UIManager : MonoBehaviour
// {
//     public GameObject LoginPanel;
//     public GameObject RegisterPanel;
//     public GameObject HomePanel;
//     public GameObject UploadPanel;
//     public GameObject AnalyticsPanel1;
//     public GameObject ProfilePanel;
//     public GameObject AdminDashboardPanel;

//     private GameObject currentPanel;

//     void Start()
//     {
//         if (HomePanel == null)
//         {
//             Debug.LogError("HomePanel is not assigned in the Inspector!");
//             return;
//         }
//         ShowPanel(HomePanel);
//     }

//     public void ShowPanel(GameObject panel)
//     {
//         if (panel == null)
//         {
//             Debug.LogError("Panel is not assigned!");
//             return;
//         }

//         if (currentPanel != null)
//         {
//             Debug.Log("Disabling panel: " + currentPanel.name);
//             currentPanel.SetActive(false); // Disable the old panel
//         }

//         Debug.Log("Enabling panel: " + panel.name);
//         panel.SetActive(true); // Show the new panel
//         currentPanel = panel; // Set the new panel as the current one

//         Debug.Log("Successfully switched to panel: " + panel.name); 
//     }

//     void Update() { }
// }




// using UnityEngine;

// public class UIManager : MonoBehaviour
// {
//     public GameObject LoginPanel;
//     public GameObject RegisterPanel;
//     public GameObject HomePanel;
//     public GameObject UploadPanel;
//     public GameObject AnalyticsPanel1;
//     public GameObject ProfilePanel;

//     private GameObject currentPanel;
//     private bool isLoggedIn = false; // Track login status

//     void Start()
//     {
//         // Ensure HomePanel is always the first panel to show
//         if (HomePanel == null)
//         {
//             Debug.LogError("HomePanel is not assigned in the Inspector!");
//             return;
//         }

//         // Disable all panels except HomePanel at the start
//         DisableAllPanels();
//         ShowPanel(HomePanel);
//     }


//     public void ShowPanel(GameObject panel)
//     {
//         if (panel == null)
//         {
//             Debug.LogError("Panel is not assigned!");
//             return;
//         }

//         // Always allow HomePanel to be shown, even if not logged in
//         if (panel == HomePanel)
//         {
//             SwitchPanel(panel);
//             return;
//         }

//         // Allow access to LoginPanel and RegisterPanel without being logged in
//         if (panel == LoginPanel || panel == RegisterPanel)
//         {
//             SwitchPanel(panel);
//             return;
//         }

//         // If the user is not logged in, redirect to LoginPanel and disable other panels
//         if (!isLoggedIn)
//         {
//             Debug.Log("User is not logged in. Redirecting to LoginPanel.");
//             SwitchPanel(LoginPanel);
//             // SwitchPanel(RegisterPanel);
//             return;
//         }

//         if (panel == LoginPanel) {

//         }

//         // If the user is logged in, allow access to the requested panel
//         SwitchPanel(panel);
//     }
    

//     private void SwitchPanel(GameObject panel)
//     {
//         if (currentPanel != null)
//         {
//             Debug.Log("Disabling panel: " + currentPanel.name);
//             currentPanel.SetActive(false); // Disable the old panel
//         }

//         Debug.Log("Enabling panel: " + panel.name);
//         panel.SetActive(true); // Show the new panel
//         currentPanel = panel; // Set the new panel as the current one

//         Debug.Log("Successfully switched to panel: " + panel.name);
//     }

//     // Disable all panels except the one passed as an argument
//     private void DisableAllPanels()
//     {
//         LoginPanel.SetActive(false);
//         RegisterPanel.SetActive(false);
//         HomePanel.SetActive(false);
//         UploadPanel.SetActive(false);
//         AnalyticsPanel1.SetActive(false);
//         ProfilePanel.SetActive(false);
//     }

//     // Call this method when the user successfully logs in
//     public void OnLoginSuccess()
//     {
//         isLoggedIn = true;
//         ShowPanel(HomePanel); // Redirect to HomePanel after login
//     }

//     // Call this method when the user logs out
//     public void OnLogout()
//     {
//         isLoggedIn = false;
//         ShowPanel(LoginPanel); // Redirect to LoginPanel after logout
//     }

//     void Update() { }
// }




// OG CODE WAG BURAHIN WORKING
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    public GameObject HomePanel;
    public GameObject UploadPanel;
    public GameObject AnalyticsPanel1;
    public GameObject ProfilePanel;
    // public GameObject MalupetPanel;
    private GameObject currentPanel;
    private bool hasRedirectedToLogin = false; // Track if redirection has occurred

    void Start()
    {
        if (HomePanel == null || LoginPanel == null || RegisterPanel == null)
        {
            Debug.LogError("HomePanel, LoginPanel, or RegisterPanel is not assigned in the Inspector!");
            return;
        }

        ShowPanel(HomePanel); // Start at HomePanel
    }

    // public void ShowPanel(GameObject panel)
    // {
    //     if (panel == null)
    //     {
    //         Debug.LogError("Panel is not assigned!");
    //         return;
    //     }

    
    //     // If trying to access a restricted panel (not Home, Login, or Register) and user is NOT logged in, redirect to LoginPanel
    //     if (panel != HomePanel && panel != LoginPanel && panel != RegisterPanel && !IsUserLoggedIn())
    //     {
    //         Debug.Log("User not logged in. Redirecting to Login Panel.");
    //         panel = LoginPanel; // Redirect to LoginPanel
    //     }

    //     if (currentPanel != null)
    //     {
    //         Debug.Log("Disabling panel: " + currentPanel.name);
    //         currentPanel.SetActive(false); // Disable the old panel
    //     }

    //     Debug.Log("Enabling panel: " + panel.name);
    //     panel.SetActive(true); // Show the new panel
    //     currentPanel = panel; // Set the new panel as the current one

    //     Debug.Log("Successfully switched to panel: " + panel.name);
    // }

    public void ShowPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Panel is not assigned!");
            return;
        }

        // If trying to access a restricted panel (not Home, Login, or Register) and user is NOT logged in
        if (panel != HomePanel && panel != LoginPanel && panel != RegisterPanel && !IsUserLoggedIn())
        {
            // Redirect only if coming from HomePanel and redirection hasn't happened yet
            if (!hasRedirectedToLogin && currentPanel == HomePanel)
            {
                Debug.Log("User not logged in. Redirecting to Login Panel.");
                panel = LoginPanel; // Redirect to LoginPanel
                hasRedirectedToLogin = true; // Mark that redirection has occurred
            }
        }

        if (currentPanel != null)
        {
            Debug.Log("Disabling panel: " + currentPanel.name);
            currentPanel.SetActive(false); // Disable the old panel
        }

        Debug.Log("Enabling panel: " + panel.name);
        panel.SetActive(true); // Show the new panel
        currentPanel = panel; // Set the new panel as the current one

        Debug.Log("Successfully switched to panel: " + panel.name);
    }

    private bool IsUserLoggedIn()
    {
        return PlayerPrefs.HasKey("auth_token"); // Check if token exists
    }

    // Call this method when the user logs in
    public void OnUserLoggedIn()
    {
        hasRedirectedToLogin = false; // Reset flag so redirection can happen again if needed
    }
    

    public void HideAnalytics(GameObject Panelskie)
    {
        Panelskie.SetActive(false);
    }
}
