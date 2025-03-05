using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameItems))]
public class GameItemsEditor : Editor
{
    private readonly string[] typeOptions = { "Relic", "Potion" };
    private readonly string[] effectOptions = { "Health", "Damage", "Defense", "Speed" };

    public override void OnInspectorGUI()
    {
        GameItems item = (GameItems)target;

        item.ItemID = EditorGUILayout.TextField("Item ID", item.ItemID);

        item.Price = EditorGUILayout.IntField("Price", item.Price);

        // Dropdown for Type
        int selectedTypeIndex = Mathf.Max(0, System.Array.IndexOf(typeOptions, item.Type));
        selectedTypeIndex = EditorGUILayout.Popup("Type", selectedTypeIndex, typeOptions);
        item.Type = typeOptions[selectedTypeIndex];

        item.Name = EditorGUILayout.TextField("Name", item.Name);
        item.Description = EditorGUILayout.TextField("Description", item.Description);

        // Dropdown for Effect
        int selectedEffectIndex = Mathf.Max(0, System.Array.IndexOf(effectOptions, item.Effect));
        selectedEffectIndex = EditorGUILayout.Popup("Effect", selectedEffectIndex, effectOptions);
        item.Effect = effectOptions[selectedEffectIndex];

        item.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.Icon, typeof(Sprite), false);

        // Show float if Effect is Speed, otherwise show int
        if (item.Effect == "Speed")
        {
            item.UnitEffectFloat = EditorGUILayout.FloatField("Unit Effect (Float)", item.UnitEffectFloat);
        }
        else
        {
            item.UnitEffectInt = EditorGUILayout.IntField("Unit Effect (Int)", item.UnitEffectInt);
        }

        EditorUtility.SetDirty(item);
    }
}
