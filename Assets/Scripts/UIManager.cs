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

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     // void Start()
//     // {
//     //     ShowPanel(HomePanel);
//     // }
//     void Start()
//     {
//         if (HomePanel == null)
//         {
//             Debug.LogError("HomePanel is not assigned in the Inspector!");
//             return;
//         }
//         ShowPanel(HomePanel);
//     }

//     // public void ShowPanel(GameObject panel)
//     // {
//     //     if (currentPanel != null)
//     //     {
//     //         Destroy(currentPanel); // Remove the old panel
//     //     }

//     //     currentPanel = Instantiate(panel, transform); // Load new panel
//     // }
//     public void ShowPanel(GameObject panel)
//     {
//         if (currentPanel != null)
//         {
//             currentPanel.SetActive(false); // Disable the old panel instead of destroying it
//         }

//         panel.SetActive(true); // Show the new panel
//         currentPanel = panel; // Set the new panel as the current one
//     }

//     // Update is called once per frame
//     void Update() { }
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
    public GameObject AdminDashboardPanel;

    private GameObject currentPanel;

    void Start()
    {
        if (HomePanel == null)
        {
            Debug.LogError("HomePanel is not assigned in the Inspector!");
            return;
        }
        ShowPanel(HomePanel);
    }

    // public void ShowPanel(GameObject panel)
    // {
    //     if (panel == null)
    //     {
    //         Debug.LogError("Panel is not assigned!");
    //         return;
    //     }

    //     if (currentPanel != null)
    //     {
    //         currentPanel.SetActive(false); // Disable the old panel
    //     }

    //     panel.SetActive(true); // Show the new panel
    //     currentPanel = panel; // Set the new panel as the current one

    //     Debug.Log("Successfully switched to panel: " + panel.name); // Confirmation in the console
    // }
    public void ShowPanel(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Panel is not assigned!");
            return;
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

    void Update() { }
}
