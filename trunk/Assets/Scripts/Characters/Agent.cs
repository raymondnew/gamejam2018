using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pawn))]
public class Agent : MonoBehaviour
{
    public enum AgentFaction
    {
        BLUE,
        RED
    }

    protected enum AgentState
    {
        Staging,
        Holding,
        Moving,
        Engaging
    }

    protected AgentState m_CurrentState = AgentState.Staging;

    protected Pawn m_Pawn;

    protected virtual void Awake()
    {
        m_Pawn = GetComponent<Pawn>();
    }

    protected virtual void Update()
    {
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    protected virtual void Dead()
    {
    }

    virtual protected void Begin()
    {
    }
}