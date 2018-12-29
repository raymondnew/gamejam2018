using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager g_Inst;

    private static bool g_Paused;

    public static float DeltaTime { get { return (g_Paused) ? 0.0f : UnityEngine.Time.deltaTime; } }
    public static float Time { get { return g_Inst.m_Time; } }

    float m_Time = -1.0f;

    private void Awake()
    {
        g_Inst = this;
    }

    public static void Pause()
    {
        g_Paused = true;
    }

    public static void Play()
    {
        g_Paused = false;
    }

    private void Update()
    {
        if (m_Time < 0.0f)
            m_Time = UnityEngine.Time.time;
        else
            m_Time += DeltaTime;
    }
}
