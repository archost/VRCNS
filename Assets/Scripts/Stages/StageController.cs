using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PartFactory), typeof(ScenarioController))]
public class StageController : MonoBehaviour
{
    private Mediator mediator;
    private StageControllerPresenter scp;

    private ScenarioController scCon;

    private Player player;

    public List<Stage> stages;

    [SerializeField]
    private PartHelper partHelper;

    [SerializeField]
    private Assistant assistant;

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

        mediator = new Mediator();
        scp = new StageControllerPresenter(mediator);
        mediator.StageControllerPresenter = scp;
        scp.OnPartFinished += ProcessFinished;
        scp.OnPartHelperUpdate += ProcessHelperUpdate;
        scp.OnProcessMistake += OnMistake;

        var actionHandlers = FindObjectsOfType<ActionHandler>();
        foreach (var item in actionHandlers)
        {
            mediator.AddActionHandler(item.InitPresenter(mediator));
        }

        scenario = scCon.GetScenario();
        stages.AddRange(scenario.stagesList);

        StartCoroutine(InitScene());
    }

    private void ProcessFinished(CommandFinished command)
    {
        if (command.Sender is PartPresenter)
        {
            var pp = command.Sender as PartPresenter;
            if (pp.PartData.ID == CurrentStage.target.ID)
            {
                partHelper.TurnOff();
                Debug.Log($"Successfully completed \"{CurrentStage.description}\"!");
                NextStage();
            }
        }
        else if (command.Sender is ActionHandlerPresenter)
        {
            var c = command as CommandActionFinished;
            if (c.ActionCode == CurrentStage.actionCode)
            {
                Debug.Log($"Successfully completed \"{CurrentStage.description}\"!");
                NextStage();
            }
        }
    }


    private IEnumerator InitScene()
    {
        StartCoroutine(partFactory.SpawnParts(mediator, scenario.spawnList));
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
        foreach (var item in inits)
        {
            item.GetComponent<ISCInit>().Init(this);
        }

        yield return new WaitForSeconds(1f);

        if (stages.Count != 0)
        {
            NextStage();
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OnMistake()
    {
        if (ProjectPreferences.instance.IsTraining) return;
        if (!ProjectPreferences.instance.multiErrorAllowed && errorHappened) return;
        score = Mathf.Clamp(score - 1, 0, 100);
        errorHappened = true;
        errorStages.Add(CurrentStage);
        player.PlayMistake();
        OnScoreChanged?.Invoke(score);
    }

    public void OnMistake(CommandProcessMistake c) => OnMistake();

    private void ProcessHelperUpdate(CommandHelperUpdate c)
    {
        if (ProjectPreferences.instance.IsTesting) return;
        if (c.IsSelected)
            partHelper.SetTarget(c.PartTransform, c.PartData);
        else partHelper.TurnOff();
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
                    scp.Send(new CommandSetTarget(scp, CurrentStage.target, CurrentStage.assemblyType, CurrentStage.initPartState), null);
                }
                else if (CurrentStage.goalType == StageGoalType.Action)
                {
                    scp.Send(new CommandSetTargetAction(scp, CurrentStage.actionCode), null);
                }
                if (CurrentStage.assistantClip != null) assistant.PlayClip(CurrentStage.assistantClip);
            }
            else
            {
                TimerScript.StopTimer(gameObject);
                assistant.PlayClip(finishClip);
                Invoke(nameof(ProcessEnd), finishClip.length);
            }
            OnStageSwitch?.Invoke(CurrentStage);
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
}