using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using TMPro;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;

public class FetchAllRewards : MonoBehaviour
{
    public GameObject button, ClaimPanel;
    public Transform parent;
    public TMP_Text rewText, msgText, QuantityText;
    public GameObject claimButton, loadingPanel;
    
    // private string claimRewardLink = ReusableVar.baseUrl + "claimReward/" + ReusableVar.userId;
    void Start()
    {
        string pathh = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");
        
        string getRewardsLink = ReusableVar.baseUrl + "getRewards/" + ReusableVar.userId;
        LetsFetchEmAll(getRewardsLink, pathh, loadingPanel);
    }

    private async Task LetsFetchEmAll(string lenk, string pathh, GameObject LoadingPanel)
    {
        try
        {
            using (UnityWebRequest request = UnityWebRequest.Get(lenk))
            {
                var opp = request.SendWebRequest();

                // place Loading Screen

                while (!opp.isDone)
                {
                    LoadingPanel.SetActive(true);
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                    LoadingPanel.SetActive(false);
                }
                else
                {
                    Debug.Log(request.downloadHandler.text);
                    var rewards = JObject.Parse(request.downloadHandler.text);
                    Debug.Log(rewards);
                    TheExecutioner(rewards, pathh, LoadingPanel);
                    LoadingPanel.SetActive(false);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
        }
    }

    private void TheExecutioner(JObject response, string pathh, GameObject LoadingPanel)
    {
        uselessclass.populateRewards.ButtonSpawner(response, button, parent, msgText, ClaimPanel, claimButton, QuantityText, pathh, LoadingPanel);
    }
}

public class uselessclass
{
    static public PopulateRewards populateRewards = new PopulateRewards();
}