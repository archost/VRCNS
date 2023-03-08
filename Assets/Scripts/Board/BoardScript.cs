using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour
{
    [SerializeField]
    private StageDisplay stageDisplayPrefab;

    [SerializeField]
    private TextMeshProUGUI curStageText;

    [SerializeField]
    private TextMeshProUGUI orderText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private TextMeshProUGUI ProfileInfoText;

    [SerializeField]
    private StageController stageController;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private RectTransform scrollContainer;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private Stage currStage = null;

    private float stageDisplayHeight = 0f;

    private float scrollInitHeight = 0f;

    private bool error = false;

    private void Awake()
    {
        if (stageDisplayPrefab == null) Debug.LogError("You forgot to update prefab");

        stageController.OnStageSwitch += OnStageSwitch;       
        stageDisplayHeight = (stageDisplayPrefab.transform as RectTransform).sizeDelta.y;
        scrollInitHeight = scrollContainer.sizeDelta.y;
        scoreText.text = "";
    }

    private void OnDestroy()
    {
        stageController.OnStageSwitch -= OnStageSwitch;
    }

    private void Update()
    {
        if (TimerScript.IsRunning)
        {
            timerText.text = TimerScript.CurrentTimeFormat;
        }
    }

    [ContextMenu("Start Timer")]
    public void StartTimer()
    {
        TimerScript.StartTimer(this.gameObject);
    }

    public void StopTimer()
    {
        TimerScript.StopTimer(this.gameObject);
    }

    private void ResetScrollPosition()
    {
        float t = scrollContainer.childCount * stageDisplayHeight;
        if (t > scrollInitHeight)
        {
            if (t > scrollContainer.sizeDelta.y)
            {
                scrollContainer.sizeDelta = new Vector2(scrollContainer.sizeDelta.x, t);
            }
            scrollContainer.localPosition = new Vector3(0f, (scrollInitHeight - t) / 2f, 0f);
        }
        else
        {
            scrollContainer.localPosition = Vector3.zero;
        }
    }

    private void OnStageSwitch(Stage stage)
    {
        if (currStage != null)
        {
            StageDisplay sd = Instantiate(stageDisplayPrefab, scrollContainer);
            sd.Init(currStage, error);
            ResetScrollPosition();
        }
        if (stage == null)
        {
            curStageText.text = "Задачи выполнены";
            orderText.text = "";
            currStage = stage;
        }
        else
        {
            curStageText.text = stage.description;
            orderText.text = stage.ID.ToString();
            currStage = stage;
        }
        error = false;
    }

    private void OnScoreChanged(int score)
    {
        scoreText.text = $"Баллы: {score}/{ProjectPreferences.instance.maxScore}";
        error = true;
    }

    public void UpdateName()
    {
        stageController.OnScoreChanged += OnScoreChanged;
        var pl = FindObjectOfType<Player>();
        ProfileInfoText.text = $"Обучающийся:\n{pl.PlayerName}\n\nГруппа:\n{pl.PlayerGroup}";
    }
}
