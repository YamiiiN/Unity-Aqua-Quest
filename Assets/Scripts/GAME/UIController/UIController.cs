using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject settingsPanel;
    public void OnSettingsButton()
    {
        settingsPanel.SetActive(true);
    }

    public void OffSettingsButton()
    {
        settingsPanel.SetActive(false);
    }
}
