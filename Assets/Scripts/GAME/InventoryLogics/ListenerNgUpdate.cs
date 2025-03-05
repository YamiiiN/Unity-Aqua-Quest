using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class ListenerNgUpdate : MonoBehaviour
{
    public TMP_Text EquipDisplay;
    public TMP_Text itemNameText;
    public TMP_Text effectType; // Set dynamically to know which attribute to check
    public TMP_Text Woins;
    public Image healthRelicImage;
    public Image damageRelicImage;
    public Image defenseRelicImage;
    public Image speedRelicImage;
    public TMP_Text DamageDisplay, HealthDisplay, DefenseDisplay, SpeedDisplay, PowerDisplay;

    public GameItemsDatabase gameItemsDatabase; // Reference to the ScriptableObject
    // private static readonly string InvPath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
    private string savePath, woinsPath;

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
        if (!File.Exists(savePath))
        {
            CreateDefaultPlayerStatsFile();
            
            // addDefaultWoins();
        }
        
        woinsPath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
        
        // SaveManager.addWoins();
        UpdateEquipDisplay();
        UpdateRelicImages();
        UpdateStatusDisplay();
        FetchWoins(woinsPath);
    }

    void Update()
    {
        UpdateEquipDisplay();
        UpdateRelicImages();
        UpdateStatusDisplay();
    }

    private void FetchWoins(string patt)
    {
        
            string json = File.ReadAllText(patt);
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);
            int playerWoins = data.Woins;
            Woins.text = playerWoins.ToString();
            Debug.Log("Fetched Woins: " + playerWoins);
        
    }
    private void CreateDefaultPlayerStatsFile()
{
    // Create a default PlayerStats object
    PlayerStats defaultStats = new PlayerStats
    {
        PlayerAttributes = new PlayerAttributes
        {
            TotalHealth = 200,
            TotalDamage = 20,
            TotalDefense = 0,
            TotalSpeed = 10f,
            CalculatedPowerLevel = 0
        },
        Relics = new RelicStats
        {
            Health = null,
            Damage = null,
            Defense = null,
            Speed = null
        }
    };

    // Convert to JSON and write to the file
    string json = JsonUtility.ToJson(defaultStats, true);
    File.WriteAllText(savePath, json);
}
    

    private void UpdateStatusDisplay()
    {
        if (!File.Exists(savePath))
        {
            // ResetStatusDisplay();
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerStats playerStats = JsonUtility.FromJson<PlayerStats>(json);

        // Update UI text fields with the player's stats
        HealthDisplay.text = $"{playerStats.PlayerAttributes.TotalHealth}";
        DamageDisplay.text = $"{playerStats.PlayerAttributes.TotalDamage}";
        DefenseDisplay.text = $"{playerStats.PlayerAttributes.TotalDefense}";
        SpeedDisplay.text = $"{playerStats.PlayerAttributes.TotalSpeed}";
        PowerDisplay.text = $"{playerStats.PlayerAttributes.CalculatedPowerLevel}";
    }

    private void UpdateEquipDisplay()
    {
        if (string.IsNullOrEmpty(itemNameText.text) || string.IsNullOrEmpty(effectType.text))
        {
            EquipDisplay.text = "Equip";
            return;
        }

        EquipDisplay.text = IsItemEquipped(itemNameText.text, effectType.text) ? "Unequip" : "Equip";
    }

    private bool IsItemEquipped(string itemName, string effectType)
    {
        if (!File.Exists(savePath)) return false;

        string json = File.ReadAllText(savePath);
        PlayerStats playerStats = JsonUtility.FromJson<PlayerStats>(json);

        switch (effectType)
        {
            case "Health":
                return playerStats.Relics.Health != null && playerStats.Relics.Health.Name == itemName;
            case "Damage":
                return playerStats.Relics.Damage != null && playerStats.Relics.Damage.Name == itemName;
            case "Defense":
                return playerStats.Relics.Defense != null && playerStats.Relics.Defense.Name == itemName;
            case "Speed":
                return playerStats.Relics.Speed != null && playerStats.Relics.Speed.Name == itemName;
            default:
                return false;
        }
    }

    private void UpdateRelicImages()
    {
        if (!File.Exists(savePath))
        {
            SetTransparentRelicImages();
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerStats playerStats = JsonUtility.FromJson<PlayerStats>(json);

        SetRelicImage(healthRelicImage, playerStats.Relics.Health?.Name);
        SetRelicImage(damageRelicImage, playerStats.Relics.Damage?.Name);
        SetRelicImage(defenseRelicImage, playerStats.Relics.Defense?.Name);
        SetRelicImage(speedRelicImage, playerStats.Relics.Speed?.Name);
    }

    private void SetRelicImage(Image image, string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); // Fully transparent
            return;
        }

        Sprite relicSprite = GetRelicSprite(itemName);
        if (relicSprite != null)
        {
            image.sprite = relicSprite;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f); // Fully visible
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); // Transparent if not found
        }
    }

    private Sprite GetRelicSprite(string itemName)
    {
        if (string.IsNullOrEmpty(itemName) || gameItemsDatabase == null) return null;

        foreach (var item in gameItemsDatabase.Items)
        {
            if (item.Name == itemName)
            {
                return item.Icon;
            }
        }
        return null;
    }

    private void SetTransparentRelicImages()
    {
        SetImageTransparent(healthRelicImage);
        SetImageTransparent(damageRelicImage);
        SetImageTransparent(defenseRelicImage);
        SetImageTransparent(speedRelicImage);
    }

    private void SetImageTransparent(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); // Fully transparent
    }
}
