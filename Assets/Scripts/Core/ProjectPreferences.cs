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


    [Header("Testing Options")]
    public bool VRTesting = false;

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
