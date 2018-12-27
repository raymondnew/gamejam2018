using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UI_Waypoints
{
    public string m_name;
    public List<Waypoint> m_waypoints = new List<Waypoint>();
    

    public struct Waypoint
    {
        public Vector3 waypoint;
        public Transform m_prefab;
        public int m_goCommand;
    }




    void Awake()
    {
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
