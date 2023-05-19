using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VREventArgs;

public class ActionHandler : MonoBehaviour, ITargetable
{
    public string actionCode = "None";

    public UnityAction OnSetTarget;

    public void ActionTrigger()
    {
        StageController.OnActionTaken.Invoke(new(this, actionCode));
    }

    public void SetAsTarget(TargetSetEventArgs e)
    {
        OnSetTarget?.Invoke();
    }
}
