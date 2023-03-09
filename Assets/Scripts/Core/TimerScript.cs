using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerScript
{
    private static int currTime = 0;
    private static Coroutine c_timer = null;
    private static bool stopTimer = false;

    private static readonly int backTimerInit = 2700;

    public static int CurrentTime => currTime;

    public static string CurrentTimeFormat
    {
        get
        {
            return $"{currTime / 600}{(currTime / 60) % 10}:{(currTime % 60) / 10}{(currTime % 60) % 10}";
        }
    }

    public static string CurrentTimeVerticalFormat
    {
        get 
        {
            return $"{currTime / 600}{(currTime / 60) % 10}\n{(currTime % 60) / 10}{(currTime % 60) % 10}";
        }
    }

    public static bool IsRunning => c_timer != null;

    public static void StartTimer(GameObject initializer)
    {
        if (initializer == null) 
        {
            Debug.LogError("Trying to initialize timer using null object."); return;
        }
        if (IsRunning)
        {
            Debug.LogWarning("Trying to start timer while it's running.", initializer);
            StopTimer(initializer);
        }
        currTime = ProjectPreferences.instance.gameMode == GameMode.Testing ? backTimerInit : 0;
        stopTimer = false;
        c_timer = initializer.GetComponent<MonoBehaviour>().StartCoroutine(TimerCoroutine());
    }

    public static void StopTimer(GameObject initializer)
    {
        if (initializer == null)
        {
            Debug.LogError("Trying to stop timer using null object."); return;
        }
        if (!IsRunning) Debug.LogWarning("Trying to stop null-timer.", initializer);
        stopTimer = true;
        initializer.GetComponent<MonoBehaviour>().StopCoroutine(TimerCoroutine());
        c_timer = null;        
    }

    private static IEnumerator TimerCoroutine()
    {
        int dt = ProjectPreferences.instance.gameMode == GameMode.Testing ? 1 : -1;
        while(!stopTimer)
        {
            yield return new WaitForSeconds(1f);
            currTime = Mathf.Clamp(currTime + dt, 0, int.MaxValue);
        }
        yield return null;
    }
}
