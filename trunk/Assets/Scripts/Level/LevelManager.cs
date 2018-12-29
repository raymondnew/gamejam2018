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

    [SerializeField]
    LayerMask m_CharacterLayer;
    public static LayerMask CharacterLayer { get { return (g_Inst != null) ? g_Inst.m_CharacterLayer : (LayerMask)0; } }

    public static LayerMask LOS_Layer { get { return (g_Inst != null) ? (LayerMask)(WalkableLayer | ObstacleLayer | CharacterLayer) : (LayerMask)0; } }

    private int m_FloorNum = 0;
    public static int FloorNum { get { return (g_Inst != null) ? g_Inst.m_FloorNum : 0; } }

    private List<Pawn> m_RED_List = new List<Pawn>();
    private Dictionary<int, List<Pawn>> m_RED_FloorMap = new Dictionary<int, List<Pawn>>();

    private List<Pawn> m_BLUE_List = new List<Pawn>();
    private Dictionary<int, List<Pawn>> m_BLUE_FloorMap = new Dictionary<int, List<Pawn>>();

    public static void RegisterLevelComponent(Level_Base component)
    {
        if (g_Inst != null)
            g_Inst.m_FloorNum = Mathf.Max(g_Inst.m_FloorNum, 1 + component.FloorLevel);
    }

    public static bool RegisterRED(Pawn pawn)
    {
        if (g_Inst == null)
            return false;

        if (!g_Inst.m_RED_FloorMap.ContainsKey(pawn.Floor))
            g_Inst.m_RED_FloorMap.Add(pawn.Floor, new List<Pawn>());

        g_Inst.m_RED_FloorMap[pawn.Floor].Add(pawn);
        g_Inst.m_RED_List.Add(pawn);

        return true;
    }

    public static bool RegisterBLUE(Pawn pawn)
    {
        if (g_Inst == null)
            return false;

        if (!g_Inst.m_BLUE_FloorMap.ContainsKey(pawn.Floor))
            g_Inst.m_BLUE_FloorMap.Add(pawn.Floor, new List<Pawn>());

        g_Inst.m_BLUE_FloorMap[pawn.Floor].Add(pawn);
        g_Inst.m_BLUE_List.Add(pawn);

        return true;
    }

    public static bool RemoveRED(Pawn pawn)
    {
        if (g_Inst == null)
            return false;

        g_Inst.m_RED_FloorMap[pawn.Floor].Remove(pawn);

        return true;
    }

    public static bool RemoveBLUE(Pawn pawn)
    {
        if (g_Inst == null)
            return false;

        g_Inst.m_BLUE_FloorMap[pawn.Floor].Remove(pawn);

        return true;
    }

    public static bool UpdateREDPawn(Pawn pawn, int newLevel)
    {
        if (g_Inst == null)
            return false;

        if (!g_Inst.m_RED_FloorMap[pawn.Floor].Remove(pawn))
            Debug.LogError("Missing pawn in RED floor map");

        if (!g_Inst.m_RED_FloorMap.ContainsKey(newLevel))
            g_Inst.m_RED_FloorMap.Add(newLevel, new List<Pawn>());

        g_Inst.m_RED_FloorMap[newLevel].Add(pawn);

        return true;
    }

    public static bool UpdateBLUEPawn(Pawn pawn, int newLevel)
    {
        if (g_Inst == null)
            return false;

        if (!g_Inst.m_BLUE_FloorMap[pawn.Floor].Remove(pawn))
            Debug.LogError("Missing pawn in BLUE floor map");

        if (!g_Inst.m_BLUE_FloorMap.ContainsKey(newLevel))
            g_Inst.m_BLUE_FloorMap.Add(newLevel, new List<Pawn>());

        g_Inst.m_BLUE_FloorMap[newLevel].Add(pawn);

        return true;
    }

    public static List<Pawn> GetREDPawns()
    {
        if (g_Inst != null)
            return g_Inst.m_RED_List;

        return null;
    }

    public static List<Pawn> GetBLUEPawns()
    {
        if (g_Inst != null)
            return g_Inst.m_BLUE_List;

        return null;
    }

    public static List<Pawn> GetREDPawnsByFloor(int floor)
    {
        if (g_Inst == null || !g_Inst.m_RED_FloorMap.ContainsKey(floor))
            return null;

        return g_Inst.m_RED_FloorMap[floor];
    }

    public static List<Pawn> GetBLUEPawnsByFloor(int floor)
    {
        if (g_Inst == null || !g_Inst.m_BLUE_FloorMap.ContainsKey(floor))
            return null;

        return g_Inst.m_BLUE_FloorMap[floor];
    }

    void Awake()
    {
        g_Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StateManager.InvokeNextState();
    }
}