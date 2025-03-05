using UnityEngine;
using System.IO;

public static class GenerateFileAfterLogin
{
    private static string inventoryPath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
    private static string statsPath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");

    public static void SaveData()
    {
        if (Static.FetchData.PlayerInventory != null)
        {
            string inventoryJson = JsonUtility.ToJson(Static.FetchData.PlayerInventory, true);
            File.WriteAllText(inventoryPath, inventoryJson);
            Debug.Log("✅ Player Inventory saved to: " + inventoryPath);
        }
        else
        {
            Debug.LogError("❌ Player Inventory is null. Cannot save.");
        }

        if (Static.FetchData.PlayerStats != null)
        {
            string statsJson = JsonUtility.ToJson(Static.FetchData.PlayerStats, true);
            File.WriteAllText(statsPath, statsJson);
            Debug.Log("✅ Player Stats saved to: " + statsPath);
        }
        else
        {
            Debug.LogError("❌ Player Stats is null. Cannot save.");
        }
    }
}
