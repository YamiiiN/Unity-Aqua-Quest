using UnityEngine;

[CreateAssetMenu(fileName = "PotionDatabase", menuName = "Scriptable Objects/PotionDatabase")]
public class PotionDatabase : ScriptableObject
{
    public Potion[] Potions;
}
