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
        string saveFolder = Path.Combine(Application.dataPath, "UserData");
        string userFilePath = Path.Combine(saveFolder, "userInfo.json");

        if (File.Exists(userFilePath))
        {
            string json = File.ReadAllText(userFilePath);
            if (!string.IsNullOrWhiteSpace(json) && json != "{}")
            {
                ME.SetActive(false);
                LOGIN.SetActive(false);
                HOME.SetActive(true);
                return;
            }
        }
        else
        {
            Directory.CreateDirectory(saveFolder); // Ensures directory exists
        }

        ME.SetActive(false);
        LOGIN.SetActive(true);
    }

}