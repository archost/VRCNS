using UnityEngine;
using UnityEngine.Events;

public class TestingPartChecker : MonoBehaviour, ISCInit
{
    private int currentPartID = -1;

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
        if (stage == null) return;
        currentPartID = stage.target.ID;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Part p))
        {
            if (p.PartID != currentPartID)
            {
                Debug.Log("Incorrect part!");
                p.WrongPartDisplay();
                OnInvalidPart?.Invoke();
                if (ac != null) ac.PlayClip("error");
            }
        }
    }
}

