using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign the enemy prefab in the Inspector
    public float minSpawnTime = 3f; // Minimum spawn time
    public float maxSpawnTime = 7f; // Maximum spawn time

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true) // Loop forever
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime); // Wait before spawning

            Instantiate(enemyPrefab, transform.position, Quaternion.identity); // Spawn at spawner's position

            // Debug.Log($"Enemy spawned at {transform.position} after {waitTime:F1} seconds.");
        }
    }
}
