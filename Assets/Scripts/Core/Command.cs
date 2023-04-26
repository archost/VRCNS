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

public class CommandHelperUpdate : Command
{
    public Transform PartTransform { get; private set; }

    public PartData PartData { get; private set; }

    public bool IsSelected { get; private set; }

    public CommandHelperUpdate(Collegue sender, Transform t, PartData pd, bool isSelected) : base(sender)
    {
        PartTransform = t;
        PartData = pd;
        IsSelected = isSelected;
    }
}

public class CommandSetTarget : Command
{
    public PartData TargetData { get; private set; }

    public PartState? NewState { get; private set; }

    public GameAssemblyType AssemblyType { get; private set; }

    public CommandSetTarget(Collegue sender, PartData targetData, GameAssemblyType assemblyType, PartState? newState = null) : base(sender)
    {
        TargetData = targetData;
        AssemblyType = assemblyType;
        NewState = newState;
    }
}

public class CommandSetTargetAction : Command
{
    public string ActionCode { get; private set; }

    public CommandSetTargetAction(Collegue sender, string actionCode) : base(sender)
    {
        ActionCode = actionCode;
    }
}

public class CommandActionFinished : CommandFinished
{
    public string ActionCode { get; private set; }

    public CommandActionFinished(Collegue sender, string actionCode) : base(sender)
    {
        ActionCode = actionCode;
    }
}

public class CommandProcessMistake : Command
{
    public CommandProcessMistake(Collegue sender) : base(sender)
    {

    }
}
