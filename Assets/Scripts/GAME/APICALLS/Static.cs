using UnityEngine;
using System.IO;
public class Static
{
    public class Links
    {
        // non prod url
        public static string baseUrl = "https://aqua-quest-backend-deployment.onrender.com/api/gamestat/";
        public static string PlayerStatsPath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
        public static string PlayerInventory = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");

        public static string userDataPath = Path.Combine(Application.persistentDataPath, "userInfo.json");




    }

    public class FetchData
    {
        public static  PlayerData PlayerInventory { get; private set; }
        public static  PlayerStats PlayerStats { get; private set; }
        public static void SetData(PlayerData inventory, PlayerStats stats)
        {
            PlayerInventory = inventory;
            PlayerStats = stats;
        }
    }
}
