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
    }

    private void OnStageSwitch(Stage stage)
    {
        if (stage == null)
        {
            taskDesc.text = "Нет задания";
        }
        else
        {
            taskDesc.text = stage.description;
        }
    }
}
