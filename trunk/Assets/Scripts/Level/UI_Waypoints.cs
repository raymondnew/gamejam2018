using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UI_WaypointList
{
    public string m_name;
    public List<Waypoint> m_waypoints = new List<Waypoint>();
    

    public struct Waypoint
    {
        public Vector3 position;
        public Transform m_prefab;
        public int m_goCommand;
    }
}