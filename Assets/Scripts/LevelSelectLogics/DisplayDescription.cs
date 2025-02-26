using System;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Use Unity's Button instead of Windows Forms

public class DisplayDescription : MonoBehaviour
{
    public GameObject DescriptionPanel;
    public TMP_Text DescriptionText;
    public TMP_Text PowerlevelReq;
    public LevelDetails Level;
    public GameObject NotifPanel, PowerInfo;
    public Button PlayButton; // Corrected to use UnityEngine.UI.Button

    private string savePath;

    // Base stats if file does not exist
    private const int BASE_HEALTH = 200;
    private const int BASE_DAMAGE = 20;
    private const int BASE_DEFENSE = 0;
    private const float BASE_SPEED = 10f;

    private void Start()
    {
        savePath = Path.Combine(UnityEngine.Application.persistentDataPath, "PlayerStats.json"); // Fixed ambiguity

        // Ensure player stats exist
        if (!File.Exists(savePath))
        {
            CreateBaseStats();
        }

        // Bind PlayButton to the eligibility check
        PlayButton.onClick.AddListener(CheckEligibility);
    }

    public void Display()
    {
        DescriptionPanel.SetActive(true);
        PowerInfo.SetActive(true);
        DescriptionText.text = Level.Story;
        Debug.Log(Level.Story);
        PowerlevelReq.text = Level.PowerLevel.ToString();
    }

    private void CheckEligibility()
    {
        if (!IsPlayerEligible())
        {
            NotifPanel.SetActive(true);
            Debug.Log("Player does not meet power level requirement.");
        }
        else
        {
            NotifPanel.SetActive(false);
            Debug.Log("Player is eligible to play the level.");
            // Add logic to start the level if needed
        }
    }

    private bool IsPlayerEligible()
    {
        PlayerStats playerStats = LoadData();
        return playerStats.PlayerAttributes.CalculatedPowerLevel >= Level.PowerLevel;
    }

    private void CreateBaseStats()
    {
        PlayerStats baseStats = new PlayerStats
        {
            PlayerAttributes = CalculatePlayerAttributes(new RelicStats()) 
        };

        SaveData(baseStats);
        Debug.Log("Base player stats created.");
    }

    private PlayerAttributes CalculatePlayerAttributes(RelicStats relics)
    {
        int totalHealth = BASE_HEALTH + (relics.Health?.ItemEffect ?? 0);
        int totalDamage = BASE_DAMAGE + (relics.Damage?.ItemEffect ?? 0);
        int totalDefense = BASE_DEFENSE + (relics.Defense?.ItemEffect ?? 0);
        float totalSpeed = BASE_SPEED + (relics.Speed?.ItemEffect ?? 0f);

        float calculatedPower = (totalHealth * 1.5f) + (totalDamage * 2f) + (totalDefense * 1.8f) + (totalSpeed * 3f);

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
        Debug.Log("Player stats saved.");
    }

    private PlayerStats LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerStats loadedData = JsonUtility.FromJson<PlayerStats>(json);
            return loadedData ?? new PlayerStats();
        }
        return new PlayerStats();
    }
}
