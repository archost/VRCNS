using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionHandlerPresenter : Collegue
{
    public UnityAction OnTargetSet;

    public string ActionCode { get; private set; }

    public ActionHandlerPresenter(Mediator mediator, string actionCode) : base(mediator) 
    { 
        ActionCode = actionCode;
    }

    public override void Notify(Command command)
    {
        if (command is CommandSetTargetAction) 
        {
            OnTargetSet?.Invoke();
        }
    }

    public override void Send(Command command, Collegue target)
    {
        mediator.Send(command, target);
    }
}
