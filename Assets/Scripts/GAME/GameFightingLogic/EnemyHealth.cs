using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json.Linq;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead = false;
    // public string enemyName;
    public static int killCount;
    private string filePath, woinsFilePath;
    public Image healthBarFill; // Drag the health bar fill image in the Inspector
    public EnemyDetail enemyDetail; // Assign in the Inspector
    // public int AmIWorthy ; // The amount of Woins earned when enemy dies

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        UpdateHealthBar(); // Ensure health bar is full on start
        filePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
        woinsFilePath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative health

        UpdateHealthBar(); // Update UI

        Debug.Log("Enemy took damage: " + damage + " | Remaining Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");

        // Play death animation
        anim.SetTrigger("dead");

        // Disable collisions (optional)
        GetComponent<Collider2D>().enabled = false;

        killCount++;
        UpdateKillCount(enemyDetail.Name);
        Earnings(); // Increase Woins when enemy dies

        // Destroy enemy after animation (adjust as needed)
    }

    public void UpdateKillCount(string enemyName)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("JSON file not found: " + filePath);
            return;
        }

        // Read the JSON file
        string jsonContent = File.ReadAllText(filePath);
        JObject jsonData = JObject.Parse(jsonContent);

        // Check if the enemy exists in the "Kills" section
        if (jsonData["Kills"]?[enemyName]?["TotalKills"] != null)
        {
            // Increase the enemy's TotalKills
            int currentKills = jsonData["Kills"][enemyName]["TotalKills"].Value<int>();
            jsonData["Kills"][enemyName]["TotalKills"] = currentKills + 1;

            // Increase the OverallKills
            int overallKills = jsonData["Kills"]["OverallKills"].Value<int>();
            jsonData["Kills"]["OverallKills"] = overallKills + 1;

            // Write back to the file
            File.WriteAllText(filePath, jsonData.ToString());
            Debug.Log($"Updated {enemyName} kills: {currentKills + 1}, Overall Kills: {overallKills + 1}");
        }
        else
        {
            Debug.LogWarning($"Enemy '{enemyName}' not found in JSON.");
        }
    }

    public void Earnings()
    {
        if (!File.Exists(woinsFilePath))
        {
            Debug.LogError("PlayerInventory.json not found: " + woinsFilePath);
            return;
        }

        // Read current JSON data
        string jsonContent = File.ReadAllText(woinsFilePath);
        JObject jsonData = JObject.Parse(jsonContent);

        // Ensure "Woins" exists in JSON
        if (jsonData["Woins"] != null)
        {
            int currentWoins = jsonData["Woins"].Value<int>();
            jsonData["Woins"] = currentWoins + enemyDetail.worth; // Increase Woins

            // Write updated data back to file
            File.WriteAllText(woinsFilePath, jsonData.ToString());
            Debug.Log($"Earned {enemyDetail.worth} Woins! Total Woins: {currentWoins + enemyDetail.worth}");
        }
        else
        {
            Debug.LogWarning("Woins field missing in PlayerInventory.json.");
        }
    }

    public void Sira()
    {
        Destroy(gameObject);
    }
}
