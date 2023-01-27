using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageControllerPresenter : Collegue
{
    public UnityAction<CommandFinished> OnPartFinished;

    public StageControllerPresenter(Mediator mediator) : base(mediator) { }

    public override void Send(Command command, Collegue target)
    {
        mediator.Send(command, target);
    }

    public override void Notify(Command command)
    {
        if (command is CommandFinished)
        {
            OnPartFinished?.Invoke(command as CommandFinished);
        }
    }
}
