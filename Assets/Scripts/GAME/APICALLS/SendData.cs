using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;

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
    public static async void SendGameData()
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
        await SendToAPI(jsonData, token, userId);
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


    public static async void GetPlayerData()
    {
        JObject playerID = GetPlayer();

        if (playerID == null)
        {
            Debug.LogError("Failed to retrieve user data.");
            return;
        }

        string userId = playerID["userId"]?.ToString();
        

        string apiUrl = GetStatFile + userId;

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            // request.SetRequestHeader("Authorization", "Bearer " + token); // Send JWT token in headers

            // Await the request
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Player data retrieved successfully: " + request.downloadHandler.text);
                // Handle the response data here
            }
            else
            {
                Debug.LogError("Error retrieving player data: " + request.error);
            }
        }
    }
}