using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePawn : MonoBehaviour
{
    [SerializeField]
    CapsuleCollider m_collider;

    private RaycastHit hit;
    private UI_WaypointList ui_WaypointList = new UI_WaypointList();
    private Ray ray;
    private Vector3 keepGrounded = new Vector3(1, 0, 1);
    private string pawnName;
    private UI_Planning ui_plan;
    private UI_UnitSelect unitSelect;

    // Start is called before the first frame update
    void Start()
    {
        GetWaypointList();
        PlanningManager.OnBegin += DestroySelf;

        unitSelect = FindObjectOfType<UI_UnitSelect>();

    }

    void DestroySelf()
    {
        PlanningManager.OnBegin -= DestroySelf;
        Destroy(this);
    }

    private void GetWaypointList()
    {
        pawnName = GetComponent<Pawn>().m_name;
        ui_plan = FindObjectOfType<UI_Planning>();
        ui_WaypointList = ui_plan.GetUIWaypointMember(pawnName);

    }

    private void MoveThePawn()
    {
        //ui_WaypointList.m_waypoints[0].position = Vector3.Scale(hit.transform.position,keepGrounded);
        Vector3 newPos = Vector3.Scale(hit.transform.position, keepGrounded);
        List<UI_WaypointList.Waypoint> list = ui_WaypointList.m_waypoints;
        UI_WaypointList.Waypoint wp = list[0];
        wp.position = newPos;

        ui_plan.ReplaceUIWaypoint(wp, pawnName);

        if (list.Count > 1)
        {
            ui_plan.StartLine(list[1], list[0]);
        }

    }

    private IEnumerator movePosition()
    {
        while (Input.GetMouseButton(0) && !GameMenu.IsOn)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            transform.position = Vector3.Scale(hit.point, keepGrounded);
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = transform.position;
            
            yield return null;
        }

        MoveThePawn();

    }

    private void Selector()
    {
        unitSelect.AddSelection(transform);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !GameMenu.IsOn)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit))
            {
                
                if (hit.collider == m_collider)
                {
                    unitSelect.RemoveSelection();

                    

                    Selector();
                    StartCoroutine(movePosition());

                    //transform.position = Vector3.Scale(hit.transform.position,keepGrounded);
                }
                else if (transform == unitSelect.currentTransform)
                {
                    unitSelect.RemoveSelection(transform);
                }
            }
        }
    }
}
