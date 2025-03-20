using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using Unity.Loading;
public class PopulateRewards : MonoBehaviour
{
    


    public void ButtonSpawner(JObject saves, GameObject button, Transform parent, TMP_Text msgholder, GameObject panel, GameObject claim, TMP_Text quantity, string pathh, GameObject LoadingPanel, GameObject ClaimButtonHolder)
    {
        // Button claimButton = claim?.GetComponent<Button>();

        
        if (saves.TryGetValue("saves", out JToken savesArray) && savesArray is JArray array)
        {
            if(array.Count == 0)
            {
                // Debug.LogWarning("No saves found in the array.");
                msgholder.text = "NOTHING TO CLAIM";
                panel.SetActive(false);
                ClaimButtonHolder.SetActive(false);
                return;
            }
            foreach (var save in array)
            {
                var buttonClone = Instantiate(button, parent);

                // Set the month text inside the button
                buttonClone.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = save["month"].Value<string>();

                // Find the actual button component
                Transform childTransform = buttonClone.transform.GetChild(0);
                Debug.Log("Child Transform: " + childTransform.name);

                Button buttonComponent = childTransform.GetComponent<Button>();
                // if (claimButton != null)
                // {
                //     Debug.LogError("claimButton is not null");
                //     return;
                // }
                if (buttonComponent != null)
                {
                    // Extract the saved cost safely
                    string month = save["month"].ToString();
                    float savedCost = 0f;

                    if (save["savedCost"] != null && float.TryParse(save["savedCost"].ToString(), out savedCost))
                    {
                        // Add listener using a lambda to capture the value
                        buttonComponent.onClick.AddListener(() => 
                        {
                            msgholder.text = msgChanger(savedCost, panel);
                            if(quantity)
                            {
                                quantity.text = Mathf.FloorToInt(savedCost).ToString();
                            }
                            
                            ListenerRemover(claim, savedCost, save["_id"].ToString(), pathh, LoadingPanel);
                        });


                    }
                    else
                    {
                        Debug.LogWarning($"Invalid savedCost value for month {month}: {save["savedCost"]}");
                    }
                }
                else
                {
                    Debug.LogError($"❌ Button component is missing on: {childTransform.name}");
                }
            }
        }
        else
        {
            Debug.LogWarning("Invalid saves format: 'saves' key not found or not an array.");
        }
    }

    private string msgChanger(float cost, GameObject panel)
    {

        if (cost >= 0)
        {
            panel.SetActive(true);
            return $" KUDOS!  Thank you for following our tips! You've saved an incredible PHP{cost:F2}—that's a huge win!  Keep up the amazing work, and let's continue making a difference together! ";
        }
        panel.SetActive(false);
        return "You didn't save any money this month. Please do your best to follow our provided tips next time ;)  But don't worry! We're here to help you save more money next month! ";
    }

    private void ListenerRemover(GameObject claim, float torecieve, string saveID, string pathh, GameObject LoadingPanel)
    {
        Button claimButton = claim?.GetComponent<Button>();
        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(() => AddWoinsAndCallApi(torecieve, saveID, pathh, LoadingPanel));
        // claim.onClick.RemoveAllListeners();
        // claim.onClick.AddListener(() => AddWoinsAndCallApi(torecieve, saveID));
    }

    private void AddWoinsAndCallApi(float toRecieve, string RewardId, string pathh, GameObject LoadingPanel)
    {
        int wholeNumber = Mathf.FloorToInt(toRecieve);
        string claimRewardAPI = ReusableVar.baseUrl + "claimReward/" + RewardId;
        Debug.Log("Claim Reward API: " + claimRewardAPI);
        try 
        {
            using (UnityWebRequest request = UnityWebRequest.PostWwwForm(claimRewardAPI, ""))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();

                while (!operation.isDone) { 
                    LoadingPanel.SetActive(true);
                }

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + request.error);
                    LoadingPanel.SetActive(false);
                }
                else
                {
                    if(wholeNumber >= 0)
                    {
                        
                        
                        JObject inventory = ReusableVar.InventoryNya;
                        if (inventory.ContainsKey("Woins"))
                        {
                            inventory["Woins"] = inventory["Woins"].Value<int>() + wholeNumber;
                        }

                        // Open the file for writing (without overwriting other content)
                        using (StreamWriter writer = new StreamWriter(ReusableVar.PlayerInven, false)) // 'false' prevents appending
                        {
                            writer.Write(inventory.ToString()); // Writes only the modified JSON content
                        }

                        Debug.Log("Successfully updated woins!");
                        LoadingPanel.SetActive(false);
                        SendData.SendGameData(LoadingPanel);
                        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                        
                    }
                    LoadingPanel.SetActive(false);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }



            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

   
}