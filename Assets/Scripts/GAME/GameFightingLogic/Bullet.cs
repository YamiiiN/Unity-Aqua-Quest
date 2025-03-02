using System.IO;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int bulletDamage;

    private void Start()
    {
        bulletDamage = LoadBulletDamage(); // Load damage from JSON
    }

    private int LoadBulletDamage()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerStats playerStats = JsonUtility.FromJson<PlayerStats>(json);

            if (playerStats != null && playerStats.Relics != null && playerStats.Relics.Damage != null)
            {
                return playerStats.PlayerAttributes.TotalDamage; // Fetch TotalDamage
            }
        }

        Debug.LogError("Failed to load bullet damage! Using default value.");
        return 10; // Default bullet damage if file not found
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(bulletDamage); // Apply damage from JSON
            }

            Destroy(gameObject); // Destroy bullet after impact
        }
    }
}
