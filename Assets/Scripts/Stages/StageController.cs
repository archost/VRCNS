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
        scp.OnPartFinished += ProcessInstallFinished;

        partFactory.SpawnParts(mediator);

        if (stages.Count != 0)
        {
            NextStage();
        }
    }

    private void ProcessInstallFinished(CommandFinished command)
    {
        if (command.Sender is PartPresenter)
        {
            var pp = command.Sender as PartPresenter;
            if (pp.PartData.ID == CurrentStage.target.ID)
            {
                Debug.Log($"Successfully completed \"{CurrentStage.description}\"!");
                NextStage();
            }
        }
    }

    public void NextStage()
    {
        if (currentStageIndex < stages.Count)
        {
            currentStageIndex++;
            if (CurrentStage != null) partFactory.ToogleSuitablePoints(CurrentStage.target);
            OnStageSwitch?.Invoke(CurrentStage);
        }
    }

    public void PrevStage()
    {
        // work in progress
    }
}
