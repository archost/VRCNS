using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PartFactory))]
public class StageController : MonoBehaviour
{
    private Mediator mediator;
    private StageControllerPresenter scp;

    public List<Stage> stages;

    [SerializeField]
    private PartHelper partHelper;

    [SerializeField]
    private List<GameObject> inits;

    private PartFactory partFactory;

    private int currentStageIndex = -1;

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

    private void Start()
    {
        partFactory = GetComponent<PartFactory>();

        mediator = new Mediator();
        scp = new StageControllerPresenter(mediator);
        mediator.StageControllerPresenter = scp;
        scp.OnPartFinished += ProcessFinished;
        scp.OnPartHelperUpdate += ProcessHelperUpdate;

        partFactory.SpawnParts(mediator);

        var actionHandlers = FindObjectsOfType<ActionHandler>();
        foreach (var item in actionHandlers) 
        {
            mediator.AddActionHandler(item.InitPresenter(mediator));
        }

        if (stages.Count != 0)
        {
            NextStage();
        }
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

    public void InitScene()
    {
        foreach (var item in inits)
        {
            item.GetComponent<ISCInit>().Init(this);
        }
    }

    private void ProcessHelperUpdate(CommandHelperUpdate c)
    {
        if (ProjectPreferences.instance.gameMode != GameMode.Training) return;
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
                    partFactory.ToogleSuitablePoints(CurrentStage.target);
                    scp.Send(new CommandSetTarget(scp, CurrentStage.target,
                        CurrentStage.initPartState == PartState.Idle ? null : CurrentStage.initPartState), null);
                }
                else if (CurrentStage.goalType == StageGoalType.Action)
                {
                    scp.Send(new CommandSetTargetAction(scp, CurrentStage.actionCode), null);
                }
                
            }
            else TimerScript.StopTimer(gameObject);
            OnStageSwitch?.Invoke(CurrentStage);
        }
    }

    public void PrevStage()
    {
        // work in progress
    }
}
