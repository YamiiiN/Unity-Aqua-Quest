using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Login : MonoBehaviour
{
    private string apiUrl = "http://localhost:5000/api/";  // Change this to your API URL

    // Public method to call on button click
    public void OnLoginButtonClick()
    {
        StartCoroutine(GetDataFromAPI());
    }

    IEnumerator GetDataFromAPI()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
