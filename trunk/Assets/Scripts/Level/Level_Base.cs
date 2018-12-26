using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Level_Base : MonoBehaviour
{
    [SerializeField]
    private int m_FloorLevel = 0;

    [SerializeField]
    private bool m_SnapToGrid = true;

    const float floorHeight = 5f;

    // Start is called before the first frame update
    void Start()
    {
        SnapToGrid();

        if (Application.isPlaying)
            Destroy(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_SnapToGrid)
            SnapToGrid();
    }

    void SnapToGrid()
    {
        float xPos = Mathf.Round(transform.position.x);
        float zPos = Mathf.Round(transform.position.z);

        float yPos = m_FloorLevel * floorHeight;

        transform.position = new Vector3(xPos, yPos, zPos);
    }
}
