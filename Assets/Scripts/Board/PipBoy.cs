using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PipBoy : MonoBehaviour, ISCInit
{
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private TextMeshProUGUI taskDesc;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI FPSText;

    private void Update()
    {
        if (TimerScript.IsRunning)
        {
            timerText.text = TimerScript.CurrentTimeVerticalFormat;
        }
    }

    public void Init(StageController sc)
    {
        if (ProjectPreferences.instance.ShowFPS)
        {
            StartCoroutine(RefreshFPS());
        }
        else FPSText.gameObject.SetActive(false);
        sc.OnStageSwitch += OnStageSwitch;
        sc.OnScoreChanged += OnStageFailed;
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

    private IEnumerator RefreshFPS()
    {
        int frameCounter = 0;
        float timeCounter = 0.0f;
        float lastFramerate = 0.0f;
        float refreshTime = 0.5f;

        while (true) 
        {
            if (timeCounter < refreshTime)
            {
                timeCounter += Time.deltaTime;
                frameCounter++;
            }
            else
            {
                lastFramerate = frameCounter / timeCounter;
                FPSText.text = "FPS: " + (int)lastFramerate;
                frameCounter = 0;
                timeCounter = 0.0f;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnStageFailed(int score)
    {
        scoreText.text = $"Баллы: {score}/{ProjectPreferences.instance.maxScore}";
    }
}
