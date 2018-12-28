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

    [SerializeField]
    Transform m_TempWaypointListParent;

    bool m_CommandWaypointsSet = false;

    protected override void Awake()
    {
        PlanningManager.OnGoCommand += HandleOnGoCommand;
        PlanningManager.OnBegin += Begin;
        base.Awake();

        m_Pawn.faction = AgentFaction.BLUE;

        if(m_TempWaypointListParent != null)
        {
            UI_Waypoints newWaypointsData = new UI_Waypoints();
            newWaypointsData.m_waypoints = new List<UI_Waypoints.Waypoint>();
            foreach (Transform trans in m_TempWaypointListParent)
            {
                Level_Waypoint_BLUE WP = trans.GetComponent<Level_Waypoint_BLUE>();
                if (WP != null)
                {
                    UI_Waypoints.Waypoint newWPdata;
                    newWPdata.m_goCommand = WP.m_GoCommand;
                    newWPdata.waypoint = trans.position;
                }
            }

            SetupWaypoints(newWaypointsData);
        }
    }

    public void SetupWaypoints(UI_Waypoints waypointData)
    {
        if (!m_CommandWaypointsSet)
            return;

        m_GoCommandWaypointList.Clear();
        foreach (UI_Waypoints.Waypoint wp in waypointData.m_waypoints)
        {
            if (wp.m_goCommand > PlanningManager.Instance.GoCommandsCount)
                PlanningManager.Instance.AddNewGoCommand(wp.m_goCommand - PlanningManager.Instance.GoCommandsCount + 1);
            GoCommandWaypoint newWP;
            newWP.goCommand = wp.m_goCommand;
            newWP.waypoint = wp.waypoint;
            m_GoCommandWaypointList.Add(newWP);
        }

        m_CommandWaypointsSet = true;
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
        base.Update();

        if (m_CurrentState == AgentState.End)
            return;

        if (IsDead)
            return;

        if (m_CurrentState == AgentState.Moving)
            HandleCommandWaypoints();
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
        base.Begin();
    }

    protected override void Dead()
    {
        PlanningManager.OnGoCommand -= HandleOnGoCommand;
        LevelManager.RemoveBLUE(m_Pawn);
        base.Dead();
    }
}
