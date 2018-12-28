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

    protected AgentFaction m_Faction;

    [SerializeField]
    protected MainWeapon m_MainWeapon;
    public MainWeapon MainWeapon { get { return m_MainWeapon; } }

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
        StartCoroutine(ProcessThreats());
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




    // COMBAT
    private List<Pawn> m_EnemiesList;

    protected void Shoot(Pawn target, float deltaTime)
    {
        //float dmg = deltaTime * m_MainWeapon.rof * m_MainWeapon.damage;
        m_Pawn.ShootAt(target, m_MainWeapon.rof);
    }

    protected void CeaseFire()
    {
        m_Pawn.StopShooting();
    }

    Pawn m_Target = null;
    float m_TimeToAim = 0.0f;

    [SerializeField]
    float m_AimTime = 1.0f;
    IEnumerator ProcessThreats()
    {
        float m_Time = Time.time;
        float m_LastTime = m_Time;
        while (true)
        {
            float deltaTime = Time.time - m_LastTime;
            // Find enemies in view
            if (m_Target == null || !m_Pawn.HasLOS(m_Target))
            {
                m_EnemiesList = m_Pawn.GetPawnsInLOS((m_Faction == AgentFaction.BLUE) ? AgentFaction.RED : AgentFaction.BLUE);
                if (m_EnemiesList.Count > 0)
                    SetTarget(m_EnemiesList[0]);
            }

            // If target found, go into engage mode
            if (m_Target != null)
            {
                transform.LookAt(m_Target.transform);
                m_CurrentState = AgentState.Engaging;
                m_Pawn.Halt();

                // TODO: ENGAGE ENEMIES
                // If dead, move on and acquire new target
                if (m_Target.IsDead)
                {
                    m_Target = null;
                    m_CurrentState = AgentState.Moving;
                    CeaseFire();
                    continue;
                }

                // If not dead, aim towards target
                m_TimeToAim = Mathf.Max(0.0f, m_TimeToAim - deltaTime);

                // Once aim is established, shoot at target
                if (m_TimeToAim == 0.0f)
                    Shoot(m_Target, deltaTime);

                m_LastTime = Time.time;
                yield return null;
            }
            else
            {
                m_LastTime = Time.time;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void SetTarget(Pawn target)
    {
        m_Target = target;
        m_TimeToAim = m_AimTime;
    }
}