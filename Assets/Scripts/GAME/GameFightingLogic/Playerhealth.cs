using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private int defense;
    public Image healthBar;
    private string jsonFilePath;
    private Animator anim;
    
    public GameObject failedUI;

    void Start()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");

        LoadPlayerStats();  // Load from JSON
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Mathf.Max(damage - defense, 1); // Reduce by defense, but always take at least 1 damage
        currentHealth -= damageTaken;
        Debug.Log("Ouch");

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
            Debug.Log(currentHealth);
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        anim.SetTrigger("Died");
        failedUI.SetActive(true);
        // Play death animation
        // Add death logic (Respawn, Game Over, etc.)
    }

    void DestroyME()
    {
        gameObject.SetActive(false);
    }

    void LoadPlayerStats()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            PlayerStats playerStats = JsonUtility.FromJson<PlayerStats>(json);

            maxHealth = playerStats.PlayerAttributes.TotalHealth;   // Load from PlayerAttributes
            defense = playerStats.PlayerAttributes.TotalDefense;   // Load defense from PlayerAttributes
        }
        else
        {
            Debug.LogError("PlayerStats.json not found!");
            maxHealth = 100; // Default values
            defense = 5;
        }
    }
}
