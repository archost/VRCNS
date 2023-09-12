using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DB_main : MonoBehaviour
{
    public static DB_main instance;

    private static readonly string URL = "http://217.18.60.195:8080";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        StartCoroutine(CheckServerConnection());
    }

    [ContextMenu("Test")]
    public void CallDB()
    {
        var currentD = PlayerDataController.instance.CurrentPlayerData;

        string name = currentD.PlayerName.Split(' ')[1];
        string surname = currentD.PlayerName.Split(' ')[0];
        StartCoroutine(PostData(name, surname, currentD.Group, currentD.TestResult ? 1 : 0, DateTime.UtcNow.ToString("yyyy-MM-dd"), 
            currentD.Score, currentD.Scenario + 1));
    }

    private IEnumerator PostData(string name, string surname, string group, int mark, string udate, int points, int test)
    {
        string url = URL + "/sendData";
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("surname", surname);
        form.AddField("group", group);
        form.AddField("mark", mark);
        form.AddField("udate", udate);
        form.AddField("points", points);
        form.AddField("test", test);
        using var request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        if (request.responseCode == 200)
        {
            Debug.Log("Data has been sent!");
        }
        else
        {
            Debug.LogError(request.error);
            //FindObjectOfType<StartScreen>().ShowServerConnectionError(request.error);
        }


    }

    private IEnumerator CheckServerConnection()
    {
        string url = URL + "/tshirt";
        string controlString = "{\"tshirt\":\"ss\",\"size\":\"large\"}";
        string gotString = string.Empty;
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                gotString = request.downloadHandler.text;
                if (gotString != controlString)
                {
                    FindObjectOfType<StartScreen>().ShowServerConnectionError("Несовпадение контрольной строки");
                }
            }
            else
            {
                //Debug.LogError(request.error);
                FindObjectOfType<StartScreen>().ShowServerConnectionError(request.error);
            }
        }
        
    }

    [Obsolete]
    IEnumerator Register(string groupfield, string namefield, short score, string result,int scenario, string testdate, string testlength)
    {
        WWWForm form = new WWWForm();

        form.AddField("name", namefield);
        form.AddField("group", groupfield);
        form.AddField("score", score);
        form.AddField("result", result);
        form.AddField("scenario", scenario);
        form.AddField("testdate", testdate);
        form.AddField("testlength", testlength);

        WWW www = new WWW("http://localhost/sqlconnect/VrcnsDb.php", form);
        yield return www;

        if (www.text == "0")
            Debug.Log("User created ");
        else
            Debug.Log("User creation failed:" + www.text);
    }
}
