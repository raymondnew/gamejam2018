using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager g_Inst;

    private static bool g_Paused;

    public static float DeltaTime { get { return (g_Paused) ? 0.0f : UnityEngine.Time.deltaTime; } }
    public static float Time { get { return g_Inst.m_Time; } }
    public static bool IsPaused { get { return g_Paused; } }

    public delegate void pauseHandler();
    static public event pauseHandler OnPause;
    static public event pauseHandler OnUnPause;

    float m_Time = -1.0f;

    private void Awake()
    {
        g_Inst = this;
        g_Paused = false;
    }

    public static void Pause()
    {
        g_Paused = true;
        OnPause?.Invoke();
    }

    public static void Play()
    {
        g_Paused = false;
        OnUnPause?.Invoke();
    }

    private void Update()
    {
        if (g_Paused)
            return;

        if (m_Time < 0.0f)
            m_Time = UnityEngine.Time.time;
        else
            m_Time += DeltaTime;
    }
}
