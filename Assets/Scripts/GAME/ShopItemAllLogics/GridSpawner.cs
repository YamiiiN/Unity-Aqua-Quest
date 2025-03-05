using UnityEngine;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameItemsDatabase Health, Damage, Defense, Speed;
    public PotionDatabase Potion;
    public GameObject gridItemPrefab;
    public Transform HealthGridParent, DamageGridParent, DefenseGridParent, SpeedGridParent, PotionGridParent;
    public DisplayTarget displayTarget;
    public GameObject targeting;
    private string filePath;
    private string lastFileContent;
    public TMP_Text woinn;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");

        if (File.Exists(filePath))
        {
            lastFileContent = File.ReadAllText(filePath); // Store initial file content
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(lastFileContent);
            woinn.text = data.Woins.ToString();
        }

        SpawnAllItems();
    }

    void Update()
    {
        if (File.Exists(filePath))
        {
            string currentContent = File.ReadAllText(filePath);

            if (currentContent != lastFileContent) // Detect changes in the inventory file
            {
                Debug.Log("Inventory file changed! Reloading UI...");
                lastFileContent = currentContent;
                PlayerData data = JsonConvert.DeserializeObject<PlayerData>(lastFileContent);
                woinn.text = data.Woins.ToString();
                ReloadUI();
            }
        }
    }

    public void ReloadUI()
    {
        ClearGrid(HealthGridParent);
        ClearGrid(DamageGridParent);
        ClearGrid(DefenseGridParent);
        ClearGrid(SpeedGridParent);
        ClearGrid(PotionGridParent);
        SpawnAllItems();
    }

    private void SpawnAllItems()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Inventory file not found: " + filePath);
            return;
        }

        PlayerData inventory = LoadInventory();
        
        SpawnItems(Health, HealthGridParent, inventory.Relics);
        SpawnItems(Damage, DamageGridParent, inventory.Relics);
        SpawnItems(Defense, DefenseGridParent, inventory.Relics);
        SpawnItems(Speed, SpeedGridParent, inventory.Relics);
        SpawnPotions(Potion, PotionGridParent, inventory.Potions);
    }

    private PlayerData LoadInventory()
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<PlayerData>(json);
    }

    private void SpawnItems(GameItemsDatabase database, Transform parent, string[] ownedItems)
    {
        foreach (GameItems item in database.Items)
        {
            if (!ownedItems.Contains(item.Name)) // Show only unowned items
            {
                GameObject gridItem = Instantiate(gridItemPrefab, parent);
                ContentDisplay display = gridItem.GetComponent<ContentDisplay>();

                if (display != null)
                {
                    display.Setup(item, displayTarget, targeting);
                }
            }
        }
    }

    private void SpawnPotions(PotionDatabase database, Transform parent, string[] ownedPotions)
    {
        foreach (Potion item in database.Potions)
        {
            if (!ownedPotions.Contains(item.Name)) // Show only unowned potions
            {
                GameObject gridItem = Instantiate(gridItemPrefab, parent);
                ContentDisplay display = gridItem.GetComponent<ContentDisplay>();

                if (display != null)
                {
                    display.potionSetup(item, displayTarget, targeting);
                }
            }
        }
    }

    private void ClearGrid(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
