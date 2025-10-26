using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static apiHandler;


public class apiHandler : MonoBehaviour
{

    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;

        public LoginData(string u, string p)
        {
            username = u;
            password = p;
        }
    }


    [System.Serializable]
    public class LoginResponse
    {
        public string token;
        public string user_email;
        public string user_nicename;
        public string user_display_name;
    }

    [System.Serializable]
    public class orderResponse
    {
        public int id;
        public string status;
        public List<lineItems> line_items;
    }

    [System.Serializable]
    public class lineItems
    {
        public int id;
        public int product_id;
        public string name;
    }

    [System.Serializable]
    public class OrderList
    {
        public List<orderResponse> orders;
    }


    [System.Serializable]
    public class customerID
    {
               public int id;
    }


    [Tooltip("example https://yourdomain.com/")]
    public string WoocommerceSiteUrl;
    public TMP_InputField TMP_username;
    public TMP_InputField TMP_password;
    public Button loginButton;
    public TMP_Text DebugText;
    public TMP_Text GamePurchaseConfirmationText;
    public GameObject playbutton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = loginButton.GetComponent<Button>();
        btn.onClick.AddListener(login);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void login()
    {
        StartCoroutine(loginProcess());
    }


    IEnumerator loginProcess()
    {
        DebugText.text = "Logging in with user: " + TMP_username.text + " and password: " + TMP_password.text;
        DebugText.text = "URL: " + WoocommerceSiteUrl + "wp-json/jwt-auth/v1/token";


        //UnityWebRequest request = UnityWebRequest.Post(WoocommerceSiteUrl + "wp-json/jwt-auth/v1/token" );

        string jsonbody = JsonUtility.ToJson(new LoginData(TMP_username.text, TMP_password.text));
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonbody);
        Debug.Log(jsonbody);



        UnityWebRequest request = new UnityWebRequest(WoocommerceSiteUrl + "wp-json/jwt-auth/v1/token/", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("User-Agent", "UnityWebRequest");



        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login Successful: " + request.downloadHandler.text);
            

            string responseJson = request.downloadHandler.text;
            LoginResponse loggedData = JsonUtility.FromJson<LoginResponse>(responseJson);
            DebugText.text = $"Login Successfull! Welcome {loggedData.user_display_name} ";
            StartCoroutine(purchaseGameProcess(loggedData.token));
        }
        else
        {
            Debug.Log("Login Failed: " + request.error);
        }


    }

    IEnumerator purchaseGameProcess(string token)
    {   
        // get customer id
        UnityWebRequest customerRequest = new UnityWebRequest(WoocommerceSiteUrl + "wp-json/wp/v2/users/me", "GET");
        customerRequest.downloadHandler = new DownloadHandlerBuffer();
        customerRequest.SetRequestHeader("Authorization", "Bearer " + token);
        customerRequest.SetRequestHeader("Accept", "application/json");
        customerRequest.SetRequestHeader("User-Agent", "UnityWebRequest");
        yield return customerRequest.SendWebRequest();
        string customerIDResponseJson = customerRequest.downloadHandler.text;
        customerID customerIDData = JsonUtility.FromJson<customerID>(customerIDResponseJson);
        Debug.Log("Customer ID: " + customerIDData.id); 

        DebugText.text = "Logged in";
        UnityWebRequest orderRequest = new UnityWebRequest(WoocommerceSiteUrl + "/wp-json/custom/v1/my-orders/", "GET");
        orderRequest.downloadHandler = new DownloadHandlerBuffer();
        orderRequest.SetRequestHeader("Authorization", "Bearer " + token);
        orderRequest.SetRequestHeader("Content-Type", "application/json");
        orderRequest.SetRequestHeader("Accept", "application/json");
        orderRequest.SetRequestHeader("User-Agent", "UnityWebRequest");
        yield return orderRequest.SendWebRequest();

        if (orderRequest.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log(orderRequest.downloadHandler.text);

            string orderResponseJson = orderRequest.downloadHandler.text;
            Debug.Log(orderResponseJson);

            string wrappedJson = "{ \"orders\": " + orderResponseJson + " }";
            OrderList orderData = JsonUtility.FromJson<OrderList>(wrappedJson);
            Debug.Log(orderData);

            //orderResponse firstOrder = orderData.orders[0];
            //Debug.Log(firstOrder.id + " " + firstOrder.status);

            foreach (orderResponse order in orderData.orders)
            {
                Debug.Log(order.id + " " + order.status);

                foreach (lineItems item in order.line_items)
                {
                    Debug.Log(item.id + " " + item.product_id + " " + item.name);

                    if (item.product_id == 4672 ) // replace 123 with your product ID
                    {
                        GamePurchaseConfirmationText.text = "Game purchase successful, play now!" + item.name;
                        playbutton.SetActive(true);
                    }
                }

            }

        }
        else
        {
            Debug.Log("Did not get order data");
        }
    }


    }
