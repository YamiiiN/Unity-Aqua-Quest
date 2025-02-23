// OG CODE WAG BURAHIN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq;

public class BillManager : MonoBehaviour
{
    private string getAllBillsURL = "http://localhost:5000/api/waterBill/bills";

    public GameObject billPrefab; // Prefab containing fileIdText, DateText, OutcomeText
    public Transform billContainer; // Parent container to hold all bill items


    void Start()
    {
        StartCoroutine(FetchBills());
    }

    // OF CODE
    // IEnumerator FetchBills()
    // {
    //     string jwtToken = PlayerPrefs.GetString("jwtToken", "");

    //     if (string.IsNullOrEmpty(jwtToken))
    //     {
    //         Debug.LogError("JWT token is missing. Please log in.");
    //         yield break;
    //     }

    //     UnityWebRequest request = UnityWebRequest.Get(getAllBillsURL);
    //     request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

    //     yield return request.SendWebRequest();

    //     if (request.result == UnityWebRequest.Result.Success)
    //     {
    //         string jsonResponse = request.downloadHandler.text;
    //         JArray bills = JArray.Parse(jsonResponse);

    //         Debug.Log("Fetched Bills Successfully: " + jsonResponse);

    //         DisplayBills(bills);
    //     }
    //     else
    //     {
    //         Debug.LogError("Failed to fetch bills: " + request.error);
    //     }
    // }


    public IEnumerator FetchBills()
    {
        string jwtToken = PlayerPrefs.GetString("jwtToken", "");

        if (string.IsNullOrEmpty(jwtToken))
        {
            Debug.LogError("JWT token is missing. Please log in.");
            yield break;  // Use yield break to exit the coroutine
        }

        UnityWebRequest request = UnityWebRequest.Get(getAllBillsURL);
        request.SetRequestHeader("Authorization", "Bearer " + jwtToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            JArray bills = JArray.Parse(jsonResponse);

            Debug.Log("Fetched Bills Successfully: " + jsonResponse);

            DisplayBills(bills);
        }
        else
        {
            Debug.LogError("Failed to fetch bills: " + request.error);
        }
    }

    void DisplayBills(JArray bills)
    {
        foreach (Transform child in billContainer)
        {
            Destroy(child.gameObject); // Clear old bills before adding new ones
        }

        float yOffset = 100f; // Adjust spacing between bill items

        for (int i = 0; i < bills.Count; i++)
        {
            JObject bill = (JObject)bills[i];

            GameObject billInstance = Instantiate(billPrefab, billContainer);
            billInstance.transform.localPosition = new Vector3(0, -i * yOffset, 0); // Stack downward

            
            // Find the TextMeshPro elements inside the instantiated prefab
            TMP_Text fileIdText = billInstance.transform.Find("FileIdText")?.GetComponent<TMP_Text>();
            TMP_Text dateText = billInstance.transform.Find("DateText")?.GetComponent<TMP_Text>();
            TMP_Text outcomeText = billInstance.transform.Find("OutcomeText")?.GetComponent<TMP_Text>();

            if (fileIdText != null) fileIdText.text = bill["_id"].ToString();
            if (dateText != null) dateText.text = bill["createdAt"].ToString();
            if (outcomeText != null) outcomeText.text = "PROCESSED";
        }
    }

    public void ClearBills()
    {
        foreach (Transform child in billContainer)
        {
            Destroy(child.gameObject); // Clear all old bill items
        }

        Debug.Log("Bills have been cleared.");
    }


}