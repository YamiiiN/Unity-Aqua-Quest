using UnityEngine;

public class Getter : MonoBehaviour
{

    public GameObject LoadingScreen;
     void Start()
    {
           
    }

    public void executeme()
    {
        SendData.GetPlayerData(LoadingScreen); 
    }
}