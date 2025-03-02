using UnityEngine;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{
    public Transform player; // Assign the player in the inspector
    public float[] segmentPositions; // X positions for segments
    public int[] requiredKills; // Required kills for each segment
    private HashSet<int> completedSegments = new HashSet<int>(); // Tracks completed segments
    public GameObject[] enemySpawners;

    private int enemyKillCount = 0; // Track enemy kills

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;

            if (player == null)
            {
                Debug.LogError("GameHandler: No Player found in scene!");
            }
        }
    }



    private void Update()
    {
        if (player == null) return;

        CheckSegmentEvents(player.position.x);
        CheckKillCountAndDeactivateSpawner();
    }


    private void CheckKillCountAndDeactivateSpawner()
    {
        for (int i = 0; i < requiredKills.Length; i++)
        {
            if (enemyKillCount >= requiredKills[i] && enemySpawners[i].activeSelf)
            {
                enemySpawners[i].SetActive(false);
                Debug.Log($"Spawner {i} deactivated due to reaching kill count.");
            }
        }
    }


  private int currentActiveSpawner = -1; // Tracks the active spawner

private void CheckSegmentEvents(float playerX)
{
    for (int i = 0; i < segmentPositions.Length; i++)
    {
        // If player is within a segment and has not completed it
        if (!completedSegments.Contains(i) && playerX >= segmentPositions[i] && 
            (i + 1 >= segmentPositions.Length || playerX < segmentPositions[i + 1]))
        {
            if (enemyKillCount >= requiredKills[i])
            {
                completedSegments.Add(i); // Mark segment as completed
                DeactivateSpawner(i);  // Deactivate the spawner
                Debug.Log($"Segment {i} completed.");
            }
            else
            {
                // Ensure only the current spawner is active
                TriggerSegmentEvent(i);
            }
            return; // Stop checking further segments once the correct one is found
        }
    }
}



private void DeactivateAllSpawners()
{
    foreach (var spawner in enemySpawners)
    {
        spawner.SetActive(false);
    }
}

private void TriggerSegmentEvent(int i)
{
    if (currentActiveSpawner != i)
    {
        DeactivateAllSpawners();

        if (enemyKillCount < requiredKills[i]) // Only activate if the kill count is not reached
        {
            enemySpawners[i].SetActive(true);
            currentActiveSpawner = i;
            Debug.Log($"Activated Spawner for Segment {i}");
        }
    }
}

private void DeactivateSpawner(int index)
{
    if (index < enemySpawners.Length && enemySpawners[index] != null)
    {
        enemySpawners[index].SetActive(false);
        Debug.Log($"Spawner {index} deactivated.");
    }
}


    // Call this function when an enemy is killed
    public void EnemyKilled()
    {
        enemyKillCount++;
        Debug.Log("Enemy killed! Total: " + enemyKillCount);
    }

        private void ActivateSpawner(int index)
        {
            // Deactivate all spawners first
            // foreach (var spawner in enemySpawners)
            // {
            //     spawner.SetActive(false);
            // }

            // Activate only the spawner for the current segment
            // if (index < enemySpawners.Length)
            // {
                // if (!enemySpawners[index].activeSelf)
                // {
                //     enemySpawners[index].SetActive(true);
                // }
                
            // }
        }
}
