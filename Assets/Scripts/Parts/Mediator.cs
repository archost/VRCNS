using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mediator
{
    public Collegue StageControllerPresenter { get; set; }

    private List<PartPresenter> parts = new List<PartPresenter>();

    private List<ActionHandlerPresenter> actionHandlers = new List<ActionHandlerPresenter>();

    public void AddPart(PartPresenter part)
    {
        parts.Add(part);
    }

    public void AddActionHandler(ActionHandlerPresenter actionHandler)
    {
        actionHandlers.Add(actionHandler);
    }

    public void Send(Command command, Collegue target)
    {
        if (command is CommandFinished || command is CommandHelperUpdate)
        {
            StageControllerPresenter.Notify(command);
        }
        else if (command is CommandSetTarget)
        {
            var c = command as CommandSetTarget;
            foreach (var i in parts)
            {
                if (i.PartData.ID == c.TargetData.ID)
                {
                    i.Notify(command);
                }
            }
        }
        else if (command is CommandSetTargetAction)
        {
            var c = command as CommandSetTargetAction;
            foreach (var i in actionHandlers)
            {
                if (i.ActionCode == c.ActionCode)
                {
                    i.Notify(command);
                }
            }
        }
        else
        {
            PartPresenter p = null;
            foreach (var part in parts)
            {
                if (target == part)
                {
                    p = part;
                    break;
                }
            }
            if (p == null)
            {
                Debug.Log("Unknown Collegue target!");
            }
            else p.Notify(command);
        }
    }
}
