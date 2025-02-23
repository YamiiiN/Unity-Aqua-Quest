using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public String Scento;
    public void SceneChangerKo()
    {
        SceneManager.LoadScene(Scento);
    }
}
