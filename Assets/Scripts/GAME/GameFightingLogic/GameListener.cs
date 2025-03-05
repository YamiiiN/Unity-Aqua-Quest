using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameListener : MonoBehaviour
{
    public float cor1, cor2, cor3;
    public int targetkill1, targetkill2, targetkill3;
    public GameObject player, ArrowUI, ClearedUI, SuccessUi, spawner1, spawner2, spawner3;
    public BoxCollider2D bound1, bound2, bound3;

    public TMP_Text ScoreCard, SuccessKillcount, Earnings;

    public Button continueButton;
    private int current;

    public void Update()
    {
        ScoreCard.text = EnemyHealth.killCount.ToString();
        current = EnemyHealth.killCount;
        SpawnerChecker();
    }

    private void SpawnerChecker()
    {
        if (player.transform.position.x >= cor1 && player.transform.position.x < cor2)
        {
            if (current >= targetkill1)
            {
                ClearSegment();
            }
            else
            {
                ToggleSpawner(spawner1, spawner2, spawner3);
            }
            ToggleCollider(bound1, bound2, bound3);
        }
        else if (player.transform.position.x >= cor2 && player.transform.position.x < cor3)
        {
            if (current < targetkill1) return; // Prevent access before targetkill1 is met

            if (current >= targetkill2)
            {
                ClearSegment();
            }
            else
            {
                ToggleSpawner(spawner2, spawner1, spawner3);
                
                if(!ClearedUI.activeSelf && !ArrowUI.activeSelf)
                {
                    return;
                }
                    ClearedUI.SetActive(false);
                    ArrowUI.SetActive(false);
            }
            ToggleCollider(bound2, bound1, bound3);
        }
        else if (player.transform.position.x >= cor3)
        {
            if (current < targetkill2) return; // Prevent access before targetkill2 is met

            if (current >= targetkill3)
            {
                DestroyAllEnemies();
                SuccessUi.SetActive(true);
                SuccessKillcount.text = current.ToString();
                int totalEarn = current * EnemyHealth.AmIWorthy;
                Earnings.text = totalEarn.ToString();
                continueButton.onClick.AddListener(() =>
                {
                    KillReset.ResetKillCount();
                    return;
                });
                
            }
            else
            {
                ToggleSpawner(spawner3, spawner1, spawner2);
                if(!ClearedUI.activeSelf && !ArrowUI.activeSelf)
                {
                    return;
                }
                    ClearedUI.SetActive(false);
                    ArrowUI.SetActive(false);
            }
            ToggleCollider(bound3, bound1, bound2);
        }
    }

    private void ClearSegment()
    {
        ToggleAllSpawners();
        DestroyAllEnemies();
        ClearedUI.SetActive(true);
        ArrowUI.SetActive(true);
    }

    private void ToggleSpawner(GameObject spawnerToActivate, GameObject spawnerToDeactivate1, GameObject spawnerToDeactivate2)
    {
        if (!spawnerToActivate.activeSelf)
        {
            spawnerToActivate.SetActive(true);
        }

        if (spawnerToDeactivate1.activeSelf)
        {
            spawnerToDeactivate1.SetActive(false);
        }

        if (spawnerToDeactivate2.activeSelf)
        {
            spawnerToDeactivate2.SetActive(false);
        }
    }

    private void ToggleAllSpawners()
    {
        spawner1.SetActive(false);
        spawner2.SetActive(false);
        spawner3.SetActive(false);
    }

    private void ToggleCollider(BoxCollider2D colliderToActivate, BoxCollider2D colliderToDeactivate1, BoxCollider2D colliderToDeactivate2)
    {
        colliderToActivate.enabled = true;
        colliderToDeactivate1.enabled = false;
        colliderToDeactivate2.enabled = false;
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

}
