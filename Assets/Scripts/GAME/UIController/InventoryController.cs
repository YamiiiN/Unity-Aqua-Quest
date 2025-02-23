using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject RelicInventory, PotionInventory, AttributesButtons;
    public void OnPotion()
    {
        RelicInventory.SetActive(false);
        AttributesButtons.SetActive(false);
        PotionInventory.SetActive(true);
    }
     public void OnRelic()
    {
        PotionInventory.SetActive(false);
        RelicInventory.SetActive(true);
        AttributesButtons.SetActive(true);
    }
}
