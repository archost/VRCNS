using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
        if (instance != null) Destroy(gameObject); 
        instance = this;
        IsSoftReset = false;
        DontDestroyOnLoad(gameObject);
    }

    public void SetFirstParameters(string name, string group, GameMode mode, GameAssemblyType scenario)
    {
        playerData = new(name, group)
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
        playerData.TestResult = "None";
        playerData.TestLength = TimeSpan.Zero;
    }

    public void SetTestTime(int seconds)
    {
        playerData.TestLength = TimeSpan.FromSeconds(seconds);
    }

    public void SetScore(int score)
    {
        playerData.Score = (short)score;
    }

    public void SetTestResult(bool res)
    {
        playerData.TestResult = res ? "Зачет" : "Незачет";
    }

    public void SetTestDateToNow()
    {
        playerData.TestDate = DateTime.UtcNow;
    }
}
