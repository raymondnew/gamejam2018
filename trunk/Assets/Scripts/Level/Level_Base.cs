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

    public int FloorLevel { get { return m_FloorLevel; } }

    const float floorHeight = 5f;
    static public float FloorHeight { get { return 5f; } }

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.RegisterLevelComponent(this);

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
