using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private struct TestingResults
    {
        public string studentName;
        public string groupName;
        public string time;
        public string assemblyType;
        public int score;
        public int maxScore;
        public bool result;
    }

    //private TestingResults results;

    public void InitWindow(StageController sc)
    {
        PlayerData data = PlayerDataController.instance.CurrentPlayerData;       
        nameText.text = $"Проходил: {data.PlayerName}";
        groupText.text = $"Группа: {data.Group}";
        PlayerDataController.instance.SetTestTime(ProjectPreferences.instance.IsTraining ? TimerScript.CurrentTime : TimerScript.BackCurrentTime);
        string time = ProjectPreferences.instance.IsTraining ? TimerScript.CurrentTimeFormat : TimerScript.BackwardsTimeFormat;
        timeText.text = $"Время: {time}";
        PlayerDataController.instance.SetTestDateToNow();
        if (ProjectPreferences.instance.IsTesting)
        {
            TestingPanel.SetActive(true);
            PlayerDataController.instance.SetScore(sc.Score);
            scoreText.text = $"Баллы: {sc.Score}/{ProjectPreferences.instance.maxScore}";
            string mark = sc.Score < ProjectPreferences.instance.maxScore * 0.8f ? "Незачет" : "Зачет";
            PlayerDataController.instance.SetTestResult(sc.Score > ProjectPreferences.instance.maxScore * 0.8f);
            markText.text = mark;
        }
        else 
        {
            TrainingPanel.SetActive(true);
        }

        SaveResults();
    }

    public void SoftReset()
    {
        SceneManager.LoadScene(0);
    }

    public void HardReset()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void SaveResults()
    {

    }
}
