using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using System.Linq; // Required for array operations

public class BuyLogic : MonoBehaviour
{
    public TMP_Text Price, Name, Woins;
    private string filepath;

    void Start()
    {
        filepath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");

        if (File.Exists(filepath))
        {
            string json = File.ReadAllText(filepath);
            PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);
            Woins.text = data.Woins.ToString();
        }
        else
        {
            Debug.LogError("Inventory file not found: " + filepath);
        }
    }

    public void BuyThis()
    {
        if (!File.Exists(filepath))
        {
            Debug.LogError("File not found: " + filepath);
            return;
        }

        // Load the current inventory data
        string json = File.ReadAllText(filepath);
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(json);

        int balance = data.Woins;
        int price = int.Parse(Price.text);
        string itemName = Name.text;

        // Check if the player can afford the item
        if (balance >= price)
        {
            balance -= price;  // Deduct price
            data.Woins = balance; // Update balance in data object

            // Add the item if not already owned
            if (!data.Relics.Contains(itemName))  
            {
                data.Relics = data.Relics.Append(itemName).ToArray(); // Append item to array
            }

            // Save the updated data back to the file
            string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filepath, updatedJson);

            // Update UI
            Woins.text = balance.ToString();
            Debug.Log("Purchase successful! Remaining Woins: " + balance);
            Reload();
        }
        else
        {
            Debug.Log("Not enough Woins to buy " + itemName);
        }
    }
    
    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
