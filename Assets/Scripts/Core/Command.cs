using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public Collegue Sender { get; private set; }

    public Command (Collegue sender)
    {
        this.Sender = sender;
    }
}

public class CommandFinished : Command
{
    public CommandFinished(Collegue sender) : base(sender)
    {

    }
}

public class CommandToogleJP : Command
{
    public int JointPointID { get; private set; }

    public CommandToogleJP(Collegue sender,  int jointPointID) : base(sender)
    {
        JointPointID = jointPointID;
    }
}
