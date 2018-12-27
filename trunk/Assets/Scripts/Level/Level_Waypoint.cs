using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Level_Waypoint : MonoBehaviour
{
    public LayerMask m_WalkableLayer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (Application.isPlaying)
            Destroy(gameObject);
    }

    protected virtual void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, m_WalkableLayer) ||
            Physics.Raycast(transform.position, Vector3.up, out hitInfo, Mathf.Infinity, m_WalkableLayer))
        {
            transform.position = hitInfo.point;
        }
    }
}