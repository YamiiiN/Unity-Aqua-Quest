using UnityEngine;
using System.IO;
public class Static
{
    public class Links
    {
        // non prod url
        public static string baseUrl = "http://localhost:5000/api/gamestat/";
        public static string PlayerStatsPath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
        public static string PlayerInventory = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");

        private static string folderpath = Application.dataPath;

        private static string savePath = Path.Combine(folderpath, "UserData");
        public static string userDataPath = Path.Combine(savePath, "userInfo.json");



    }
}
