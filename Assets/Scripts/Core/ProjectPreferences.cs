using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectPreferences : MonoBehaviour
{
    public static ProjectPreferences instance = null;

    [Header("Outline Settings")]
    public Color highlightOutlineColor;
    public Color wrongOutlineColor;
    public float outlineWidth = 2f;

    [Header("Current Play Settings")]
    public GameMode gameMode;
    public GameAssemblyType assemblyType;
    public bool multiErrorAllowed;
    public int maxScore;


    [Header("Testing Options")]
    public bool VRTesting = false;
    public bool AssistantVoice = true;

    public bool IsTesting => gameMode == GameMode.Testing;

    public bool IsTraining => gameMode == GameMode.Training;

    public bool IsAssembly => assemblyType == GameAssemblyType.Assembly;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

public enum GameMode
{
    Training,
    Testing
}

public enum GameAssemblyType
{
    Assembly,
    Disassembly
}
