using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using VREventArgs;

public class PlayerDataController : MonoBehaviour
{
    public static PlayerDataController instance = null!;

    public bool IsSoftReset { get; private set; }

    public PlayerData playerData;

    public PlayerData CurrentPlayerData
    {
        get
        {
            return playerData;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        IsSoftReset = false;
        DontDestroyOnLoad(gameObject);
    }

    public void SetFirstParameters(string name, string surname, string group, GameMode mode, GameAssemblyType scenario)
    {
        playerData = new(name, surname, group)
        {
            Scenario = (short)scenario,
            GameMode = (short)mode
        };
    }

    public void SetReset(bool isSoft)
    {
        IsSoftReset = isSoft;
    }

    public void ResetSecondParameters()
    {
        playerData.TestDate = DateTime.MinValue;
        playerData.Scenario = 0;
        playerData.GameMode = 0;
        playerData.Score = 0;
        playerData.TestResult = false;
        playerData.TestTime = "";
        playerData.Mistakes = new string[0];
    }

    public void SetTestTime(string timeFormatted)
    {
        playerData.TestTime = timeFormatted;
    }

    public void SetScore(int score)
    {
        playerData.Score = (short)score;
    }

    public void SetTestResult(bool res)
    {
        playerData.TestResult = res;
    }

    public void SetTestDateToNow()
    {
        playerData.TestDate = DateTime.UtcNow;
    }

    public void SetMistakes(List<MistakeEventArgs> mistakesEvents)
    {
        var mistakes = new string[mistakesEvents.Count];
        for (int i = 0; i < mistakesEvents.Count; i++)
        {
            mistakes[i] = mistakesEvents[i].ToString();
        }
        playerData.Mistakes = mistakes;
    }
}
