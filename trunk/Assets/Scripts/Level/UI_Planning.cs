using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UI_Planning : MonoBehaviour
{

    UI_Waypoints m_alphaOne = new UI_Waypoints();
    UI_Waypoints m_alphaTwo = new UI_Waypoints();
    UI_Waypoints m_alphaThree = new UI_Waypoints();
    UI_Waypoints m_alphaFour = new UI_Waypoints();
    UI_Waypoints m_bravoOne = new UI_Waypoints();
    UI_Waypoints m_bravoTwo = new UI_Waypoints();
    UI_Waypoints m_bravoThree = new UI_Waypoints();
    UI_Waypoints m_bravoFour = new UI_Waypoints();

    [SerializeField]
    List<Pawn> m_ListOfFriendlyPawns = new List<Pawn>();

    [SerializeField]
    GameObject m_waypointGameObjectHolder;

    [SerializeField]
    CanvasGroup m_Planning_UI;

    private bool init = false;

    public string m_selector;
    public UI_Waypoints m_uiwaypoint;
    public GameObject m_waypointPrefab;

    public void InitPlanning()
    {
        if (m_ListOfFriendlyPawns.Count == 0)
        {
            Debug.LogError("No Friendly Pawns Found!");
            return;
        }

        init = true;


        foreach(Pawn element in m_ListOfFriendlyPawns)
        {
            m_selector = element.m_name;
            SetWaypoint(false);

        }
        init = false;
        m_selector = "";
        Debug.Log("UI_Planning Init Complete");

    }

    public void SetSelector(string selector)
    {
        m_selector = selector;

    }

    public UI_Waypoints GetUIWaypointMember()
    {

        switch (m_selector)
        {
            case "alphaOne":
                return m_alphaOne;
            case "alphaTwo":
                return m_alphaTwo;
            case "alphaThree":
                return m_alphaThree;
            case "alphaFour":
                return m_alphaFour;
            case "bravoOne":
                return m_bravoOne;
            case "bravoTwo":
                return m_bravoTwo;
            case "bravoThree":
                return m_bravoThree;
            case "bravoFour":
                return m_bravoFour;
            default:
                return new UI_Waypoints();
        }



    }

    public void GoCommand()
    {
        SetWaypoint(true);

    }




    public void SetWaypoint(bool goCommand)
    {
        UI_Waypoints selectedWaypoint;
        string waypointName = "";
        switch (m_selector)
        {
            case "alphaOne":
                waypointName = "A1";
                selectedWaypoint = m_alphaOne;
                break;
            case "alphaTwo":
                waypointName = "A2";
                selectedWaypoint = m_alphaTwo;
                break;
            case "alphaThree":
                waypointName = "A3";
                selectedWaypoint = m_alphaThree;
                break;
            case "alphaFour":
                waypointName = "A4";
                selectedWaypoint = m_alphaFour;
                break;
            case "bravoOne":
                waypointName = "B1";
                selectedWaypoint = m_bravoOne;
                break;
            case "bravoTwo":
                waypointName = "B2";
                selectedWaypoint = m_bravoTwo;
                break;
            case "bravoThree":
                waypointName = "B3";
                selectedWaypoint = m_bravoThree;
                break;
            case "bravoFour":
                waypointName = "B4";
                selectedWaypoint = m_bravoFour;
                break;
            default:
                return;
        };

        if(init)
        {
            UI_Waypoints.Waypoint initWaypointStruct = new UI_Waypoints.Waypoint();

   
            foreach (Pawn element in m_ListOfFriendlyPawns)
                if (element.m_name == m_selector)
                    initWaypointStruct.waypoint = element.transform.position;
            initWaypointStruct.m_goCommand = 0;
            selectedWaypoint.m_waypoints.Add(initWaypointStruct);
            return;

        }


        if (goCommand)
        {
            UI_Waypoints.Waypoint goWaypointStruct = new UI_Waypoints.Waypoint();
            goWaypointStruct.waypoint = new Vector3(-1,-1,-1);
            

            if (selectedWaypoint.m_waypoints.Count == 0)
                goWaypointStruct.m_goCommand = 0;
            else
                goWaypointStruct.m_goCommand = selectedWaypoint.m_waypoints[selectedWaypoint.m_waypoints.Count - 1].m_goCommand+1;

            selectedWaypoint.m_waypoints.Add(goWaypointStruct);
            return;
        }



        waypointName = waypointName + " - " + (selectedWaypoint.m_waypoints.Count).ToString();
        Vector3 waypoint = GetWaypoint();

        if (waypoint == Vector3.left)
            return;
        
        if (m_waypointGameObjectHolder == null)
        {
            Debug.LogError("WaypointGameObjectHolder not set!");
            return;
        }


        GameObject prefab = Instantiate(m_waypointPrefab, waypoint, Quaternion.identity,m_waypointGameObjectHolder.transform);

        prefab.GetComponent<LineRenderer>().positionCount = 0;

    
        

        UI_Waypoints.Waypoint waypointStruct = new UI_Waypoints.Waypoint();
        waypointStruct.waypoint = waypoint;
        waypointStruct.m_prefab = prefab.transform;
        if (selectedWaypoint.m_waypoints.Count == 0)
            waypointStruct.m_goCommand = 0;
        else
            waypointStruct.m_goCommand = selectedWaypoint.m_waypoints[selectedWaypoint.m_waypoints.Count - 1].m_goCommand;



        prefab.transform.Find("Name").GetComponent<TextMesh>().text = waypointName + "\n" + "Go: " + waypointStruct.m_goCommand.ToString();

        UI_Waypoints.Waypoint previous = new UI_Waypoints.Waypoint();
        previous.waypoint = new Vector3(-1, -1, -1);

        bool foundPreviousWaypoint = false;
        for(int i = selectedWaypoint.m_waypoints.Count -1; i >= 0; i--)
        {
            if (IsWaypoint(selectedWaypoint.m_waypoints[i]))
            {
                previous = selectedWaypoint.m_waypoints[i];
                foundPreviousWaypoint = true;
                i = 0;
            }
        }
        if (foundPreviousWaypoint)
            StartLine(waypointStruct,previous);

        selectedWaypoint.m_waypoints.Add(waypointStruct);

    }

    public bool IsWaypoint(UI_Waypoints.Waypoint test)
    {
        if (test.waypoint.x == -1f && test.waypoint.y == -1f && test.waypoint.z == -1f)
            return false;
        else
            return true;
    }

    public void StartLine(UI_Waypoints.Waypoint current , UI_Waypoints.Waypoint previous)
    {
        if (!IsWaypoint(previous))
            return;
        NavMeshAgent navmesh = current.m_prefab.GetComponent<NavMeshAgent>();
        NavMeshPath path = new NavMeshPath();
        if (navmesh.CalculatePath(previous.waypoint, path))
        {
            StartCoroutine(DrawLine(navmesh, path));
        }

        

    }

    public IEnumerator DrawLine(NavMeshAgent navmesh, NavMeshPath path)
    {
        while (path.status == NavMeshPathStatus.PathPartial)
            yield return null;

       


        navmesh.GetComponent<LineRenderer>().positionCount = path.corners.Length;
        

        for (int i = 0; i < path.corners.Length; i++)
        {
            navmesh.GetComponent<LineRenderer>().SetPosition(i, path.corners[i]);
        }
    }


    public void DeleteLast()
    {
        if (m_selector == "")
            return;
        UI_Waypoints deleteWaypoint = GetUIWaypointMember();

        if (deleteWaypoint.m_waypoints.Count == 1)
            return;

        UI_Waypoints.Waypoint deleter = deleteWaypoint.m_waypoints[deleteWaypoint.m_waypoints.Count - 1];

        if (deleteWaypoint.m_waypoints[deleteWaypoint.m_waypoints.Count - 1].waypoint.x == -1f)
            if (deleteWaypoint.m_waypoints[deleteWaypoint.m_waypoints.Count - 1].waypoint.y == -1f)
                if (deleteWaypoint.m_waypoints[deleteWaypoint.m_waypoints.Count - 1].waypoint.z == -1f)
                {
                    deleteWaypoint.m_waypoints.Remove(deleter);
                    return;
                }
        

        
        DeleteWaypoint(deleter);
        deleteWaypoint.m_waypoints.Remove(deleter);

    }

    public void DeleteWaypoint(UI_Waypoints.Waypoint waypoint)
    {
        Destroy(waypoint.m_prefab.gameObject);
    }

    [SerializeField]
    LayerMask walkableLayer;
    public Vector3 GetWaypoint()
    {

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
            if (hit.transform.gameObject.layer == 9) //walkable
                return hit.point;

        return new Vector3(-1, 0, 0);
        
        //return Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_selector = "";


        m_alphaOne.m_name = "alphaOne";
        m_alphaTwo.m_name = "alphaTwo";
        m_alphaThree.m_name = "alphaThree";
        m_alphaFour.m_name = "alphaFour";

        m_bravoOne.m_name = "bravoOne";
        m_bravoTwo.m_name = "bravoTwo";
        m_bravoThree.m_name = "bravoThree";
        m_bravoFour.m_name = "bravoFour";

        InitPlanning();
    }

    public void ParseWaypoints()
    {
        UI_Waypoints waypointList;
        Vector3 goCommand = new Vector3(-1, -1, -1);

        foreach (Pawn element in m_ListOfFriendlyPawns)
        {
            m_selector = element.m_name;

            waypointList = GetUIWaypointMember();

            waypointList.m_waypoints.RemoveAll(T => T.waypoint == goCommand);

            element.GetComponent<AgentBLUE>().SetupWaypoints(waypointList);

        }
        m_Planning_UI.alpha = 0;
        m_Planning_UI.blocksRaycasts = false;
        m_Planning_UI.interactable = false;

        m_waypointGameObjectHolder.SetActive(false);

        StateManager.StartGame();

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (m_selector != "")
            {
                SetWaypoint(false);
            }

        }
    }
}
