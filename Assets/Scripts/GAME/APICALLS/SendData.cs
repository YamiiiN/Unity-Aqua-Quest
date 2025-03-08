using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;
using System;
using Newtonsoft.Json;

public class SendData
{
    // API Endpoints
    private static string CreateStatFile = Static.Links.baseUrl + "createStatFiles/"; // Adjust API endpoint as needed
    private static string GetStatFile = Static.Links.baseUrl + "getStatFiles/"; // Adjust API endpoint as needed

    /// <summary>
    /// Reads and returns the content of PlayerStats.json as a JObject.
    /// </summary>
    public static JObject GetPlayerStats()
    {
        if (!File.Exists(Static.Links.PlayerStatsPath))
        {
            Debug.LogError("PlayerStats.json not found!");
            return null;
        }

        string jsonContent = File.ReadAllText(Static.Links.PlayerStatsPath);
        return JObject.Parse(jsonContent);
    }

    /// <summary>
    /// Reads and returns the content of PlayerInventory.json as a JObject.
    /// </summary>
    public static JObject GetPlayerInventory()
    {
        if (!File.Exists(Static.Links.PlayerInventory))
        {
            Debug.LogError("PlayerInventory.json not found!");
            return null;
        }

        string jsonContent = File.ReadAllText(Static.Links.PlayerInventory);
        return JObject.Parse(jsonContent);
    }

    /// <summary>
    /// Reads and returns user information (userId and token).
    /// </summary>
    public static JObject GetPlayer()
    {
        if (!File.Exists(Static.Links.userDataPath))
        {
            Debug.LogError("userData.json not found!");
            return null;
        }

        string jsonContent = File.ReadAllText(Static.Links.userDataPath);
        return JObject.Parse(jsonContent);
    }

    /// <summary>
    /// Sends the JSON data (PlayerStats + PlayerInventory) to an API.
    /// </summary>
    public static async void SendGameData(GameObject LoadingScreen)
    {
        try
        {
            JObject playerStats = GetPlayerStats();
            JObject playerInventory = GetPlayerInventory();
            JObject playerID = GetPlayer();

            if (playerStats == null || playerInventory == null || playerID == null)
            {
            Debug.LogError("Failed to retrieve game data.");
            return;
            }

            string userId = playerID["userId"]?.ToString();
            string token = playerID["token"]?.ToString();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
            Debug.LogError("User authentication details are missing.");
            return;
            }

            JObject payload = new JObject
            {
            { "PlayerStats", playerStats },
            { "PlayerInventory", playerInventory }
            };

            string jsonData = payload.ToString();
            Debug.Log("Sending Game Data: " + jsonData);

            // Use async/await to send the API request
            LoadingScreen.SetActive(true);
            await SendToAPI(jsonData, token, userId);
            LoadingScreen.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogError($"An exception occurred while sending game data: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends JSON data to the API using UnityWebRequest with async/await.
    /// </summary>
    private static async Task SendToAPI(string jsonData, string token, string id)
    {
        string apiUrl = CreateStatFile + id; 

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            // request.SetRequestHeader("Authorization", "Bearer " + token); // Send JWT token in headers

            // Await the request
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Game data sent successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error sending game data: " + request.error);
            }
        }
    }


    public static async void GetPlayerData(GameObject LoadingScreen)
    {
        try
        {
            JObject playerID = GetPlayer();

            if (playerID == null || !playerID.ContainsKey("userId"))
            {
                Debug.LogError("Failed to retrieve user data. Invalid player ID.");
                return;
            }

            string userId = playerID["userId"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("User ID is null or empty.");
                return;
            }

            string apiUrl = GetStatFile + userId;

            using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
            {
                // request.SetRequestHeader("Authorization", "Bearer " + token); // Send JWT token in headers

                var operation = request.SendWebRequest();
                LoadingScreen.SetActive(true);
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    LoadingScreen.SetActive(false);
                    string jsonResponse = request.downloadHandler.text;

                    // Deserialize JSON correctly
                    PlayerDatas playerData = JsonConvert.DeserializeObject<PlayerDatas>(jsonResponse);

                    // Assign data to static class
                    Static.FetchData.SetData(playerData.PlayerInventory, playerData.PlayerStats);
                    Debug.Log(jsonResponse);
                    GenerateFileAfterLogin.SaveData();
                }
                else
                {
                    Debug.LogError($"Error retrieving player data: {request.result}, {request.error}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An exception occurred while retrieving player data: {ex.Message}");
        }
    }
}