using System.Collections.Generic;
// using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    public GameItemsDatabase startGameItemsDatabase;
    public PotionDatabase startGamePotionDatabase; // Reference to the starting items
    private List<GameItems> playerInventory = new List<GameItems>();
    
    private List<Potion> PlayerInventoryPotion = new List<Potion>(); // Player's inventory
    public Button AttrButtonH, AttrButtonD,AttrButtonDef,AttrButtonS, RelicRefresh, PotionClick;
    // public GameObject Grid;
    public DisplayTarget target;
    public Transform Grid;

    public GameObject Cell, TargetPanel, equipPanel;
    // public Button itemButton;
    void Start()
    {
        LoadPlayerInventory();

        // LoadPlayerInventoryPotions();
        AttrButtonD.onClick.RemoveAllListeners();
        AttrButtonH.onClick.RemoveAllListeners();
        AttrButtonDef.onClick.RemoveAllListeners();
        AttrButtonS.onClick.RemoveAllListeners();

        
        
        RelicRefresh.onClick.AddListener(() => 
            {
                foreach (Transform child in Grid)
                {
                    Destroy(child.gameObject); // Remove old items
                }
                LoadPlayerInventory();
                equipPanel.SetActive(true);
            }
        );
        AttrButtonD.onClick.AddListener(() => 
        {
            PlayerData data = SaveManager.LoadData();
            equipPanel.SetActive(true);
            if (data != null && data.Relics.Length > 0)
            {
                 foreach (Transform child in Grid)
                {
                    Destroy(child.gameObject); // Remove old items
                }
                // playerInventory = new List<GameItems>();

                foreach (string itemName in data.Relics)
                {
                    GameItems item = Resources.Load<GameItems>($"GameItemsScriptable/Relics/{itemName}");
                    if (item.Effect != "Damage")
                    {
                        
                    }
                    else{
                        GameObject cellItem = Instantiate(Cell, Grid);
                        Display display = cellItem.GetComponent<Display>();
                        if (display != null)
                        {
                            display.Setup(item, target, TargetPanel);
                        }
                    }
                }
            }
        }
        
        );
        AttrButtonH.onClick.AddListener(() => {
            equipPanel.SetActive(true);
            PlayerData data = SaveManager.LoadData();
            if (data != null && data.Relics.Length > 0)
            {
                 foreach (Transform child in Grid)
                {
                    Destroy(child.gameObject); // Remove old items
                }
                // playerInventory = new List<GameItems>();

                foreach (string itemName in data.Relics)
                {
                    GameItems item = Resources.Load<GameItems>($"GameItemsScriptable/Relics/{itemName}");
                    if (item.Effect != "Health")
                    {
                        
                    }
                    else{
                        GameObject cellItem = Instantiate(Cell, Grid);
                        Display display = cellItem.GetComponent<Display>();
                        if (display != null)
                        {
                            display.Setup(item, target, TargetPanel);
                        }
                    }
                }
            }
        });
        AttrButtonDef.onClick.AddListener(() => {
            equipPanel.SetActive(true);
            PlayerData data = SaveManager.LoadData();
            if (data != null && data.Relics.Length > 0)
            {
                 foreach (Transform child in Grid)
                {
                    Destroy(child.gameObject); // Remove old items
                }
                // playerInventory = new List<GameItems>();

                foreach (string itemName in data.Relics)
                {
                    GameItems item = Resources.Load<GameItems>($"GameItemsScriptable/Relics/{itemName}");
                    if (item.Effect != "Defense")
                    {
                        
                    }
                    else{
                        GameObject cellItem = Instantiate(Cell, Grid);
                        Display display = cellItem.GetComponent<Display>();
                        if (display != null)
                        {
                            display.Setup(item, target, TargetPanel);
                        }
                    }
                }
            }
        });
        AttrButtonS.onClick.AddListener(() => {
            PlayerData data = SaveManager.LoadData();
            equipPanel.SetActive(true);
            if (data != null && data.Relics.Length > 0)
            {
                 foreach (Transform child in Grid)
                {
                    Destroy(child.gameObject); // Remove old items
                }
                // playerInventory = new List<GameItems>();

                foreach (string itemName in data.Relics)
                {
                    GameItems item = Resources.Load<GameItems>($"GameItemsScriptable/Relics/{itemName}");
                    if (item.Effect != "Speed")
                    {
                        
                    }
                    else{
                        GameObject cellItem = Instantiate(Cell, Grid);
                        Display display = cellItem.GetComponent<Display>();
                        if (display != null)
                        {
                            display.Setup(item, target, TargetPanel);
                        }
                    }
                }
            }
        });

        PotionClick.onClick.AddListener(() =>
        {
             foreach (Transform child in Grid)
                {
                    Destroy(child.gameObject); // Remove old items
                }
            equipPanel.SetActive(false);
            
            LoadPlayerInventoryPotions();
        });
    }

    private void LoadPlayerInventory()
    {
        PlayerData data = SaveManager.LoadData();
        if (data != null && data.Relics.Length > 0)
        {
            playerInventory = new List<GameItems>();

            foreach (string itemName in data.Relics)
            {
                GameItems item = Resources.Load<GameItems>($"GameItemsScriptable/Relics/{itemName}");
                if (item != null)
                {
                    playerInventory.Add(item);
                    Debug.Log("Loaded item: " + item.Name);
                }
                
                GameObject cellItem = Instantiate(Cell, Grid);
                Display display = cellItem.GetComponent<Display>();
                if (display != null)
                {
                    display.Setup(item, target, TargetPanel);
                }
                
            }
        }
        else
        {
            AssignStartingItems();
             // No saved data, assign starting items
        }

        SaveManager.addDefaultWoins();
    }

    public void LoadPlayerInventoryPotions()
    {   
        // Clear existing items
        foreach (Transform child in Grid)
        {
            Destroy(child.gameObject); // Remove old items
        }

        PlayerData data = SaveManager.LoadData();
        if (data != null && data.Potions.Length > 0)
        {
            PlayerInventoryPotion.Clear(); // Make sure list is cleared before adding new items

            foreach (string potionName in data.Potions)
            {
                Potion potion = Resources.Load<Potion>($"GameItemsScriptable/Potions/{potionName}");
                if (potion != null)
                {
                    PlayerInventoryPotion.Add(potion);
                    Debug.Log("Loaded potion: " + potion.Name);
                    
                    // Instantiate only if potion exists
                    GameObject cellItem = Instantiate(Cell, Grid);
                    Display display = cellItem.GetComponent<Display>();
                    if (display != null)
                    {
                        display.potionSetup(potion, target, TargetPanel);
                    }
                }
            }
        }
        else
        {
            AssignStartingPotions(); // No saved data, assign starting items
        }
        
    }


    private void AssignStartingItems()
    {
        if (startGameItemsDatabase != null)
        {
            playerInventory = new List<GameItems>(startGameItemsDatabase.Items); // Use the correct list
            SaveManager.SaveData(playerInventory); // Save starting items
        }
    }

    private void AssignStartingPotions()
    {
        if (startGamePotionDatabase != null)
        {
            PlayerInventoryPotion = new List<Potion>(startGamePotionDatabase.Potions); // Use the correct list
            SaveManager.SavePotionData(PlayerInventoryPotion); // Save starting items
        }
    }

    public void AttachItem(GameItems item)
    {
        if (!playerInventory.Contains(item))
        {
            playerInventory.Add(item);
            SaveManager.SaveData(playerInventory);
        }
    }

    public void AttachPotion(Potion potion)
    {
        if (!PlayerInventoryPotion.Contains(potion))
        {
            PlayerInventoryPotion.Add(potion);
            SaveManager.SavePotionData(PlayerInventoryPotion);
        }
    }

    
}
