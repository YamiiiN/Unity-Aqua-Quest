// using System.Collections;
// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.UI;

// public class APIManager : MonoBehaviour
// {
//     // Base URL of your backend API
//     private string baseUrl = "http://localhost:5000/api";

//     // Reference to your input fields in the Unity Inspector
//     public InputField FirstNameInput;
//     public InputField LastNameInput;
//     public InputField AddressInput;
//     public InputField EmailInput;
//     public InputField PasswordInput;

//     // Public method to call on button click for registration
//     public void OnRegisterButtonClick()
//     {
//         StartCoroutine(RegisterUser());
//     }

//     // Public method to call on button click for login
//     public void OnLoginButtonClick()
//     {
//         StartCoroutine(LoginUser());
//     }

//     IEnumerator RegisterUser()
//     {
//         // Get the values from the input fields
//         string firstName = FirstNameInput.text;
//         string lastName = LastNameInput.text;
//         string address = AddressInput.text;
//         string email = EmailInput.text;
//         string password = PasswordInput.text;

//         // Create a form to send data
//         WWWForm form = new WWWForm();
//         form.AddField("first_name", firstName);
//         form.AddField("last_name", lastName);
//         form.AddField("address", address);
//         form.AddField("email", email);
//         form.AddField("password", password);

//         // Create a POST request to the register endpoint
//         using (UnityWebRequest request = UnityWebRequest.Post($"{baseUrl}/register", form))
//         {
//             // Send the request and wait for a response
//             yield return request.SendWebRequest();

//             // Check for errors
//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("User registered successfully!");
//                 Debug.Log("Response: " + request.downloadHandler.text);
//             }
//             else
//             {
//                 Debug.LogError("Error: " + request.error);
//                 Debug.LogError("Response: " + request.downloadHandler.text);
//             }
//         }
//     }



//     // Public method to call on button click
//     public void OnLoginButtonClick()
//     {
//         StartCoroutine(GetDataFromAPI());
//     }

//     IEnumerator GetDataFromAPI()
//     {
//         using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
//         {
//             yield return request.SendWebRequest();

//             if (request.result == UnityWebRequest.Result.Success)
//             {
//                 Debug.Log("Response: " + request.downloadHandler.text);
//             }
//             else
//             {
//                 Debug.LogError("Error: " + request.error);
//             }
//         }
//     }

   
// }


using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginRegister : MonoBehaviour
{
    // Base URL of your backend API
    private string baseUrl = "http://localhost:5000/api";

    // Reference to your input fields in the Unity Inspector
    public InputField FirstNameInput;
    public InputField LastNameInput;
    public InputField AddressInput;
    public InputField EmailInput;
    public InputField PasswordInput;

    //  private InputField FirstNameInput;
    // private InputField LastNameInput;
    // private InputField AddressInput;
    // private InputField EmailInput;
    // private InputField PasswordInput;

    // void Start()
    // {
    //     // Find InputFields by name
    //     FirstNameInput = GameObject.Find("FirstNameInput").GetComponent<InputField>();
    //     LastNameInput = GameObject.Find("LastNameInput").GetComponent<InputField>();
    //     AddressInput = GameObject.Find("AddressInput").GetComponent<InputField>();
    //     EmailInput = GameObject.Find("EmailInput").GetComponent<InputField>();
    //     PasswordInput = GameObject.Find("PasswordInput").GetComponent<InputField>();

    //     // Debug to check if InputFields are found
    //     Debug.Log("FirstNameInput: " + FirstNameInput);
    //     Debug.Log("LastNameInput: " + LastNameInput);
    //     Debug.Log("AddressInput: " + AddressInput);
    //     Debug.Log("EmailInput: " + EmailInput);
    //     Debug.Log("PasswordInput: " + PasswordInput);
    // }
   

    // Public method to call on button click for registration
    public void OnRegisterButtonClick()
    {
        StartCoroutine(RegisterUser());
    }

    // Public method to call on button click for login
    public void OnLoginButtonClick()
    {
        StartCoroutine(GetDataFromAPI());
    }

    IEnumerator RegisterUser()
    {
        // Get the values from the input fields
        string firstName = FirstNameInput.text;
        string lastName = LastNameInput.text;
        string address = AddressInput.text;
        string email = EmailInput.text;
        string password = PasswordInput.text;

        // Create a form to send data
        WWWForm form = new WWWForm();
        form.AddField("first_name", firstName);
        form.AddField("last_name", lastName);
        form.AddField("address", address);
        form.AddField("email", email);
        form.AddField("password", password);

        // Create a POST request to the register endpoint
        using (UnityWebRequest request = UnityWebRequest.Post($"{baseUrl}/register", form))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User registered successfully!");
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }

    IEnumerator GetDataFromAPI()
    {
        // Use the baseUrl for the login endpoint
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/login"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}