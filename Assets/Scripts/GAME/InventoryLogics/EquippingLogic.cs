using System;
using System.IO;
using UnityEngine;
using TMPro;

public class EquippingLogic : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Attribute;
    public TMP_Text AttrEffect;
    public TMP_Text EquipDisplay;

    private string savePath;

    // Base stats of the player
    private const int BASE_HEALTH = 200;
    private const int BASE_DAMAGE = 20;
    private const int BASE_DEFENSE = 0;
    private const float BASE_SPEED = 10f;

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
    }

    public void ExtractDatas()
    {
        if (string.IsNullOrEmpty(Name.text) || string.IsNullOrEmpty(Attribute.text) || string.IsNullOrEmpty(AttrEffect.text))
        {
            Debug.LogError("Missing data in EquippingLogic. Make sure Name, Attribute, and AttrEffect are set.");
            return;
        }

        // Load existing data or create a new one if the file doesn't exist
        PlayerStats playerStats = LoadData();

        bool itemRemoved = false;

        // Check if the item already exists in relics
        switch (Attribute.text)
        {
            case "Health":
                if (playerStats.Relics.Health != null && playerStats.Relics.Health.Name == Name.text)
                {
                    playerStats.Relics.Health = null;
                    itemRemoved = true;
                    
                }
                else
                {
                    playerStats.Relics.Health = new StatData { Name = Name.text, ItemEffect = SafeParseInt(AttrEffect.text) };
                    
                }
                break;

            case "Damage":
                if (playerStats.Relics.Damage != null && playerStats.Relics.Damage.Name == Name.text)
                {
                    playerStats.Relics.Damage = null;
                    itemRemoved = true;
                    
                }
                else
                {
                    playerStats.Relics.Damage = new StatData { Name = Name.text, ItemEffect = SafeParseInt(AttrEffect.text) };
                   
                }
                break;

            case "Defense":
                if (playerStats.Relics.Defense != null && playerStats.Relics.Defense.Name == Name.text)
                {
                    playerStats.Relics.Defense = null;
                    itemRemoved = true;
                    
                }
                else
                {
                    playerStats.Relics.Defense = new StatData { Name = Name.text, ItemEffect = SafeParseInt(AttrEffect.text) };
                    
                }
                break;

            case "Speed":
                if (playerStats.Relics.Speed != null && playerStats.Relics.Speed.Name == Name.text)
                {
                    playerStats.Relics.Speed = null;
                    itemRemoved = true;
                    
                }
                else
                {
                    playerStats.Relics.Speed = new SpeedStatData { Name = Name.text, ItemEffect = SafeParseFloat(AttrEffect.text) };
                   
                }
                break;

            default:
                Debug.LogWarning("Unknown attribute: " + Attribute.text);
                return;
        }

        // Recalculate player attributes and power level
        playerStats.PlayerAttributes = CalculatePlayerAttributes(playerStats.Relics);

        // Save updated data
        SaveData(playerStats);

        Debug.Log(itemRemoved ? $"Removed {Name.text} from {Attribute.text}" : $"Equipped {Name.text} to {Attribute.text}");
    }

    private PlayerAttributes CalculatePlayerAttributes(RelicStats relics)
    {
        // Calculate total values (base + item effects)
        int totalHealth = BASE_HEALTH + (relics.Health?.ItemEffect ?? 0);
        int totalDamage = BASE_DAMAGE + (relics.Damage?.ItemEffect ?? 0);
        int totalDefense = BASE_DEFENSE + (relics.Defense?.ItemEffect ?? 0);
        float totalSpeed = BASE_SPEED + (relics.Speed?.ItemEffect ?? 0f);

        // Power level calculation (customizable formula)
        float calculatedPower = (totalHealth * 1.5f) + (totalDamage * 2f) + (totalDefense * 1.8f) + (totalSpeed * 3f);

        // Return the updated attributes
        return new PlayerAttributes
        {
            TotalHealth = totalHealth,
            TotalDamage = totalDamage,
            TotalDefense = totalDefense,
            TotalSpeed = totalSpeed,
            CalculatedPowerLevel = calculatedPower
        };
    }

    private void SaveData(PlayerStats data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Player Stats saved at: " + savePath);
    }

    private PlayerStats LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerStats loadedData = JsonUtility.FromJson<PlayerStats>(json);
            return loadedData ?? new PlayerStats(); // Ensure we never return null
        }
        return new PlayerStats();
    }

    private int SafeParseInt(string value)
    {
        if (int.TryParse(value, out int result))
            return result;
        Debug.LogWarning("Invalid integer value: " + value);
        return 0;
    }

    private float SafeParseFloat(string value)
    {
        if (float.TryParse(value, out float result))
            return result;
        Debug.LogWarning("Invalid float value: " + value);
        return 0f;
    }
}
