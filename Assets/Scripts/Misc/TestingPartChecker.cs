using UnityEngine;
using UnityEngine.Events;

public class TestingPartChecker : MonoBehaviour, ISCInit
{
    private int currentPartID = -1;

    private Part lastCorrectPart = null;

    public void Init(StageController sc)
    {
        if(!ProjectPreferences.instance.IsTesting)
        {
            gameObject.SetActive(false);
            return;
        }
        sc.OnStageSwitch += SwitchTarget;
    }

    private void SwitchTarget(Stage stage)
    {
        if (stage == null || stage.goalType == StageGoalType.Action) return;
        gameObject.SetActive(stage.assemblyType == GameAssemblyType.Assembly);
        currentPartID = stage.target.ID;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Part p))
        {
            if (p.PartID != currentPartID)
            {
                if (p.IgnoreErrors) return;
                /* Баг: можно во время установки одной детали пронести в рабочую
                 зону неправильные детали, и за это не будет штрафа */
                if (!lastCorrectPart || !lastCorrectPart.IsFixed)
                {
                    Debug.Log("Incorrect part!");
                    p.WrongPartDisplay();
                    StageController.OnMadeMistake(new(this));
                }               
            }
            else
            {
                lastCorrectPart = p;
            }
        }
    }
}

