using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ForbiddenArea : MonoBehaviour, ISCInit
{
    [SerializeField]
    private Stage toBeActivatedOn;

    public void Init(StageController sc)
    {
        if (ProjectPreferences.instance.IsTraining) return;
        sc.OnStageSwitch += OnStageSwitch;
    }

    private void OnStageSwitch(Stage s)
    {
        gameObject.SetActive(s.ID == toBeActivatedOn.ID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var p))
        {
            StageController.OnMadeMistake(new(this));
        }
    }
}
