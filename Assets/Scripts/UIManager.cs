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
    public GameObject BillPanel;

    private GameObject currentPanel;
    private bool hasRedirectedToLogin = false; 

    void Start()
    {
        if (HomePanel == null || LoginPanel == null || RegisterPanel == null)
        {
            Debug.LogError("HomePanel, LoginPanel, or RegisterPanel is not assigned in the Inspector!");
            return;
        }

        ShowPanel(HomePanel); 
    }

    public void ShowPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Panel is not assigned!");
            return;
        }

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
            Debug.Log("Disabling panel: " + currentPanel.name);
            currentPanel.SetActive(false); 
        }

        Debug.Log("Enabling panel: " + panel.name);
        panel.SetActive(true); 
        currentPanel = panel; 

        Debug.Log("Successfully switched to panel: " + panel.name);
    }

    private bool IsUserLoggedIn()
    {
        return PlayerPrefs.HasKey("jwtToken");
    }

    
    public void OnUserLoggedIn()
    {
        hasRedirectedToLogin = false;
    }

}
