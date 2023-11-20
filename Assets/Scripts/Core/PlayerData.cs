using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using VREventArgs;

[Serializable]
public struct PlayerData
{
    public string PlayerName;

    public string PlayerSurname;

    public string Group;

    public DateTime TestDate;

    public short Scenario;

    public short GameMode;

    public short Score;

    public bool TestResult;

    public string TestTime;

    public string[] Mistakes;

    public PlayerData(string name, string surname, string group)
    {
        PlayerName = name;
        PlayerSurname = surname;
        Group = group;
        TestDate = DateTime.MinValue;
        Scenario = 0;
        GameMode = 0;
        Score = 0;
        TestResult = false;
        TestTime = "00:00";
        Mistakes = new string[0];
    }
}
