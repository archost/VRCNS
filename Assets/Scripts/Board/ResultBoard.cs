using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ResultBoard : MonoBehaviour
{
    public Transform TeleportTransform => teleportTransform;

    [SerializeField]
    private Transform teleportTransform;

    [SerializeField]
    private GameObject TestingPanel;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI markText;

    [SerializeField]
    private GameObject TrainingPanel;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI groupText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    public void InitWindow(StageController sc)
    {
        PlayerData data = PlayerDataController.instance.CurrentPlayerData;       
        nameText.text = $"Проходил: {data.PlayerSurname} {data.PlayerName}";
        groupText.text = $"Группа: {data.Group}";
        PlayerDataController.instance.SetTestTime(ProjectPreferences.instance.IsTraining ? TimerScript.CurrentTimeFormat : TimerScript.BackwardsTimeFormat);
        string time = ProjectPreferences.instance.IsTraining ? TimerScript.CurrentTimeFormat : TimerScript.BackwardsTimeFormat;
        timeText.text = $"Время: {time}";
        PlayerDataController.instance.SetTestDateToNow();
        if (ProjectPreferences.instance.IsTesting)
        {
            TestingPanel.SetActive(true);
            PlayerDataController.instance.SetScore(sc.Score);
            scoreText.text = $"Баллы: {sc.Score}/{ProjectPreferences.instance.maxScore}";
            bool mark = GetResult(sc.Score);
            PlayerDataController.instance.SetTestResult(mark);
            PlayerDataController.instance.SetMistakes(sc.Mistakes);
            markText.text = mark ? "Зачёт" : "Незачёт";
            SaveResults();
        }
        else 
        {
            TrainingPanel.SetActive(true);
        }
    }

    private bool GetResult(int score)
    {
        if (ProjectPreferences.instance.IsTesting && TimerScript.BackCurrentTime <= 0)
            return false;
        return score >= ProjectPreferences.instance.maxScore * 0.8f;
    }

    public void SoftReset()
    {
        PlayerDataController.instance.SetReset(true);
        SceneManager.LoadScene(0);
    }

    public void HardReset()
    {
        PlayerDataController.instance.SetReset(false);
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void SaveResults()
    {
        if (DB_main.instance == null)
        {
            Debug.LogError(nameof(DB_main) + " is null!");
            return;
        }
        DB_main.instance.CallDB();
    }
}
