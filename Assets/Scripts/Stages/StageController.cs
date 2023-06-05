using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using VREventArgs;

[RequireComponent(typeof(PartFactory), typeof(ScenarioController))]
public class StageController : MonoBehaviour
{
    public static UnityAction<PartInstalledEventArgs> OnPartInstalled;

    public static UnityAction<ActionTakenEventArgs> OnActionTaken;

    public static UnityAction<MistakeEventArgs> OnMadeMistake;

    public static UnityAction<PartSelectedEventArgs> OnPartSelected;

    public static UnityAction<PartClickedEventArgs> OnPartClicked;

    private ScenarioController scCon;

    private Player player;

    public List<Stage> stages;

    [SerializeField]
    private PartHelper partHelper;

    [SerializeField]
    private Assistant assistant;

    [SerializeField]
    private Questionnaire questionnaire;

    [SerializeField]
    private ResultBoard ResultBoard;

    [SerializeField]
    private AudioClip finishClip;

    [SerializeField]
    private List<GameObject> inits;

    private PartFactory partFactory;

    private int currentStageIndex = -1;

    private List<Stage> errorStages;

    private int score = 0;

    private bool errorHappened = false;

    private Scenario scenario;

    private ActionHandler[] actionHandlers;

    public int Score => score;

    private Stage CurrentStage
    {
        get
        {
            if (currentStageIndex <= stages.Count - 1)
                return stages[currentStageIndex];
            else
                return null;
        }
    }

    public UnityAction<Stage> OnStageSwitch;

    public UnityAction<int> OnScoreChanged;

    private void Start()
    {
        partFactory = GetComponent<PartFactory>();
        scCon = GetComponent<ScenarioController>();
        player = FindObjectOfType<Player>();

        OnPartInstalled += PartInstalled;
        OnMadeMistake += Mistake;
        OnPartSelected += PartSelected;
        OnActionTaken += ActionTaken;
        OnPartClicked += PartClicked;

        actionHandlers = FindObjectsOfType<ActionHandler>();

        scenario = scCon.GetScenario();
        stages.AddRange(scenario.stagesList);

        StartCoroutine(InitScene());
    }

    private void PartClicked(PartClickedEventArgs e)
    {
        if (CurrentStage.question == null)
            e.Part.DetachAnimationPlay();
        else
        {
            questionnaire.SetPosition(e.Part.transform.position);
            questionnaire.SetQuestion(CurrentStage.question);
            questionnaire.OnQuestionAnswered += e.Part.DetachAnimationPlay;
            player.PlaySound("notification");
        }
    }

    private void PartInstalled(PartInstalledEventArgs e)
    {
        if (e.Sender is not Part) return;
        var part = e.Sender as Part;
        if (part.PartID == CurrentStage.target.ID)
        {
            partHelper.TurnOff();
            Debug.Log($"(new) Successfully completed \"{CurrentStage.description}\"!");
            NextStage();
        }
    }

    private void ActionTaken(ActionTakenEventArgs e)
    {
        if (e.ActionCode == CurrentStage.actionCode)
        {
            Debug.Log($"(new) Successfully completed \"{CurrentStage.description}\"!");
            NextStage();
        }
    }

    private IEnumerator InitScene()
    {
        foreach (var item in inits)
        {
            item.GetComponent<ISCInit>().Init(this);
        }
        StartCoroutine(partFactory.SpawnParts(scenario.spawnList));
        while (!partFactory.IsDone)
        {
            yield return null;
        }

        if (ProjectPreferences.instance.IsTesting)
        {
            errorHappened = false;
            errorStages = new List<Stage>();
            score = ProjectPreferences.instance.maxScore;
            OnScoreChanged?.Invoke(score);
        }

        yield return new WaitForSeconds(1f);

        if (stages.Count != 0)
        {
            NextStage();
        }
    }

    private void Mistake(MistakeEventArgs e)
    {
        if (ProjectPreferences.instance.IsTraining) return;
        if (!ProjectPreferences.instance.multiErrorAllowed && errorHappened) return;
        Debug.Log($"Made a mistake! (new score - {score - 1})!");
        score = Mathf.Clamp(score - 1, 0, 100);
        errorHappened = true;
        errorStages.Add(CurrentStage);
        player.PlaySound("mistake");
        OnScoreChanged?.Invoke(score);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }

    private void PartSelected(PartSelectedEventArgs e)
    {
        if (ProjectPreferences.instance.IsTesting) return;
        if (e.IsSelected) 
        {
            partHelper.SetTarget(e.PartTransform, e.PartData);
        }
        else
        {
            partHelper.TurnOff();
        }
    }

    public void NextStage()
    {
        if (currentStageIndex < stages.Count)
        {
            currentStageIndex++;
            if (CurrentStage != null)
            {
                if (CurrentStage.goalType == StageGoalType.PartPlacing)
                {
                    errorHappened = false;
                    partFactory.ToogleSuitablePoints(CurrentStage.assemblyType, CurrentStage.target);
                    partFactory.SetPartAsTarget(new PartSetAsTargetEventArgs(this, 
                        CurrentStage.target, CurrentStage.assemblyType));
                }
                else if (CurrentStage.goalType == StageGoalType.Action)
                {
                    SetActionAsTarget(new ActionSetAsTargetEventArgs(this, CurrentStage.actionCode));
                }
                if (CurrentStage.assistantClip != null) assistant.PlayClip(CurrentStage.assistantClip);
            }
            else
            {
                TimerScript.StopTimer(gameObject);
                assistant.PlayClip(finishClip);
                Invoke(nameof(ProcessEnd), finishClip.length);
            }
            OnStageSwitch.Invoke(CurrentStage);
        }
    }

    private void ProcessEnd()
    {
        TeleportRequest tr = new()
        {
            destinationPosition = ResultBoard.TeleportTransform.position,
            destinationRotation = Quaternion.identity,
            requestTime = 0f,
            matchOrientation = MatchOrientation.WorldSpaceUp
        };

        FindObjectOfType<TeleportationProvider>().QueueTeleportRequest(tr);
        ResultBoard.InitWindow(this);
    }

    private void SetActionAsTarget(ActionSetAsTargetEventArgs e)
    {
        foreach (var handler in actionHandlers)
        {
            if (handler.actionCode == e.ActionCode)
            {
                handler.SetAsTarget(e);
            }
        }
    }
}