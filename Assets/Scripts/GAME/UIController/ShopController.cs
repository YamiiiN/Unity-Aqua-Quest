using UnityEngine;

public class ShopController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject RelicPanel;
    public GameObject PotionPanel, InfoPanel;

    public GameObject[] Relics;
    public GameObject AssignedRelic;
    public void OnRelicsAttributes()
    {
        foreach(GameObject relic in Relics)
        {
            if(AssignedRelic == relic)
            {
                relic.SetActive(true);
            }
            else
            {
                relic.SetActive(false);
            }
        }
        PotionPanel.SetActive(false);
    }



    public void OnPotionButton()
    {
        PotionPanel.SetActive(true);
        InfoPanel.SetActive(false);
        foreach(GameObject relic in Relics)
        {
            
                relic.SetActive(false);
            
        }
    }

    public void OnRelicButton()
    {
        RelicPanel.SetActive(true);
        PotionPanel.SetActive(false);
        InfoPanel.SetActive(false);
    }

    public void OffPotionButton()
    {
        PotionPanel.SetActive(false);
    }

    public void OffRelicButton()
    {
        RelicPanel.SetActive(false);
         foreach(GameObject relic in Relics)
        {
            
                relic.SetActive(false);
            
        }
    }
}
