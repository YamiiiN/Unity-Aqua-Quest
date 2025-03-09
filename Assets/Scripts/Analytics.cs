// NO CHARTS PA
// using System.Collections;
// using UnityEngine;
// using UnityEngine.Networking;
// using TMPro;
// using Newtonsoft.Json.Linq;

// public class Analytics : MonoBehaviour
// {
//     private string baseUrl = "http://localhost:5000/api"; 

//     public TMP_Text DateText;
//     public TMP_Text ConsumptionText;
//     public TMP_Text CostText;

//     void Start()
//     {
//         StartCoroutine(FetchLatestBill());
//     }

//     // WORKING (PANG EMERGENCY IF MABURA YUNG LATEST)
//     // public IEnumerator FetchLatestBill()
//     // {
//     //     string jwtToken = PlayerPrefs.GetString("jwtToken", "");

//     //     if (string.IsNullOrEmpty(jwtToken))
//     //     {
//     //         Debug.LogError("JWT token is missing. Please log in.");
//     //         yield break;
//     //     }

//     //     UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/waterBill/latest");
//     //     request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

//     //     yield return request.SendWebRequest();

//     //     if (request.result == UnityWebRequest.Result.Success)
//     //     {
//     //         string jsonResponse = request.downloadHandler.text;
//     //         Debug.Log("Latest Bill Data: " + jsonResponse);

//     //         JObject latestBill = JObject.Parse(jsonResponse);

//     //         if (DateText != null) DateText.text = latestBill["billDate"]?.ToString() ?? "N/A";
//     //         if (ConsumptionText != null) ConsumptionText.text = latestBill["waterConsumption"]?.ToString() + " m³" ?? "N/A";
//     //         if (CostText != null) CostText.text = "₱" + latestBill["billAmount"]?.ToString() ?? "N/A";
//     //     }
//     //     else
//     //     {
//     //         Debug.LogError($"Failed to fetch latest bill. HTTP Code: {request.responseCode}, Error: {request.error}");
//     //         Debug.LogError($"Response: {request.downloadHandler.text}");
//     //     }
//     // }

//     public IEnumerator FetchLatestBill()
//     {
//         string jwtToken = PlayerPrefs.GetString("jwtToken", "");

//         if (string.IsNullOrEmpty(jwtToken))
//         {
//             Debug.LogError("JWT token is missing. Please log in.");
//             yield break;
//         }

//         UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/waterBill/latest");
//         request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

//         yield return request.SendWebRequest();

//         if (request.result == UnityWebRequest.Result.Success)
//         {
//             string jsonResponse = request.downloadHandler.text;
//             Debug.Log("Latest Bill Data: " + jsonResponse);

//             JObject latestBill = JObject.Parse(jsonResponse);

//             if (latestBill.ContainsKey("message") && latestBill["message"].ToString() == "No bills found")
//             {
//                 Debug.LogWarning("No bills found for this user.");
//             }
//             else
//             {
//                 if (DateText != null) DateText.text = latestBill["billDate"]?.ToString() ?? "N/A";
//                 if (ConsumptionText != null) ConsumptionText.text = latestBill["waterConsumption"]?.ToString() + " m³" ?? "N/A";
//                 if (CostText != null) CostText.text = "₱" + latestBill["billAmount"]?.ToString() ?? "N/A";
//             }
//         }
//         else
//         {
//             Debug.LogError($"Failed to fetch latest bill. HTTP Code: {request.responseCode}, Error: {request.error}");
//             Debug.LogError($"Response: {request.downloadHandler.text}");
//         }
//     }


//     public void ClearLatestBill()
//     {
//         if (DateText != null) DateText.text = "";
//         if (ConsumptionText != null) ConsumptionText.text = "";
//         if (CostText != null) CostText.text = "";

//         Debug.Log("Latest bill information cleared.");
//     }
// }




// W/ CHARTS
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using TMPro;
// using Newtonsoft.Json.Linq;
// using XCharts; 
// using XCharts.Runtime;

// public class Analytics : MonoBehaviour
// {
//     private string baseUrl = "http://localhost:5000/api"; 
//     public TMP_Text DateText;
//     public TMP_Text ConsumptionText;
//     public TMP_Text CostText;

