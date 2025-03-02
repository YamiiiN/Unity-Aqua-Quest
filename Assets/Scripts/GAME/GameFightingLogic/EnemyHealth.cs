using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead = false;

    public static int killCount = 0;

    public Image healthBarFill; // Drag the health bar fill image in the Inspector

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        UpdateHealthBar(); // Ensure health bar is full on start
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

        // Destroy enemy after animation finishes (adjust time as needed)
        // Adjust 2f to match your animation length
    }

    public void Sira()
    {
         Destroy(gameObject);
    }
}
