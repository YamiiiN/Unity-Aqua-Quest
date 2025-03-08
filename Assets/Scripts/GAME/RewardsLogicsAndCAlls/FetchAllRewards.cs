using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;
using System;
using Newtonsoft.Json;

public class FetchAllRewards : MonoBehaviour
{
    
    // private string claimRewardLink = ReusableVar.baseUrl + "claimReward/" + ReusableVar.userId;
    void Start()
    {
        string getRewardsLink = ReusableVar.baseUrl + "getRewards/" + ReusableVar.userId;
        LetsFetchEmAll();
    }

    private async Task LetsFetchEmAll()
    {
        try
        {
            using (UnityWebRequest request = UnityWebRequest.Get(getRewardsLink))
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
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
        }
    }
}