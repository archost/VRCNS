using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartPresenter : Collegue
{
    public UnityAction<int> OnJointPointToogle;

    public PartData PartData { get; private set; }

    public PartPresenter(Mediator mediator, PartData pd) : base(mediator)
    {
        PartData = pd;
    }

    public override void Send(Command command, Collegue target)
    {
        mediator.Send(command, target);
    }

    public override void Notify(Command command)
    {
        if (command is CommandToogleJP)
        {
            CommandToogleJP commandToogleJP = command as CommandToogleJP;
            OnJointPointToogle?.Invoke(commandToogleJP.JointPointID);
        }
    }
}
