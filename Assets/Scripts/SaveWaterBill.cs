using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using XCharts.Runtime;
using System.Globalization;
using System.Text;

public class SaveWaterBill : MonoBehaviour
{
    // private string saveUrl = "http://localhost:5000/api/save/save-waterbill";
    // private string baseUrl = "http://localhost:5000/api/save";

    private string saveUrl = "https://aqua-quest-backend-deployment.onrender.com/api/save/save-waterbill";
    private string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api/save";

    public BarChart savedChart; 

    private void Start()
    {
        StartCoroutine(FetchMonthlySavedCost());
    }

    public IEnumerator SavePredictedVsActual()
    {
        string jwtToken = PlayerPrefs.GetString("jwtToken", "");
        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is missing. Please log in.");
            yield break;
        }

        string lastSavedMonth = PlayerPrefs.GetString("lastSavedMonth", "");
        string currentMonth = DateTime.UtcNow.ToString("yyyy-MM");

        if (lastSavedMonth == currentMonth)
        {
            Debug.Log("✅ Data already saved for this month. Skipping request.");
            yield break;
        }

        string jsonData = "{}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(saveUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Save Successful: " + responseText);

                SavedRecord savedRecord = JsonUtility.FromJson<SavedRecord>(responseText);
                PlayerPrefs.SetString("lastSavedMonth", savedRecord.month);
                PlayerPrefs.SetFloat("lastSavedCost", savedRecord.savedCost);
                PlayerPrefs.SetFloat("lastSavedConsumption", savedRecord.savedConsumption);
                PlayerPrefs.Save();

                StartCoroutine(FetchMonthlySavedCost()); 
            }
            else
            {
                Debug.LogError("Save Failed: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class SavedRecord
    {
        public string month;
        public float savedCost;
        public float savedConsumption;
    }

    private string GetJWTToken()
    {
        string token = PlayerPrefs.GetString("jwtToken", "");
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("JWT token is missing. Please log in.");
        }
        return token;
    }

    public IEnumerator FetchMonthlySavedCost()
    {
        string jwtToken = GetJWTToken();
        if (string.IsNullOrEmpty(jwtToken)) yield break;

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/save-cost"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Monthly Cost Data: " + jsonResponse);

                JArray costData = JArray.Parse(jsonResponse);
                List<string> months = new List<string>();
                List<float> costs = new List<float>();

                foreach (JObject entry in costData)
                {
                    string rawMonth = entry["_id"].ToString();
                    if (DateTime.TryParseExact(rawMonth, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        months.Add(parsedDate.ToString("MMMM yyyy"));
                    }
                    else
                    {
                        months.Add(rawMonth); 
                    }
                    costs.Add(float.Parse(entry["totalCost"].ToString(), CultureInfo.InvariantCulture));
                }


                UpdateSavedChart(months, costs);
            }
            else
            {
                Debug.LogError($"Failed to fetch monthly cost. Error: {request.error}");
            }
        }
    }
    private void UpdateSavedChart(List<string> months, List<float> costs)
    {
        if (savedChart == null)
        {
            Debug.LogError("Saved Chart is not assigned in the Inspector.");
            return;
        }

        savedChart.ClearData();
        savedChart.AddSerie<Bar>();

        for (int i = 0; i < months.Count; i++)
        {
            savedChart.AddXAxisData(months[i]);
            savedChart.AddData(0, costs[i]);
        }
    }

}

