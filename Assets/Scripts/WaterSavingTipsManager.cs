// WORKING NALABAS NA SA CONSOLE RESPONSE NI GROQ
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json.Linq;

public class WaterSavingTipsManager : MonoBehaviour
{
    public GameObject tipPrefab;
    public Transform contentPanel;
    private string apiUrl = "http://localhost:5000/api/chart/water-saving-tips";
    // private string apiUrl = "https://aqua-quest-backend-deployment.onrender.com/api/chart/water-saving-tips";

    void Start()
    {
        StartCoroutine(FetchWaterSavingTips());
    }

    public IEnumerator FetchWaterSavingTips()
    {
        string authToken = PlayerPrefs.GetString("jwtToken");
        if (string.IsNullOrEmpty(authToken))
        {
            Debug.LogError("Authorization token is missing.");
            yield break;
        }

        Debug.Log("Fetching water-saving tips from API: " + apiUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Raw JSON Response: " + jsonResponse);

                try
                {
                    JObject responseObject = JObject.Parse(jsonResponse);

                    if (responseObject["tips"] != null)
                    {
                        JArray tipsArray = (JArray)responseObject["tips"];
                        List<string> tips = tipsArray.ToObject<List<string>>();

                        Debug.Log($"Number of tips received: {tips.Count}");

                        if (tips.Count > 0)
                        {
                            PopulateTips(tips.ToArray());
                        }
                        else
                        {
                            Debug.LogWarning("No water-saving tips were received from the server.");
                            PopulateTips(new string[] { "No tips available at this time." });
                        }
                    }
                    else
                    {
                        Debug.LogError("Response does not contain 'tips' key.");
                        PopulateTips(new string[] { "No tips available." });
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error parsing tips: " + ex.Message);
                    PopulateTips(new string[] { "Error occurred while fetching tips." });
                }
            }
            else
            {
                Debug.LogError($"API Error: {request.error} ({request.responseCode})");
                PopulateTips(new string[] { "Failed to fetch tips." });
            }
        }
    }

    private void PopulateTips(string[] tips)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (string tip in tips)
        {
            GameObject newTip = Instantiate(tipPrefab, contentPanel);
            TextMeshProUGUI textComponent = newTip.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent)
                textComponent.text = tip;
            else
                Debug.LogError("tipPrefab does not have a TextMeshProUGUI component!");
        }

        StartCoroutine(AdjustContentPanelHeight());
    }

    private IEnumerator AdjustContentPanelHeight()
    {
        yield return new WaitForEndOfFrame();

        RectTransform contentPanelRect = contentPanel.GetComponent<RectTransform>();
        float totalHeight = 0f;

        foreach (Transform child in contentPanel)
        {
            totalHeight += child.GetComponent<RectTransform>().rect.height;
        }

        contentPanelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }

    public void ClearTips()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
