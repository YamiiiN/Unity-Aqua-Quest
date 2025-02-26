using UnityEngine;

[CreateAssetMenu(fileName = "GameItemsDatabase", menuName = "Scriptable Objects/GameItemsDatabase")]
public class GameItemsDatabase : ScriptableObject
{
    public GameItems[] Items;
}
