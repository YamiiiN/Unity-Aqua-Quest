using UnityEngine;

public class HandCollider : MonoBehaviour
{
    private EnemyAI enemyAI; // Reference to parent script

    private void Start()
    {
        // Find the EnemyAI script on the parent
        if (enemyAI == null)
        {
            enemyAI = GetComponentInParent<EnemyAI>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the EnemyAI script (parent) about the hit
            enemyAI.DealDamage(other.GetComponent<Collider2D>());
        }
    }
}