//     public BarChart consumptionChart;
//     public LineChart costChart;

//     void Start()
//     {
//         StartCoroutine(FetchLatestBill());
//         StartCoroutine(FetchMonthlyConsumption());
//         StartCoroutine(FetchMonthlyCost());
//     }

//     private string GetJWTToken()
//     {
//         string token = PlayerPrefs.GetString("jwtToken", "");
//         if (string.IsNullOrEmpty(token))
//         {
//             Debug.LogError("JWT token is missing. Please log in.");
//         }
//         return token;
//     }

//     public IEnumerator FetchLatestBill()
//     {
//         string jwtToken = GetJWTToken();
//         if (string.IsNullOrEmpty(jwtToken)) yield break;

//         using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/waterBill/latest"))
//         {
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 string jsonResponse = request.downloadHandler.text;
//                 Debug.Log("Latest Bill Data: " + jsonResponse);

//                 JObject latestBill = JObject.Parse(jsonResponse);
//                 if (latestBill.ContainsKey("message") && latestBill["message"].ToString() == "No bills found")
//                 {
//                     Debug.LogWarning("No bills found for this user.");
//                 }
//                 else
//                 {
//                     DateText.text = latestBill["billDate"]?.ToString() ?? "N/A";
//                     ConsumptionText.text = latestBill["waterConsumption"]?.ToString() + " m³" ?? "N/A";
//                     CostText.text = "₱" + (latestBill["billAmount"]?.ToString() ?? "N/A");
//                 }
//             }
//             else
//             {
//                 Debug.LogError($"Failed to fetch latest bill. HTTP Code: {request.responseCode}, Error: {request.error}");
//             }
//         }
//     }

//     public IEnumerator FetchMonthlyConsumption()
//     {
//         string jwtToken = GetJWTToken();
//         if (string.IsNullOrEmpty(jwtToken)) yield break;

//         using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/chart/monthly-consumption"))
//         {
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 string jsonResponse = request.downloadHandler.text;
//                 Debug.Log("Monthly Consumption Data: " + jsonResponse);

//                 JArray consumptionData = JArray.Parse(jsonResponse);
//                 List<string> months = new List<string>();
//                 List<float> consumptions = new List<float>();

//                 foreach (JObject entry in consumptionData)
//                 {
//                     months.Add(entry["_id"].ToString());
//                     consumptions.Add(float.Parse(entry["totalConsumption"].ToString()));
//                 }

//                 UpdateConsumptionChart(months, consumptions);
//             }
//             else
//             {
//                 Debug.LogError($"Failed to fetch monthly consumption. Error: {request.error}");
//             }
//         }
//     }

//     public IEnumerator FetchMonthlyCost()
//     {
//         string jwtToken = GetJWTToken();
//         if (string.IsNullOrEmpty(jwtToken)) yield break;

//         using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/chart/monthly-cost"))
//         {
//             request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 string jsonResponse = request.downloadHandler.text;
//                 Debug.Log("Monthly Cost Data: " + jsonResponse);

//                 JArray costData = JArray.Parse(jsonResponse);
//                 List<string> months = new List<string>();
//                 List<float> costs = new List<float>();

//                 foreach (JObject entry in costData)
//                 {
//                     months.Add(entry["_id"].ToString());
//                     costs.Add(float.Parse(entry["totalCost"].ToString()));
//                 }

//                 UpdateCostChart(months, costs);
//             }
//             else
//             {
//                 Debug.LogError($"Failed to fetch monthly cost. Error: {request.error}");
//             }
//         }
//     }

//     private void UpdateConsumptionChart(List<string> months, List<float> consumptions)
//     {
//         if (consumptionChart == null)
//         {
//             Debug.LogError("Consumption Chart is not assigned in the Inspector.");
//             return;
//         }

//         consumptionChart.ClearData();
//         consumptionChart.AddSerie<Bar>();

//         for (int i = 0; i < months.Count; i++)
//         {
//             consumptionChart.AddXAxisData(months[i]);
//             consumptionChart.AddData(0, consumptions[i]);
//         }
//     }

