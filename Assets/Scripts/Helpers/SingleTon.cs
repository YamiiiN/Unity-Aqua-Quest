using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    public static T Instance 
    {
        get
        {
           
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = (T) (object) this;
    }
}
