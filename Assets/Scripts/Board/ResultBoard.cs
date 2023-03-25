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

    private TestingResults results;

    public void InitWindow(StageController sc)
    {
        results = new TestingResults();
        Player pl = FindObjectOfType<Player>();
        results.assemblyType = ProjectPreferences.instance.assemblyType.ToString();
        results.studentName = pl.PlayerName;
        nameText.text = $"Проходил: {results.studentName}";
        results.groupName = pl.PlayerGroup;
        groupText.text = $"Группа: {results.groupName}";
        results.time = (ProjectPreferences.instance.IsTraining ? TimerScript.CurrentTimeFormat : TimerScript.BackwardsTimeFormat);
        timeText.text = $"Время: {results.time}";
        if (ProjectPreferences.instance.IsTesting)
        {
            TestingPanel.SetActive(true);
            results.score = sc.Score;
            results.maxScore = ProjectPreferences.instance.maxScore;
            scoreText.text = $"Баллы: {results.score}/{results.maxScore}";
            if (sc.Score < ProjectPreferences.instance.maxScore * 0.8f)
            {
                results.result = false;
                markText.text = "Незачет";
            }
            else
            {
                results.result = true;
                markText.text = "Зачет";
            }
        }
        else 
        {
            TrainingPanel.SetActive(true);
        }

        SaveResults();
    }

    public void SoftReset()
    {
        ProjectPreferences.instance.SavedData = (results.studentName, results.groupName);
        SceneManager.LoadScene(0);
    }

    public void HardReset()
    {
        ProjectPreferences.instance.SavedData = ("", "");
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
