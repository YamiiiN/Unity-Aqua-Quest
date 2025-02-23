using UnityEngine;

public class RewardsController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    public GameObject RewardsPanel;

    public void ShowNotif()
    {
        RewardsPanel.SetActive(true);
    } 

    public void HideNotif()
    {
        RewardsPanel.SetActive(false);
    }
}
