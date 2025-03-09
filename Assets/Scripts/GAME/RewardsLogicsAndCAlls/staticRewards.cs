using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;
using System;
using Newtonsoft.Json;
public class ReusableVar
{
        //  public static string baseUrl = "http://localhost:5000/api/rewards/";
        public static string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api/rewards/";
        public static string PlayerStatsPath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
        public static string PlayerInven = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");

        public static string userDataPath = Path.Combine(Application.persistentDataPath, "userInfo.json");
        static string jsonContent = File.ReadAllText(userDataPath);
        static JObject playerInfo = JObject.Parse(jsonContent);
        public static string userId = playerInfo["userId"]?.ToString();

        public static string InventoryKo= File.ReadAllText(PlayerInven);
        public static JObject InventoryNya = JObject.Parse(InventoryKo);
        public static JObject response;

        
        // public static string userId = 

}