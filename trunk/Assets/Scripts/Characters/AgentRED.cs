using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRED : Agent
{
    [System.Serializable]
    struct EnemyWaypoint
    {
        public float wait;
        public Vector3 waypoint;
    }

    private List<EnemyWaypoint> m_EnemyWaypointList = new List<EnemyWaypoint>();

    private List<Pawn> m_EnemiesList;

    [SerializeField]
    Transform m_TempWaypointListParent;

    protected override void Awake()
    {
        PlanningManager.OnBegin += Begin;
        base.Awake();

        m_Pawn.faction = Agent.AgentFaction.RED;

        if (m_TempWaypointListParent != null)
        {
            foreach (Transform trans in m_TempWaypointListParent)
            {
                Level_Waypoint_RED WP = trans.GetComponent<Level_Waypoint_RED>();
                if (WP != null)
                {
                    EnemyWaypoint newWP;
                    newWP.wait = WP.waitDuration;
                    newWP.waypoint = trans.position;

                    m_EnemyWaypointList.Add(newWP);
                }
            }
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        m_Pawn.RegisterAsRED();
        StartCoroutine(ProcessThreats());
        base.Start();
    }

    int m_CurrentPatrolNode = 0;
    // Update is called once per frame
    protected override void Update()
    {
        if (m_CurrentState == AgentState.Moving)
            HandlePatrolPoints();
    }

    void HandlePatrolPoints()
    {
        if (m_EnemyWaypointList.Count < 1)
            return;

        EnemyWaypoint waypoint = m_EnemyWaypointList[m_CurrentPatrolNode];
        ProcessWaypoint(waypoint);
    }

    void ProcessWaypoint(EnemyWaypoint waypoint)
    {
        m_CurrentState = AgentState.Moving;
        m_Pawn.MoveTo(waypoint.waypoint);

        if (Vector3.Distance(transform.position, waypoint.waypoint) < 0.2f)
        {
            m_CurrentState = AgentState.Holding;
            StartCoroutine(ProcessEndOfWaypoint(waypoint));
        }
    }

    IEnumerator ProcessEndOfWaypoint(EnemyWaypoint waypoint)
    {
        if (waypoint.wait > 0.0f)
            yield return new WaitForSeconds(waypoint.wait);

        m_CurrentPatrolNode = (m_CurrentPatrolNode == (m_EnemyWaypointList.Count - 1)) ? 0 : m_CurrentPatrolNode + 1;
        EnemyWaypoint nextWaypoint = m_EnemyWaypointList[m_CurrentPatrolNode];
        ProcessWaypoint(nextWaypoint);
    }

    IEnumerator ProcessThreats()
    {
        while (true)
        {
            if (m_CurrentState == AgentState.Holding || m_CurrentState == AgentState.Moving)
            {
                m_EnemiesList = m_Pawn.GetPawnsInLOS(AgentFaction.BLUE);

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

    override protected void Begin()
    {
        m_CurrentState = AgentState.Moving;
    }
}
