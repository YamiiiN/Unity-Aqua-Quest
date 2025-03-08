using UnityEngine;

public class Sender : MonoBehaviour
{
    public GameObject LoadingScreen;
    
    void Start()
    {
        SendData.SendGameData(LoadingScreen);
    }
    // public void SENDME()
    // {
    //     SendData.SendGameData();
    // }
}