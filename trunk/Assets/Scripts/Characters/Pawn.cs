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

    public int Floor { get; private set; } = -1;

    void Awake()
    {
        InitAgent();
    }

    void Start()
    {
        Floor = GetFloorLevel();
        LevelManager.RegisterPawn(this);
    }

    void Update()
    {
        UpdateFloor();
        GetPawnsInLOS();
    }

    void InitAgent()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_NavAgent.destination = transform.position;
    }

    public void MoveTo(Vector3 pos)
    {
        m_NavAgent.SetDestination(pos);
    }

    public void GetPawnsInLOS()//out List<Pawn> pawnList)
    {
        List<Pawn> pawnList = LevelManager.GetPawnsByFloor(Floor);

        foreach (Pawn pawn in pawnList)
        {
            // Check if in FOV
            Vector3 dirToPawn = (pawn.transform.position - transform.position).normalized;
            float angleDiff = Vector3.Angle(dirToPawn, pawn.transform.forward);

            Debug.Log(name + " angle to " + pawn.name + ": " + angleDiff);
        }
    }

    void UpdateFloor()
    {
        int floorLevel = GetFloorLevel();
        if (Floor != floorLevel)
        {
            LevelManager.UpdatePawn(this, floorLevel);
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
        Gizmos.DrawFrustum(transform.position, m_FOV, m_MaxRange, m_MinRange, 1.0f);
    }
}