//     private void UpdateCostChart(List<string> months, List<float> costs)
//     {
//         if (costChart == null)
//         {
//             Debug.LogError("Cost Chart is not assigned in the Inspector.");
//             return;
//         }

//         costChart.ClearData();
//         costChart.AddSerie<Line>();

//         for (int i = 0; i < months.Count; i++)
//         {
//             costChart.AddXAxisData(months[i]);
//             costChart.AddData(0, costs[i]);
//         }
//     }

//     public void ClearLatestBill()
//     {
//         DateText.text = "";
//         ConsumptionText.text = "";
//         CostText.text = "";
//         Debug.Log("Latest bill information cleared.");
//     }
// }



// W/ CHARTS AND PREDICTIVE CHARTS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq;
using XCharts;
using XCharts.Runtime;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;
using System.IO;

public class Analytics : MonoBehaviour
{
    // private string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api"; 
    private string baseUrl = "http://localhost:5000/api";
    public BarChart predictedAllConsumptionChart;
    public LineChart predictedAllCostChart;
    public TMP_Text DateText;
    public TMP_Text ConsumptionText;
    public TMP_Text CostText;

    public BarChart consumptionChart;
    public LineChart costChart;
    public BarChart predictedConsumptionChart;
    public LineChart predictedCostChart;

    public GameObject LoadingScreen;

    public Button AnalyticsButton;

    private float predictedMonthlyCostKo;
    private float predictedMonthlyConsumptionKo;
    private string monthKo;

