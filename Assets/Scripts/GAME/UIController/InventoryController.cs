using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnRelic();
    }
    public GameObject AttributesButtons;
    // public GameObject Potions;
    public void OnPotion()
    {
        // RelicInventory.SetActive(false);
        AttributesButtons.SetActive(false);
      
    }
     public void OnRelic()
    {
  
        AttributesButtons.SetActive(true);
    }
}
