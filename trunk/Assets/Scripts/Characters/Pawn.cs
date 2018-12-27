using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pawn : MonoBehaviour
{
    private NavMeshAgent m_NavAgent;

    [SerializeField]
    float m_FOV;

    [SerializeField]
    float m_MaxRange;

    [SerializeField]
    float m_MinRange;

    [HideInInspector]
    public Agent.AgentFaction faction;

    public int Floor { get; private set; } = -1;

    void Awake()
    {
        InitAgent();
    }

    void Start()
    {
        Floor = GetFloorLevel();
    }

    public void RegisterAsRED()
    {
        Floor = GetFloorLevel();
        LevelManager.RegisterRED(this);
    }

    public void RegisterAsBLUE()
    {
        Floor = GetFloorLevel();
        LevelManager.RegisterBLUE(this);
    }

    void Update()
    {
        UpdateFloor();

        if (Vector3.Distance(m_NavAgent.destination, transform.position) < 0.2f)
        {
            transform.position = m_NavAgent.destination;
            Halt();
        }
    }

    void InitAgent()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_NavAgent.destination = transform.position;
    }

    public void MoveTo(Vector3 pos)
    {
        if (m_NavAgent.destination == pos)
            return;

        m_NavAgent.SetDestination(pos);
    }

    public void Halt()
    {
        m_NavAgent.destination = transform.position;
    }

    public List<Pawn> GetPawnsInLOS(Agent.AgentFaction faction)
    {
        List<Pawn> pawnList = new List<Pawn>();
        List<Pawn> currList = (faction == Agent.AgentFaction.RED) ? LevelManager.GetREDPawnsByFloor(Floor) : LevelManager.GetBLUEPawnsByFloor(Floor);

        if (currList == null)
            return pawnList;

        for(int i = 0; i < currList.Count; i++)
        {
            Pawn pawn = currList[i];
            Vector3 yOffset = new Vector3(0.0f, 0.25f, 0.0f);

            // Ignore if including yourself
            if (pawn == this)
                continue;

            // Check if in FOV
            Vector3 dirToPawn = (pawn.transform.position - transform.position).normalized;
            float angleDiff = Vector3.Angle(dirToPawn, transform.forward);
            if (angleDiff > (m_FOV * 0.5f))
                continue;

            // Check if in LOS
            Debug.DrawRay(transform.position + (transform.forward * m_MinRange) + yOffset, dirToPawn * m_MaxRange, Color.blue);
            bool noLOS = true;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + (transform.forward * m_MinRange) + yOffset, dirToPawn, out hitInfo, m_MaxRange, LevelManager.LOS_Layer))
            {
                Transform hitTransform = hitInfo.transform;
                while (hitTransform != null && hitTransform.gameObject != pawn.gameObject)
                    hitTransform = hitTransform.parent;

                noLOS = (hitTransform == null);
            }

            if (!noLOS)
            {
                pawnList.Add(pawn);
                Debug.Log(name + " seeing " + pawn.name + " at " + angleDiff + " angle and " + Vector3.Distance(transform.position, pawn.transform.position) + " distance.");
            }
        }


        return pawnList;
    }

    void UpdateFloor()
    {
        int floorLevel = GetFloorLevel();
        if (Floor != floorLevel)
        {
            if(faction == Agent.AgentFaction.RED)
                LevelManager.UpdateREDPawn(this, floorLevel);
            else
                LevelManager.UpdateBLUEPawn(this, floorLevel);
            Floor = floorLevel;
        }
    }

    int GetFloorLevel()
    {
        if (Level_Base.FloorHeight == 0)
            return 0;

        return Mathf.RoundToInt(transform.position.y / Level_Base.FloorHeight);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 yOffset = new Vector3(0.0f, 0.25f, 0.0f);
        Vector3 leftDir = (Quaternion.AngleAxis(-(m_FOV * 0.5f), Vector3.up) * transform.forward).normalized;
        Vector3 rightDir = (Quaternion.AngleAxis(m_FOV * 0.5f, Vector3.up) * transform.forward).normalized;
        Gizmos.DrawLine(transform.position + (leftDir * m_MinRange) + yOffset, transform.position + (leftDir * m_MaxRange) + yOffset);
        Gizmos.DrawLine(transform.position + (rightDir * m_MinRange) + yOffset, transform.position + (rightDir * m_MaxRange) + yOffset);
    }
}