    void Start()
    {
        AnalyticsButton.onClick.AddListener(() =>
        {
            StartCoroutine(FetchLatestBill());
            StartCoroutine(FetchMonthlyConsumption());
            StartCoroutine(FetchMonthlyCost());
            StartCoroutine(FetchPredictedMonthlyConsumption());
            StartCoroutine(FetchPredictedMonthlyCost());
            // StartCoroutine(SavePredictedData(PredictedData.month, PredictedData.consumption, PredictedData.cost));
        });

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

    public IEnumerator FetchLatestBill()
    {
        string jwtToken = GetJWTToken();
        if (string.IsNullOrEmpty(jwtToken)) yield break;

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/waterBill/latest"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Latest Bill Data: " + jsonResponse);

                JObject latestBill = JObject.Parse(jsonResponse);
                if (latestBill.ContainsKey("message") && latestBill["message"].ToString() == "No bills found")
                {
                    Debug.LogWarning("No bills found for this user.");
                }
                else
                {
                    // Convert ISO date format to readable format
                    DateTime billDate = DateTime.Parse(latestBill["billDate"]?.ToString() ?? DateTime.MinValue.ToString());
                    string formattedDate = billDate.ToString("dd MMM yyyy"); // Example: "04 Jan 2025"

                    DateText.text = formattedDate;
                    ConsumptionText.text = latestBill["waterConsumption"]?.ToString() + " m³" ?? "N/A";
                    CostText.text = (latestBill["billAmount"]?.ToString() ?? "N/A");
                }
            }
            else
            {
                Debug.LogError($"Failed to fetch latest bill. HTTP Code: {request.responseCode}, Error: {request.error}");
            }
        }
    }

    // OG
    public IEnumerator FetchMonthlyConsumption()
    {
        string jwtToken = GetJWTToken();
        if (string.IsNullOrEmpty(jwtToken)) yield break;

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/chart/monthly-consumption"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Monthly Consumption Data: " + jsonResponse);

                JArray consumptionData = JArray.Parse(jsonResponse);
                List<string> months = new List<string>();
                List<float> consumptions = new List<float>();

                foreach (JObject entry in consumptionData)
                {
                    months.Add(entry["_id"].ToString());
                    consumptions.Add(float.Parse(entry["totalConsumption"].ToString()));
                }

                UpdateConsumptionChart(months, consumptions);
                PredictNextMonth(months, consumptions, "consumption");

            }
            else
            {
                Debug.LogError($"Failed to fetch monthly consumption. Error: {request.error}");
            }
        }
    }

    // OG
    public IEnumerator FetchMonthlyCost()
    {
        string jwtToken = GetJWTToken();
        if (string.IsNullOrEmpty(jwtToken)) yield break;

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/chart/monthly-cost"))
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
                    months.Add(entry["_id"].ToString());
                    costs.Add(float.Parse(entry["totalCost"].ToString()));
                }

                UpdateCostChart(months, costs);
                PredictNextMonth(months, costs, "cost");
            }
            else
            {
                Debug.LogError($"Failed to fetch monthly cost. Error: {request.error}");
            }
        }
    }

    private void UpdateConsumptionChart(List<string> months, List<float> consumptions)
    {
        if (consumptionChart == null)
        {
            Debug.LogError("Consumption Chart is not assigned in the Inspector.");
            return;
        }

        consumptionChart.ClearData();
        consumptionChart.AddSerie<Bar>();

        for (int i = 0; i < months.Count; i++)
        {
            consumptionChart.AddXAxisData(months[i]);
            consumptionChart.AddData(0, consumptions[i]);
        }
    }

    private void UpdateCostChart(List<string> months, List<float> costs)
    {
        if (costChart == null)
        {
            Debug.LogError("Cost Chart is not assigned in the Inspector.");
            return;
        }

        costChart.ClearData();
        costChart.AddSerie<Line>();

        for (int i = 0; i < months.Count; i++)
        {
            costChart.AddXAxisData(months[i]);
            costChart.AddData(0, costs[i]);
        }
    }

      // TRIAL
    public IEnumerator FetchPredictedMonthlyConsumption()
    {
        string jwtToken = GetJWTToken();
        if (string.IsNullOrEmpty(jwtToken)) yield break;

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/chart/predicted-monthly-consumption"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Predicted Monthly Consumption Data: " + jsonResponse);

                JArray predictedConsumptionData = JArray.Parse(jsonResponse);
                List<string> months = new List<string>();
                List<float> predictedConsumptions = new List<float>();

                foreach (JObject entry in predictedConsumptionData)
                {
                    months.Add(entry["_id"].ToString());
                    predictedConsumptions.Add(float.Parse(entry["predictedConsumption"].ToString()));
                }

                UpdatePredictedConsumptionChart(months, predictedConsumptions);
            }
            else
            {
                Debug.LogError($"Failed to fetch predicted monthly consumption. Error: {request.error}");
            }
        }
    }

    public IEnumerator FetchPredictedMonthlyCost()
    {
        string jwtToken = GetJWTToken();
        if (string.IsNullOrEmpty(jwtToken)) yield break;

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/chart/predicted-monthly-cost"))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Predicted Monthly Cost Data: " + jsonResponse);

                JArray predictedCostData = JArray.Parse(jsonResponse);
                List<string> months = new List<string>();
                List<float> predictedCosts = new List<float>();

                foreach (JObject entry in predictedCostData)
                {
                    months.Add(entry["_id"].ToString());
                    predictedCosts.Add(float.Parse(entry["predictedCost"].ToString()));
                }

                UpdatePredictedCostChart(months, predictedCosts);
            }
            else
            {
                Debug.LogError($"Failed to fetch predicted monthly cost. Error: {request.error}");
            }
        }
    }

    private void UpdatePredictedConsumptionChart(List<string> months, List<float> predictedConsumptions)
    {
        if (predictedAllConsumptionChart == null)
        {
            Debug.LogError("Predicted Consumption Chart is not assigned in the Inspector.");
            return;
        }

        predictedAllConsumptionChart.ClearData();
        predictedAllConsumptionChart.AddSerie<Bar>();

        for (int i = 0; i < months.Count; i++)
        {
            predictedAllConsumptionChart.AddXAxisData(months[i]);
            predictedAllConsumptionChart.AddData(0, predictedConsumptions[i]);
        }
    }

    private void UpdatePredictedCostChart(List<string> months, List<float> predictedCosts)
    {
        if (predictedAllCostChart == null)
        {
            Debug.LogError("Predicted Cost Chart is not assigned in the Inspector.");
            return;
        }

        predictedAllCostChart.ClearData();
        predictedAllCostChart.AddSerie<Line>();

        for (int i = 0; i < months.Count; i++)
        {
            predictedAllCostChart.AddXAxisData(months[i]);
            predictedAllCostChart.AddData(0, predictedCosts[i]);
        }
    }





    public void PredictNextMonth(List<string> months, List<float> values, string type)
    {
        if (values.Count < 2) return;

        int lastIndex = values.Count - 1;
        float previous = values[lastIndex - 1];
        float current = values[lastIndex];

        // Simple Linear Regression for Prediction
        float predictedValue = current + (current - previous);

        string nextMonth = GetNextMonth(months[lastIndex]);

        Debug.Log($"Predicted {type} for {nextMonth}: {predictedValue}");
        PredictedData.month = nextMonth;


        if (type == "consumption")
        {
            UpdatePredictionBarChart(predictedConsumptionChart, nextMonth, predictedValue);
            PredictedData.consumption = predictedValue;
            return;
            // SavePredictedData(nextMonth, predictedConsumption, predictedValue);

        }
        else if (type == "cost")
        {
            UpdatePredictionLineChart(predictedCostChart, nextMonth, predictedValue);
            PredictedData.cost = predictedValue;
            // return ;

            // SavePredictedData(month, predictedConsumption, predictedAmount);
        }
        StartCoroutine(SavePredictedData(PredictedData.month, PredictedData.consumption, PredictedData.cost));


    }

    public static string GetNextMonth(string lastMonth)
    {
        DateTime lastMonthDate;

        // Try parsing the input date in "yyyy-MM-dd" format
        if (!DateTime.TryParseExact(lastMonth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastMonthDate))
        {
            Debug.LogError($"Failed to parse date: {lastMonth}");
            return "Invalid Date";
        }

        // Get the next month's date
        DateTime nextMonthDate = lastMonthDate.AddMonths(1);

        // Return formatted string in "yyyy-MM-dd"
        return nextMonthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public void UpdatePredictionBarChart(BarChart chart, string nextMonth, float predictedValue)
    {
        if (chart == null)
        {
            Debug.LogError("Prediction Bar Chart is not assigned in the Inspector.");
            return;
        }

        chart.ClearData();
        chart.AddSerie<Bar>();
        chart.AddXAxisData(nextMonth);
        chart.AddData(0, predictedValue);
    }

    public void UpdatePredictionLineChart(LineChart chart, string nextMonth, float predictedValue)
    {
        if (chart == null)
        {
            Debug.LogError("Prediction Line Chart is not assigned in the Inspector.");
            return;
        }

        chart.ClearData();
        chart.AddSerie<Line>();
        chart.AddXAxisData(nextMonth);
        chart.AddData(0, predictedValue);
    }

    public void ClearLatestBill()
    {
        DateText.text = "";
        ConsumptionText.text = "";
        CostText.text = "";
        Debug.Log("Latest bill information cleared.");
    }

    public void ClearCharts()
    {
        // Clear the consumption chart
        if (consumptionChart != null)
        {
            consumptionChart.ClearData();
        }

        // Clear the cost chart
        if (costChart != null)
        {
            costChart.ClearData();
        }

        // Clear the predicted consumption chart
        if (predictedConsumptionChart != null)
        {
            predictedConsumptionChart.ClearData();
        }

        // Clear the predicted cost chart
        if (predictedCostChart != null)
        {
            predictedCostChart.ClearData();
        }
    }

    


    // OG FROM GITHUB
    // public IEnumerator SavePredictedData(string month, float predictedConsumption, float predictedAmount)
    // {
    //     Debug.Log($"🔍 Checking existing predictions for {month}...");

    //     // Step 1: Get existing predictions before saving
    //     // string jwtToken = GetJWTToken();


    //     string folderpath = Application.dataPath;
    //     string savePath = Path.Combine(folderpath, "UserData");
    //     string userDataPath = Path.Combine(savePath, "userInfo.json");
    //     string jsonContent = File.ReadAllText(userDataPath);
    //     JObject PlayerId = JObject.Parse(jsonContent);
    //     string userid = PlayerId["userId"]?.ToString();
    //     string jwtToken = PlayerId["token"]?.ToString();
    //     Debug.Log($"📦 Saving new prediction for {month}...");

    //     JObject predictedData = new JObject
    //         {
    //             {"user", userid},
    //             { "predictedAmount", predictedAmount },
    //             { "predictedConsumption", predictedConsumption },
    //             { "predictedMonth", month },



    //         };

    //     string jsonPayload = predictedData.ToString();
    //     byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

    //     using (UnityWebRequest request = new UnityWebRequest("https://aqua-quest-backend-deployment.onrender.com/api/chart/save-prediction", "POST"))
    //     {
    //         request.SetRequestHeader("Content-Type", "application/json");
    //         request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
    //         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //         request.downloadHandler = new DownloadHandlerBuffer();
    //         request.method = UnityWebRequest.kHttpVerbPOST;

    //         Debug.Log("⏳ Sending HTTP request...");
    //         yield return request.SendWebRequest();

    //         if (request.result == UnityWebRequest.Result.Success)
    //         {
    //             Debug.Log($"✅ Predicted data saved successfully! Response: {request.downloadHandler.text}");
    //         }
    //         else
    //         {
    //             Debug.LogError($"❌ Failed to save predicted data. HTTP Code: {request.responseCode}, Error: {request.error}");
    //             Debug.LogError($"Server Response: {request.downloadHandler.text}");
    //         }
    //     }
    // }

    // TRIAL
    public IEnumerator SavePredictedData(string month, float predictedConsumption, float predictedAmount)
    {
        Debug.Log($"🔍 Checking existing predictions for {month}...");

        // Step 1: Read user credentials from file
        string folderpath = UnityEngine.Application.persistentDataPath;
        
        string userDataPath = Path.Combine(folderpath, "userInfo.json");
        string jsonContent = File.ReadAllText(userDataPath);
        JObject PlayerId = JObject.Parse(jsonContent);
        string userid = PlayerId["userId"]?.ToString();
        string jwtToken = PlayerId["token"]?.ToString();

        if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("❌ User ID or JWT token is missing.");
            yield break;
        }

        // Step 2: Check if prediction for the same month already exists
        using (UnityWebRequest checkRequest = UnityWebRequest.Get($"{baseUrl}/predicted-cost"))
        {
            checkRequest.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            yield return checkRequest.SendWebRequest();

            if (checkRequest.result == UnityWebRequest.Result.Success)
            {
                JArray predictions = JArray.Parse(checkRequest.downloadHandler.text);
                foreach (JObject prediction in predictions)
                {
                    if (prediction["predictedMonth"]?.ToString() == month)
                    {
                        Debug.Log($"⚠️ Prediction for {month} already exists. Skipping save.");
                        yield break;
                    }
                }
            }
            // else
            // {
            //     Debug.LogError($"⚠️ Failed to fetch predictions. HTTP Code: {checkRequest.responseCode}, Error: {checkRequest.error}");
            // }
        }

        // Step 3: Save new prediction if no existing entry for the month
        Debug.Log($"📦 Saving new prediction for {month}...");

        JObject predictedData = new JObject
        {
            { "user", userid },
            { "predictedAmount", predictedAmount },
            { "predictedConsumption", predictedConsumption },
            { "predictedMonth", month }
        };

        string jsonPayload = predictedData.ToString();
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/chart/save-prediction", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.method = UnityWebRequest.kHttpVerbPOST;

            Debug.Log("⏳ Sending HTTP request...");
            LoadingScreen.SetActive(true);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                LoadingScreen.SetActive(false);
                Debug.Log($"✅ Predicted data saved successfully! Response: {request.downloadHandler.text}");
                StartCoroutine(FetchPredictedMonthlyConsumption());
                StartCoroutine(FetchPredictedMonthlyCost());
            }
            else
            {
                LoadingScreen.SetActive(false);
                Debug.LogError($"❌ Failed to save predicted data. HTTP Code: {request.responseCode}, Error: {request.error}");
                Debug.LogError($"Server Response: {request.downloadHandler.text}");
            }
        }
    }


    

}

