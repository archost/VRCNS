using UnityEngine;
using UnityEngine.Events;

public class TestingPartChecker : MonoBehaviour, ISCInit
{
    private int currentPartID = -1;

    private Part lastCorrectPart = null;

    private UnityAction OnInvalidPart;

    private AudioController ac;

    public void Init(StageController sc)
    {
        if(!ProjectPreferences.instance.IsTesting
            || ProjectPreferences.instance.assemblyType != GameAssemblyType.Assembly)
        {
            gameObject.SetActive(false);
            return;
        }
        ac = GetComponent<AudioController>();
        OnInvalidPart += sc.OnWrongPart;
        sc.OnStageSwitch += SwitchTarget;
    }

    private void SwitchTarget(Stage stage)
    {
        if (stage == null || stage.goalType == StageGoalType.Action) return;
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
                    OnInvalidPart?.Invoke();
                    if (ac != null) ac.PlayClip("error");
                }               
            }
            else
            {
                lastCorrectPart = p;
            }
        }
    }
}

