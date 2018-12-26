using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pawn : MonoBehaviour
{
    private NavMeshAgent m_NavAgent;

    void Awake()
    {
        InitAgent();
    }

    void InitAgent()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_NavAgent.destination = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveTo(Vector3 pos)
    {
        Debug.Log("Move to: " + pos);
        m_NavAgent.destination = pos;
    }
}
