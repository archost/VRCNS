using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mediator
{
    public Collegue StageControllerPresenter {get; set;}

    private List<PartPresenter> parts = new List<PartPresenter>();

    public void AddPart(PartPresenter part)
    {
        parts.Add(part);
    }

    public void Send(Command command, Collegue target)
    {
        if (command is CommandFinished)
        {
            StageControllerPresenter.Notify(command);
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
