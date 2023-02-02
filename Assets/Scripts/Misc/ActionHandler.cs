using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionHandler : MonoBehaviour
{
    private ActionHandlerPresenter presenter = null;

    public string actionCode = "None";

    public UnityAction OnSetTarget;

    public ActionHandlerPresenter InitPresenter(Mediator mediator)
    {
        presenter = new ActionHandlerPresenter(mediator, actionCode);
        presenter.OnTargetSet += SetTarget;
        return presenter;
    }

    private void SetTarget()
    {
        OnSetTarget?.Invoke();
    }

    public void ActionTrigger()
    {
        if (presenter == null) return;
        presenter.Send(new CommandActionFinished(presenter, actionCode), null);
    }
}
