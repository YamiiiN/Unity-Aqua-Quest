using UnityEngine;
using System.IO;


public class InventoryLogics
{
    private string saveFilePath;
    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerDataInventory.json");
    }

    // public void SaveData(GameItems[] Relics)
    // {
    //     PlayerData data = new PlayerData();
    //     data.Relics = new string[Relics.Length];

    //     for (int i = 0; i < Relics.Length; i++)
    //     {
    //         data.Relics[i] = Relics[i].itemName; // Store item names
    //     }

    //     string json = JsonUtility.ToJson(data, true);

    //     try
    //     {
    //         File.WriteAllText(saveFilePath, json);
    //         Debug.Log("Game Saved at: " + saveFilePath);
    //     }
    //     catch (System.Exception e)
    //     {
    //         Debug.LogError("Save Failed: " + e.Message);
    //     }
    // }
}
