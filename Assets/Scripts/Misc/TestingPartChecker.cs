using UnityEngine;

public class TestingPartChecker : MonoBehaviour, ISCInit

{
    private int currentPartID = -1;

    public void Init(StageController sc)
    {
        if(ProjectPreferences.instance.gameMode != GameMode.Testing 
            || ProjectPreferences.instance.assemblyType != GameAssemblyType.Assembly)
        {
            gameObject.SetActive(false);
            return;
        }
        sc.OnStageSwitch += SwitchTarget;
    }

    private void SwitchTarget(Stage stage)
    {
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
            }
        }
    }
}

