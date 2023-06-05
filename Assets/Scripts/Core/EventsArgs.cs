using UnityEngine;

namespace VREventArgs
{
    public abstract class EventArgs
    {
        public object Sender { get; private set; }

        public EventArgs(object sender)
        {
            Sender = sender;
        }
    }

    public class PartInstalledEventArgs : EventArgs
    {
        public PartInstalledEventArgs(object sender) : base(sender) { }
    }

    public class MistakeEventArgs : EventArgs
    {
        public MistakeEventArgs(object sender) : base(sender) { }
    }

    public class PartSelectedEventArgs : EventArgs
    {
        public Transform PartTransform { get; private set; }

        public PartData PartData { get; private set; }

        public bool IsSelected { get; private set; }

        public PartSelectedEventArgs(object sender, Transform partTransform, PartData partData, bool isSelected) : base(sender)
        {
            PartTransform = partTransform;
            PartData = partData;
            IsSelected = isSelected;
        }
    }

    public class ActionTakenEventArgs : EventArgs
    {
        public string ActionCode { get; private set; }

        public ActionTakenEventArgs(object sender, string actionCode) : base(sender)
        {
            ActionCode = actionCode;
        }
    }

    public abstract class TargetSetEventArgs : EventArgs
    {
        public TargetSetEventArgs(object sender) : base(sender) { }
    }

    public class PartSetAsTargetEventArgs : TargetSetEventArgs
    {
        public PartData TargetData { get; private set; }

        public GameAssemblyType AssemblyType { get; private set; }

        public PartSetAsTargetEventArgs(object sender, PartData targetData, GameAssemblyType assemblyType) : base(sender)
        {
            TargetData = targetData;
            AssemblyType = assemblyType;
        }
    }

    public class ActionSetAsTargetEventArgs : TargetSetEventArgs
    {
        public string ActionCode { get; private set; }

        public ActionSetAsTargetEventArgs(object sender, string actionCode) : base(sender)
        {
            ActionCode = actionCode;
        }
    }

    public class PartAttachRequestEventArgs : EventArgs
    {
        public Part Part { get; private set; }

        public Vector3 FixedPosition { get; private set; }

        public Quaternion FixedRotation { get; private set; }

        public bool ToBeInstalled { get; private set; }

        public PartAttachRequestEventArgs(object sender, Part part, Vector3 pos, Quaternion rot, bool toInstall) : base(sender)
        {
            Part = part;
            FixedPosition = pos;
            FixedRotation = rot;
            ToBeInstalled = toInstall;
        }
    }

    public class PartClickedEventArgs : EventArgs
    {
        public Part Part { get; private set; }

        public PartClickedEventArgs(object sender, Part invoker) : base(sender)
        {
            Part = invoker;
        }
    }
}