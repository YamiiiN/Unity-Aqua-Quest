using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.IO;

public class Display : MonoBehaviour
{
    [SerializeField] public Button itemButton; // Reference to the child Button
    public TMP_Text itemNameText; 
    public TMP_Text itemDescriptionText;
    public TMP_Text itemEffectText;
    public TMP_Text Title;

    private GameItems currentItem;
    private Potion currentPotion;

    private DisplayTarget displayTarget;
     // Reference to the external display object
     private GameObject panel;
     private string savePath;
   

    public void Setup(GameItems item, DisplayTarget target, GameObject targeting)
    {
        currentItem = item;
        displayTarget = target; // Store reference to external object
        panel = targeting;

        if (itemButton == null)
        {
            Debug.LogError("Item Button is not assigned in " + gameObject.name);
            return;
        }

        Image buttonImage = itemButton.GetComponent<Image>(); // Get the Image component from the button
        if (buttonImage != null)
        {
            buttonImage.sprite = item.Icon; // Change the button's sprite
        }
        else
        {
            Debug.LogError("Button does not have an Image component on " + gameObject.name);
        }

        // **Dynamically assign the button click event**
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() => SendDataToExternalObject());

        
        // AttrButtonD.onClick.RemoveAllListeners();
        // AttrButtonH.onClick.RemoveAllListeners();
        // AttrButtonDef.onClick.RemoveAllListeners();
        // AttrButtonS.onClick.RemoveAllListeners();
       
        // AttrButtonD.onClick.AddListener(() => CheckAndHideItem(item));
        // AttrButtonH.onClick.AddListener(() => SendDataToExternalObject());
        // AttrButtonDef.onClick.AddListener(() => SendDataToExternalObject());
        // AttrButtonS.onClick.AddListener(() => SendDataToExternalObject());
    }

  

    public void potionSetup(Potion item, DisplayTarget target, GameObject targeting)
    {
        currentPotion = item;
        displayTarget = target; // Store reference to external object
        panel = targeting;

        if (itemButton == null)
        {
            Debug.LogError("Item Button is not assigned in " + gameObject.name);
            return;
        }

        Image buttonImage = itemButton.GetComponent<Image>(); // Get the Image component from the button
        if (buttonImage != null)
        {
            buttonImage.sprite = item.Icon; // Change the button's sprite
        }
        else
        {
            Debug.LogError("Button does not have an Image component on " + gameObject.name);
        }

        // **Dynamically assign the button click event**
        itemButton.onClick.RemoveAllListeners(); // Clear previous listeners to avoid duplicate calls
        itemButton.onClick.AddListener(() => SendDataToExternalObjectPotion()); // Assign the click event
    }

    private void SendDataToExternalObject()
    {
        panel.SetActive(true);
        if (displayTarget != null && currentItem != null)
        {
            displayTarget.UpdateDisplay(currentItem); // Send data to the external object
        }
        else
        {
            Debug.LogError("DisplayTarget or Item is null!");
        }
    }
    private void SendDataToExternalObjectPotion()
    {
        panel.SetActive(true);
        if (displayTarget != null && currentPotion != null)
        {
            displayTarget.UpdateDisplayPotion(currentPotion); // Send data to the external object
        }
        else
        {
            Debug.LogError("DisplayTarget or Item is null!");
        }
    }
}
