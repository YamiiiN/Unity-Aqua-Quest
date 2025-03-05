// using UnityEngine;

// public class StaticCoroutine : MonoBehaviour
// {
//     private static StaticCoroutine _instance;

//     // Ensure there's only one instance of this class
//     private static StaticCoroutine Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 _instance = new GameObject("StaticCoroutine").AddComponent<StaticCoroutine>();
//                 DontDestroyOnLoad(_instance.gameObject);
//             }
//             return _instance;
//         }
//     }

//     // Start a coroutine from a static context
//     public static void Start(IEnumerator coroutine)
//     {
//         Instance.StartCoroutine(coroutine);
//     }
// }