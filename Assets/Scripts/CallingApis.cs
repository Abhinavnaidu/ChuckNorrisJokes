using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CallingApis : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Paste those jokes here and hit the space key for awesomeness")]
    private string catergory;
    [SerializeField]
    private string url;
    private string result;
    [SerializeField]
    GetResult getResult;


    [SerializeField]
    private string showUrl;
    [SerializeField]
    [Tooltip("Choose from a bunch of topics to get jokes about Chuck Norris LOL!")]
    JokeList root;

    #region
    [SerializeField]
    private GameObject CatergoryList;
    #endregion

    #region
    [SerializeField]
    private InputField input;
    [SerializeField]
    private GameObject button;
    #endregion


    public void Start()
    {
         StartCoroutine(ShowCatergories(showUrl));

        input.GetComponentInChildren<Text>().text = "Enter the Category";
        button = GameObject.Find("Button");
        button.GetComponentInChildren<Text>().text = "Hit For Jokes";

        button.GetComponent<Button>().onClick.AddListener(Updater);
    }



    public void Updater()
    {
      
       StartCoroutine(CallApi(url));
        
    }

   public IEnumerator CallApi(string url)
    {
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url+input.text))   //url+catergory
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                result = webRequest.downloadHandler.text;
                getResult = JsonUtility.FromJson<GetResult>(result);
                Debug.Log(getResult.value);
                CatergoryList.GetComponent<Text>().text = getResult.value;
            }
        }
    }

    
    IEnumerator ShowCatergories(string urlCatergory)
    {
        using (UnityWebRequest web = UnityWebRequest.Get(urlCatergory))
        {
            yield return web.SendWebRequest();
            if (web.isNetworkError)
            {
                Debug.Log(web.error);
            }
            else
            {
                Debug.Log("Choose from the list of Catergories for the jokes");
                byte[] bt = web.downloadHandler.data;
                string js = System.Text.Encoding.UTF8.GetString(bt);
                string returningJson = Fixjson(js);
                root = JsonUtility.FromJson<JokeList>(returningJson);
                foreach (var x in root.jokes)
                {
                    Debug.Log(x);
                }
            }
        }
    }

    public string Fixjson(string jsonString)
    {
        jsonString = "{\"jokes\":" + jsonString + "}";
        return jsonString;
    }
}



[System.Serializable]
public class GetResult
{
    public string value;
}

[System.Serializable]
public class JokeList
{
    public List<string> jokes;

}

