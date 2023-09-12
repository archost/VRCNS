using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    public string PlayerName;

    public string Group;

    public DateTime TestDate;

    public short Scenario;

    public short GameMode;

    public short Score;

    public bool TestResult;

    public TimeSpan TestLength;

    public PlayerData(string playerName, string group)
    {
        PlayerName = playerName;
        Group = group;
        TestDate = DateTime.MinValue;
        Scenario = 0;
        GameMode = 0;
        Score = 0;
        TestResult = false;
        TestLength = TimeSpan.Zero;
    }
}
