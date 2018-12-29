using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
[RequireComponent (typeof(LineRenderer))]
public class Level_Waypoint_RED : Level_Waypoint
{
    public float waitDuration;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (transform.parent != null && transform.parent.childCount > 1)
        {
            // Find next WP
            List<Transform> siblings = new List<Transform>();
            foreach (Transform sibling in transform.parent)
                siblings.Add(sibling);

            for (int i = 0; i < siblings.Count; i++)
            {
                if (transform == siblings[i])
                {
                    if (i < siblings.Count - 1)
                        DrawPathToSibling(siblings[i + 1]);
                    else
                        DrawPathToSibling(siblings[0]);
                    break;
                }
            }
        }
    }

    void DrawPathToSibling(Transform sibling)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, sibling.position, NavMesh.AllAreas, path);
        if (path.corners != null && path.corners.Length > 1)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
                lineRenderer.SetPosition(i, path.corners[i]);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }
    }
}