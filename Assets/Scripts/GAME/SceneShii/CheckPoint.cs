using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public string Scento; // Scene name to load
    public GameObject Check; // UI element or GameObject to activate if file exists

    public void FileChecker()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "PlayerStats.json");
        string woinsFilePath = Path.Combine(Application.persistentDataPath, "PlayerInventory.json");

        // Check if either of the files exist
        if (File.Exists(filePath) || File.Exists(woinsFilePath))
        {
            Debug.Log("File exists. Showing checkpoint option.");
            SceneManager.LoadScene(Scento);
             // Activate the checkpoint option
        }
        else
        {
            Debug.Log("Files not found. Loading scene: " + Scento);
            Check.SetActive(true);
        }
    }

    public void SceneChangerKo()
    {
        Debug.Log("Changing scene to: " + Scento);
        SceneManager.LoadScene(Scento);
    }
}
