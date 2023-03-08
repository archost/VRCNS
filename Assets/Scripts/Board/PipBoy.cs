using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PipBoy : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private TextMeshProUGUI taskDesc;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private StageController stageController;

    private void Update()
    {
        if (TimerScript.IsRunning)
        {
            timerText.text = TimerScript.CurrentTimeVerticalFormat;
        }
    }

    private void Awake()
    {
        stageController.OnStageSwitch += OnStageSwitch;
        stageController.OnScoreChanged += OnStageFailed;
        scoreText.text = "";
    }

    private void OnStageSwitch(Stage stage)
    {
        if (stage == null)
        {
            taskDesc.text = "Задачи выполнены";
        }
        else
        {
            taskDesc.text = stage.description;
        }
    }

    private void OnStageFailed(int score)
    {
        scoreText.text = $"Баллы: {score}/{ProjectPreferences.instance.maxScore}";
    }
}
