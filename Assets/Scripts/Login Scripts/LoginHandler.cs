// OG CODE WAG BURAHIN 
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

public class LoginHandler : MonoBehaviour
{
    public GameObject ME, LOGIN, HOME; // UI Elements

    private string userFilePath;

    public void ClickME()
    {
        // Use Application.persistentDataPath (works on all platforms, including Android)
        userFilePath = Path.Combine(Application.persistentDataPath, "userInfo.json");
        Debug.Log("User info file path: " + userFilePath);

        // Ensure the file exists
        if (!File.Exists(userFilePath))
        {
            File.WriteAllText(userFilePath, "{}"); // Creates an empty JSON object
            Debug.Log("Created new userInfo.json file");
        }

        // Read the file
        string json = File.ReadAllText(userFilePath);

        // Check if user info exists
        if (!string.IsNullOrWhiteSpace(json) && json != "{}")
        {
            ME.SetActive(false);
            LOGIN.SetActive(false);
            HOME.SetActive(true);
            return;
        }

        // Default: Show login screen
        ME.SetActive(false);
        LOGIN.SetActive(true);
    }

}