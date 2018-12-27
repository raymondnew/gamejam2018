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
    }

    void HandleCommandWaypoints()
    {
        if (m_GoCommandWaypointList.Count < 1)
            return;

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
        base.Dead();
    }

    IEnumerator ProcessThreats()
    {
        while (true)
        {
            if (m_CurrentState == AgentState.Holding || m_CurrentState == AgentState.Moving)
            {
                m_EnemiesList = m_Pawn.GetPawnsInLOS(AgentFaction.RED);

                // If enemies are spotted, go into engage mode
                if (m_EnemiesList.Count > 0)
                {
                    m_CurrentState = AgentState.Engaging;
                    m_Pawn.Halt();
                }
            }

            if (m_CurrentState == AgentState.Engaging)
            {
                // TODO: ENGAGE ENEMIES
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
