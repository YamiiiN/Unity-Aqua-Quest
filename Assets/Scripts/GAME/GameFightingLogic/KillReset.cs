using UnityEngine;

public class KillReset : MonoBehaviour
{
    // [SerializeField] 
    public static void ResetKillCount()
    {
        EnemyHealth.killCount = 0;
    }
}