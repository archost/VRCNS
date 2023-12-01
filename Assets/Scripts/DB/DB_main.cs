using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class DB_main : MonoBehaviour
{
    public static DB_main instance;

    private string URL = null;

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
        CheckConnection();
    }

    public void CheckConnection()
    {
        if (PlayerPrefs.HasKey(ProjectPreferences.BackendAddressKey))
        {
            URL = PlayerPrefs.GetString(ProjectPreferences.BackendAddressKey);
            StartCoroutine(CheckServerConnection());
        }
        else
        {
            FindObjectOfType<StartScreen>().ShowServerConnectionError("Не задан адрес отправки данных!");
        }
    }

    [ContextMenu("Test")]
    public void CallDB()
    {
        var currentD = PlayerDataController.instance.CurrentPlayerData;
        string name = currentD.PlayerName;
        string surname = currentD.PlayerSurname;
        
        
        StartCoroutine(PostData(name, surname, currentD.Group, 
            currentD.TestResult ? 1 : 0, currentD.TestTime, DateTime.UtcNow.ToString("yyyy-MM-dd"), 
            currentD.Score, currentD.Scenario + 1, currentD.Mistakes));
    }

    private IEnumerator PostData(string name, 
        string surname, string group, int mark, 
        string time, string udate, int points, int test, string[] mistakes)
    {
        string url = URL + "/sendData";
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("surname", surname);
        form.AddField("group", group);
        form.AddField("mark", mark);
        form.AddField("utime", time);
        form.AddField("udate", udate);
        form.AddField("points", points);
        form.AddField("test", test);
        form.AddField("mistakes", JsonConvert.SerializeObject(mistakes, Formatting.None));
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
}
