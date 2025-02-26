using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameItemsDatabase Health, Damage, Defense, Speed;
    public PotionDatabase Potion;
    public GameObject gridItemPrefab;
    public Transform HealthGridParent, DamageGridParent, DefenseGridParent, SpeedGridParent, PotionGridParent;
    public DisplayTarget displayTarget; // Reference to the external object
    public GameObject targeting;

    void Start()
    {
        spawnLogic();
        SpawnPotionLogic();
    }

    void spawnLogic()
    {
        SpawnItems(Health, HealthGridParent);
        SpawnItems(Damage, DamageGridParent);
        SpawnItems(Defense, DefenseGridParent);
        SpawnItems(Speed, SpeedGridParent);
    }

    public void SpawnPotionLogic()
    {
        SpawnPotion(Potion, PotionGridParent);
    }
    void SpawnPotion(PotionDatabase database, Transform parent)
    {
        foreach (Potion item in database.Potions)
        {
            GameObject gridItem = Instantiate(gridItemPrefab, parent);
            ContentDisplay display = gridItem.GetComponent<ContentDisplay>();

            if (display != null)
            {
                display.potionSetup(item, displayTarget, targeting); // Pass external object reference
            }
        }
    }

    void SpawnItems(GameItemsDatabase database, Transform parent)
    {
        foreach (GameItems item in database.Items)
        {
            GameObject gridItem = Instantiate(gridItemPrefab, parent);
            ContentDisplay display = gridItem.GetComponent<ContentDisplay>();

            if (display != null)
            {
                display.Setup(item, displayTarget, targeting); // Pass external object reference
            }
        }
    }
    
}
