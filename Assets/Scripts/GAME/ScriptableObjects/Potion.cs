using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion")]
public class Potion : ScriptableObject
{
    public int ItemID;
    public string Name;
    public string Description;

    public int HealthEffect;
    public int Price;

    public Sprite Icon;
}
