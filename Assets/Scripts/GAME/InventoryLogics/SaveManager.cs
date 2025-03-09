using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
// using Unity.Android.Gradle.Manifest;
[System.Serializable]
public class PlayerData
{
    public string[] Relics;
    public string[] Potions; // Save items as names
    public int Woins; 
}

public static class SaveManager
{
    private static readonly string SavePath = Path.Combine(UnityEngine.Application.persistentDataPath, "PlayerInventory.json");

    public static void SaveData(List<GameItems> items)
    {
        PlayerData data = new PlayerData(); 
        data.Relics = new string[items.Count];
        
        for (int i = 0; i < items.Count; i++)
        {
            data.Relics[i] = items[i].Name; // Store item names
        }

        string json = JsonUtility.ToJson(data, true); // Pretty format for easier debugging
        File.WriteAllText(SavePath, json);

        Debug.Log(SavePath);

        LoadData();
        // File.Delete(SavePath);
    }

    public static void SavePotionData(List<Potion> potions)
    {
        PlayerData existingData = LoadData() ?? new PlayerData(); // Load existing data or create new

        HashSet<string> potionSet = new HashSet<string>(existingData.Potions ?? new string[0]); // Avoid duplicates

        foreach (Potion potion in potions)
        {
            potionSet.Add(potion.Name); // HashSet ensures no duplicates
        }

        existingData.Potions = new string[potionSet.Count];
        potionSet.CopyTo(existingData.Potions); // Convert HashSet back to array

        string json = JsonUtility.ToJson(existingData, true); // Pretty format
        File.WriteAllText(SavePath, json); // Save updated data

        Debug.Log($"Potion data saved to {SavePath}");

        LoadData();
        // File.Delete(SavePath);
    }

    public static PlayerData LoadData()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            Debug.Log("Loaded Inventory: " + json);
            Debug.Log(SavePath);
            // File.Delete(SavePath);

            return JsonUtility.FromJson<PlayerData>(json);
        }

        Debug.LogWarning("No save file found. Returning null.");
        return null;
    }

    public static void addDefaultWoins()
    {
        // 
        string json = File.ReadAllText(SavePath);
        PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);

        if (playerData == null)
        {
            Debug.LogError("Failed to deserialize PlayerData!");
            return;
        }

        playerData.Woins = 200; // Update only Woins
        File.WriteAllText(SavePath, JsonConvert.SerializeObject(playerData, Formatting.Indented));
        // Debug.Log($"Updated Woins: {newWoins}");
    }



}
