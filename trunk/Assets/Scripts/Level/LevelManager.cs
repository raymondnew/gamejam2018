using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager g_Inst;

    [SerializeField]
    LayerMask m_WalkableLayer;

    public static LayerMask WalkableLayer { get { return (g_Inst != null) ? g_Inst.m_WalkableLayer : (LayerMask)0; } }

    void Awake()
    {
        g_Inst = this;
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