using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBLUE : Agent
{
    [System.Serializable]
    struct GoCommandWaypoint
    {
        public int goCommand;
        public Vector3 waypoint;
    }

    private List<GoCommandWaypoint> m_GoCommandWaypointList = new List<GoCommandWaypoint>();

    private List<Pawn> m_EnemiesList;

    [SerializeField]
    Transform m_TempWaypointListParent;

    protected override void Awake()
    {
        PlanningManager.OnGoCommand += HandleOnGoCommand;
        PlanningManager.OnBegin += Begin;
        base.Awake();

        m_Pawn.faction = AgentFaction.BLUE;

        if(m_TempWaypointListParent != null)
        {
            foreach (Transform trans in m_TempWaypointListParent)
            {
                Level_Waypoint_BLUE WP = trans.GetComponent<Level_Waypoint_BLUE>();
                if (WP != null)
                {
                    GoCommandWaypoint newWP;
                    newWP.goCommand = WP.m_GoCommand;
                    newWP.waypoint = trans.position;

                    m_GoCommandWaypointList.Add(newWP);
                }
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        m_Pawn.RegisterAsBLUE();
        StartCoroutine(ProcessThreats());
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (m_CurrentState == AgentState.Moving)
            HandleCommandWaypoints();

        base.Update();
    }

    void HandleCommandWaypoints()
    {
        if (m_GoCommandWaypointList.Count < 1)
        {
            m_CurrentState = AgentState.Holding;
            return;
        }

        GoCommandWaypoint currWaypoint = m_GoCommandWaypointList[0];
        if (PlanningManager.Instance.CurrentGoCommand < currWaypoint.goCommand)
            m_CurrentState = AgentState.Holding;
        else
            ProcessWaypoint(currWaypoint);
    }

    void ProcessWaypoint(GoCommandWaypoint waypoint)
    {
        m_CurrentState = AgentState.Moving;
        m_Pawn.MoveTo(waypoint.waypoint);

        if (Vector3.Distance(transform.position, waypoint.waypoint) < 0.2f)
            m_GoCommandWaypointList.Remove(waypoint);
    }

    void HandleOnGoCommand(int goCommand)
    {
        if (m_CurrentState == AgentState.Holding)
            HandleCommandWaypoints();
    }

    protected override void Dead()
    {
        PlanningManager.OnGoCommand -= HandleOnGoCommand;
        LevelManager.RemoveBLUE(m_Pawn);
        base.Dead();
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
                m_EnemiesList = m_Pawn.GetPawnsInLOS(AgentFaction.RED);
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

    override protected void Begin()
    {
        m_CurrentState = AgentState.Holding;
    }
}
