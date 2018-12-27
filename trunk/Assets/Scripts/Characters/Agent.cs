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

    [SerializeField]
    protected MainWeapon m_MainWeapon;

    public bool IsDead { get; private set; } = false;

    protected virtual void Awake()
    {
        m_Pawn = GetComponent<Pawn>();
    }

    protected virtual void Update()
    {
        if (m_Pawn.HP < 0.0f)
            Dead();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    protected virtual void Dead()
    {
        IsDead = true;
        m_Pawn.IsDead = true;

        gameObject.SetActive(false);
    }

    virtual protected void Begin()
    {
    }

    protected void Shoot(Pawn target, float deltaTime)
    {
        float dmg = deltaTime * m_MainWeapon.rof;
        m_Pawn.ShootAt(target, dmg);
    }
}