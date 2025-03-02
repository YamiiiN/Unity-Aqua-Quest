using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class WaterSavingTipsManager : MonoBehaviour
{
    public GameObject tipPrefab; 
    public Transform contentPanel; 
    // private string apiUrl = "http://localhost:5000/api/chart/water-saving-tips"; 
    private string apiUrl = "https://aqua-quest-backend-deployment.onrender.com/api/chart/water-saving-tips";

    void Start()
    {
        StartCoroutine(FetchWaterSavingTips());
    }

    IEnumerator FetchWaterSavingTips()
    {
        string authToken = PlayerPrefs.GetString("jwtToken");

        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("Authorization token is missing.");
            yield break;
        }

        Debug.Log("Token Confirmation para sa Saving Tips: " + authToken);

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // Deserialize the response
                    WaterSavingTipsResponse response = JsonUtility.FromJson<WaterSavingTipsResponse>(request.downloadHandler.text);

                    // Ensure tips are received and there are at least 4
                    if (response != null && response.tips != null && response.tips.Length >= 4)
                    {
                        Debug.Log("Received Tips: " + string.Join(", ", response.tips));
                        PopulateTips(response.tips);
                    }
                    else
                    {
                        Debug.LogWarning("Insufficient tips received. Displaying default message.");
                        PopulateTips(new string[] { "Not enough water-saving tips available. Try again later." });
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error parsing water-saving tips: " + ex.Message);
                    PopulateTips(new string[] { "Error occurred while fetching tips." });
                }
            }
            else
            {
                // Handle HTTP errors
                Debug.LogError($"Error fetching water-saving tips: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
                Debug.LogError($"Response Text: {request.downloadHandler.text}");
                PopulateTips(new string[] { "Error occurred while fetching tips." });
            }
        }
    }

    // Function to populate the tips in the ScrollView
    private void PopulateTips(string[] tips)
    {
        // Clear old tips
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new tips
        foreach (string tip in tips)
        {
            GameObject newTip = Instantiate(tipPrefab, contentPanel);

            // Ensure the prefab has a TextMeshProUGUI component
            TMPro.TextMeshProUGUI textComponent = newTip.GetComponent<TMPro.TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = tip;
            }
            else
            {
                Debug.LogError("tipPrefab does not have a TextMeshProUGUI component!");
            }
        }
    }
}

[System.Serializable]
public class WaterSavingTipsResponse
{
    public string[] tips;
}
