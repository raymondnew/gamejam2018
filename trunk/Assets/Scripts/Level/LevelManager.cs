using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager g_Inst;

    [SerializeField]
    LayerMask m_WalkableLayer;
    public static LayerMask WalkableLayer { get { return (g_Inst != null) ? g_Inst.m_WalkableLayer : (LayerMask)0; } }

    [SerializeField]
    LayerMask m_ObstacleLayer;
    public static LayerMask ObstacleLayer { get { return (g_Inst != null) ? g_Inst.m_ObstacleLayer : (LayerMask)0; } }

    public static LayerMask LOS_Layer { get { return (g_Inst != null) ? (LayerMask)(WalkableLayer | ObstacleLayer) : (LayerMask)0; } }

    private int m_FloorNum = 0;

    private Dictionary<int, List<Pawn>> m_PawnFloorMap = new Dictionary<int, List<Pawn>>();

    public static void RegisterLevelComponent(Level_Base component)
    {
        if (g_Inst != null)
            g_Inst.m_FloorNum = Mathf.Max(g_Inst.m_FloorNum, 1 + component.FloorLevel);
    }

    public static bool RegisterPawn(Pawn pawn)
    {
        if (g_Inst == null)
            return false;

        if (!g_Inst.m_PawnFloorMap.ContainsKey(pawn.Floor))
            g_Inst.m_PawnFloorMap.Add(pawn.Floor, new List<Pawn>());

        g_Inst.m_PawnFloorMap[pawn.Floor].Add(pawn);

        return true;
    }

    public static bool UpdatePawn(Pawn pawn, int newLevel)
    {
        if (g_Inst == null)
            return false;

        if (!g_Inst.m_PawnFloorMap[pawn.Floor].Remove(pawn))
            Debug.LogError("Missing pawn in floor map");

        if (!g_Inst.m_PawnFloorMap.ContainsKey(newLevel))
            Debug.LogError("Missign floor in floor map");

        g_Inst.m_PawnFloorMap[pawn.Floor].Add(pawn);

        return true;
    }

    public static List<Pawn> GetPawnsByFloor(int floor)
    {
        if (g_Inst == null || !g_Inst.m_PawnFloorMap.ContainsKey(floor))
            return null;

        return g_Inst.m_PawnFloorMap[floor];
    }

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