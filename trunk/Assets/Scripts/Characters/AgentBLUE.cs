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
        m_Faction = AgentFaction.BLUE;
        m_Pawn.RegisterAsBLUE();
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

    override protected void Begin()
    {
        m_CurrentState = AgentState.Holding;
    }

    protected override void Dead()
    {
        PlanningManager.OnGoCommand -= HandleOnGoCommand;
        LevelManager.RemoveBLUE(m_Pawn);
        base.Dead();
    }
}
