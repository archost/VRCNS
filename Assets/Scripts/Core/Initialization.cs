using System;

public class Initialization
{
    public static Initialization Instance = null;

    private float progress;

    public bool IsDone { get; private set; }

    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            if (value >= 1f)
            {
                IsDone = true;
                progress = 1f;
            }
            else progress = value;
        }
    }

    public Initialization()
    {
        if (Instance == null) Instance = this;
        progress = 0.0f;
    }
}
