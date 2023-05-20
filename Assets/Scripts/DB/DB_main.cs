using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_main : MonoBehaviour
{
    public static DB_main instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public void CallDB()
    {
        var currentD = PlayerDataController.instance.CurrentPlayerData;

        StartCoroutine(Register(currentD.Group, currentD.PlayerName, currentD.Score, currentD.TestResult));

    }

    IEnumerator Register(string groupfield, string namefield, short score, string result)
    {
        WWWForm form = new WWWForm();

        form.AddField("name", namefield);
        form.AddField("group", groupfield);
        form.AddField("score", score);
        form.AddField("result", result);

        WWW www = new WWW("http://localhost/sqlconnect/VrcnsDb.php", form);
        yield return www;

        if (www.text == "0")
            Debug.Log("User created ");
        else
            Debug.Log("User creation failed:" + www.text);
    }
}
