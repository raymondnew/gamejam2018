﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string m_selector;
    public UI_Waypoints m_uiwaypoint;
    public GameObject m_waypointPrefab;


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

    public void SetWaypoint()
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

        

        waypointName = waypointName + " Waypoint " + (selectedWaypoint.m_waypoints.Count + 1).ToString();
        Vector3 waypoint = GetWaypoint();

        if (waypoint == Vector3.left)
            return;

        GameObject prefab = Instantiate(m_waypointPrefab, waypoint, Quaternion.identity);
        prefab.transform.Find("Name").GetComponent<TextMesh>().text = waypointName;

        UI_Waypoints.Waypoint waypointStruct = new UI_Waypoints.Waypoint();
        waypointStruct.waypoint = waypoint;
        waypointStruct.m_prefab = prefab.transform;

        selectedWaypoint.m_waypoints.Add(waypointStruct);

    }

    public void DeleteLast()
    {
        if (m_selector == "")
            return;
        UI_Waypoints deleteWaypoint = GetUIWaypointMember();

        if (deleteWaypoint.m_waypoints.Count == 0)
            return;

        UI_Waypoints.Waypoint deleter = deleteWaypoint.m_waypoints[deleteWaypoint.m_waypoints.Count - 1];
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (m_selector != "")
            {
                SetWaypoint();
            }

        }
    }
}
