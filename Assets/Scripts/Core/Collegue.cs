using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collegue 
{
    protected Mediator mediator;

    public Collegue(Mediator mediator)
    {
        this.mediator = mediator;
    }

    public abstract void Send(Command command, Collegue target);

    public abstract void Notify(Command command);
}
