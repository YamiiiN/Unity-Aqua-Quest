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
    private string userFilepAth;
    public GameObject LOGIN, HOME, ME;

    public void ClickME()
    {
        string projectFolderPath = Application.dataPath; // Points to "Assets" folder in the project
        string saveFolder = Path.Combine(projectFolderPath, "UserData");
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder); // Ensure the folder exists
        }
        userFilepAth = Path.Combine(saveFolder, "userInfo.json");
        if (File.Exists(userFilepAth))
        {
            string json = File.ReadAllText(userFilepAth);
            if (!string.IsNullOrWhiteSpace(json)) 
            {
               ME.SetActive(false);
               LOGIN.SetActive(false);
               HOME.SetActive(true);
            }
        }
        else
        {
            ME.SetActive(false);
            LOGIN.SetActive(true);
        }
    }
}