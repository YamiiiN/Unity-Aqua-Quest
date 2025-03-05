using UnityEngine;
using TMPro;
using System.IO;

public class DisplayTarget : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemEffectText;
    public TMP_Text titleText;
    public TMP_Text EquipDisplay;
    public TMP_Text Price;

    

   

    public void UpdateDisplay(GameItems item)
    {
        if (item == null) return;

        itemNameText.text = item.Name;
        itemDescriptionText.text = item.Description;

        if (item.Effect == "Speed")
        {
            itemEffectText.text = " " + item.UnitEffectFloat;
        }
        else
        {
            itemEffectText.text = " " + item.UnitEffectInt;
        }

        titleText.text = item.Effect;
        Price.text = item.Price.ToString();

        // Check if the item is equipped and update EquipDisplay text
       

        Debug.Log("Updated display for: " + item.UnitEffectInt);
    }

    public void UpdateDisplayPotion(Potion item)
    {
        if (item == null) return;

        itemNameText.text = item.Name;
        itemDescriptionText.text = item.Description;
        titleText.text = "Health Effect: ";
        itemEffectText.text = " " + item.HealthEffect;
        Price.text = item.Price.ToString();
    }

    
}
