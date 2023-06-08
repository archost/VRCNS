using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREventArgs;

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
        if(s == null) return;
        gameObject.SetActive(s.ID == toBeActivatedOn.ID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out var p))
        {
            StageController.OnMadeMistake(new ForbiddenAreaEventArgs(this));
        }
    }
}
