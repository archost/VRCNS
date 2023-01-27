using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectPreferences : MonoBehaviour
{
    public static ProjectPreferences instance = null;

    [Header("Outline Settings")]
    public Color outlineColor;
    public float outlineWidth = 2f;

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
