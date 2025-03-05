using UnityEngine;

[CreateAssetMenu(fileName = "GameItems", menuName = "Scriptable Objects/GameItems")]
public class GameItems : ScriptableObject
{
    public string ItemID;
    public string Type; 
    public string Name;
    public string Description;
    public string Effect;

    public int Price;
    
    
    public int UnitEffectInt;
    public float UnitEffectFloat;

    public Sprite Icon;

    public object GetUnitEffect()
    {
        return Effect == "Speed" ? (object)UnitEffectFloat : UnitEffectInt;
    }
}
