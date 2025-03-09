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
    public GameObject claimButton;
    
    // private string claimRewardLink = ReusableVar.baseUrl + "claimReward/" + ReusableVar.userId;
    void Start()
    {
        string getRewardsLink = ReusableVar.baseUrl + "getRewards/" + ReusableVar.userId;
        LetsFetchEmAll(getRewardsLink);
    }

    private async Task LetsFetchEmAll(string lenk)
    {
        try
        {
            using (UnityWebRequest request = UnityWebRequest.Get(lenk))
            {
                var opp = request.SendWebRequest();

                // place Loading Screen

                while (!opp.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log(request.downloadHandler.text);
                    var rewards = JObject.Parse(request.downloadHandler.text);
                    Debug.Log(rewards);
                    TheExecutioner(rewards);
                    // ReusableVar.response = rewards;

                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
        }
    }

    private void TheExecutioner(JObject response)
    {
        
        uselessclass.populateRewards.ButtonSpawner(response, button, parent, msgText, ClaimPanel, claimButton, QuantityText);
    }
}

public class uselessclass
{
    static public PopulateRewards populateRewards = new PopulateRewards();
    
}
