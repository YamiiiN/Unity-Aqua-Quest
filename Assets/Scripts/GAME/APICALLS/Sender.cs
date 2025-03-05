using UnityEngine;

public class Sender : MonoBehaviour
{
    
    void Start()
    {
        SendData.SendGameData();
    }
    public void SENDME()
    {
        SendData.SendGameData();
    }
}