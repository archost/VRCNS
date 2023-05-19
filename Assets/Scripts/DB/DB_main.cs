using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_main : MonoBehaviour
{
    public void CallRegister(string groupfield, string namefield, string score, bool result)
    {
        StartCoroutine(Register(groupfield, namefield, score, result));

    }

    IEnumerator Register(string groupfield, string namefield, string score, bool result)
    {
        WWWForm form = new WWWForm();

        form.AddField("name", namefield);
        form.AddField("group", groupfield);
        form.AddField("score", score);
        if (result)
            form.AddField("result", "Passed");
        else
            form.AddField("result", "Didn't Passed");

        WWW www = new WWW("http://localhost/sqlconnect/VrcnsDb.php", form);
        yield return www;

        if (www.text == "0")
            Debug.Log("User created ");
        else
            Debug.Log("User creation failed:" + www.text);
    }
}
