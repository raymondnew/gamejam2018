using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGameRule
{
    void Init();
    bool HasEndConditionMet();
}

[System.Serializable]
public struct GameProfile
{
    public string name;
    public List<string> m_WinConditions;
    public List<string> m_LossConditions;
}

public struct LevelSettings
{
    public float m_TimeLimit;

    public void SetSettings(float timeLimit = 0.0f)
    {
        m_TimeLimit = timeLimit;
    }

    public GameRules.EndCondition CheckSettings(float timeElapsed)
    {
        if (timeElapsed > m_TimeLimit && m_TimeLimit > 0.0f)
            return GameRules.EndCondition.Loss;

        return GameRules.EndCondition.NoEnd;
    }
}

public class GameRules : MonoBehaviour
{
    static private GameRules g_Inst;
    public delegate void EndGame(EndCondition endCondition);
    static public event EndGame OnEndGame;

    public enum EndCondition
    {
        Win,
        Loss,
        NoEnd
    }

    List<IGameRule> m_WinConditions = new List<IGameRule>();
    List<IGameRule> m_LossConditions = new List<IGameRule>();

    LevelSettings m_LevelSettings;
    static public LevelSettings LvlSettings { get { return g_Inst.m_LevelSettings; } }
    static public float StartTime { get { return g_Inst.m_StartTime; } }

    bool m_Loaded = false;
    EndCondition m_CurrentCondition = EndCondition.NoEnd;

    float m_StartTime = 0.0f;

    bool m_Begin = false;

    private void Awake()
    {
        g_Inst = this;
        PlanningManager.OnBegin += BeginGame;
        OnEndGame += GameEnded;
    }

    void BeginGame()
    {
        m_StartTime = Time.time;
        m_Begin = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Loaded)
        {
            m_LevelSettings = StateManager.GetSelectedLevelSettings;
            ProcessGameProfile(StateManager.GetSelectedGameProfile);

            foreach (IGameRule gameRule in m_WinConditions)
                gameRule.Init();
            foreach (IGameRule gameRule in m_LossConditions)
                gameRule.Init();

            m_Loaded = true;
        }

        if (!m_Begin)
            return;

        if (m_CurrentCondition == EndCondition.NoEnd)
        {
            bool winCondition = true;
            foreach (IGameRule gameRule in m_WinConditions)
            {
                if (!gameRule.HasEndConditionMet())
                {
                    winCondition = false;
                    break;
                }
            }
            if (winCondition)
            {
                m_CurrentCondition = EndCondition.Win;
                OnEndGame?.Invoke(EndCondition.Win);
                return;
            }

            bool lossCondition = true;
            foreach (IGameRule gameRule in m_LossConditions)
            {
                if (!gameRule.HasEndConditionMet())
                {
                    lossCondition = false;
                    break;
                }
            }
            if (lossCondition)
            {
                m_CurrentCondition = EndCondition.Loss;
                OnEndGame?.Invoke(EndCondition.Loss);
                return;
            }

            m_CurrentCondition = m_LevelSettings.CheckSettings(Time.time - m_StartTime);
            if (m_CurrentCondition != EndCondition.NoEnd)
                OnEndGame?.Invoke(m_CurrentCondition);
        }
    }

    void GameEnded(EndCondition condition)
    {
        Debug.Log("END OF GAME: " + condition);
    }

    void ProcessGameProfile(GameProfile gameProfile)
    {
        Debug.Log("Processing Game Profile: " + gameProfile.name);
        foreach (string gameRuleName in gameProfile.m_WinConditions)
        {
            IGameRule gameRule = GenerateGameProfile(gameRuleName);
            if (gameRule != null)
                m_WinConditions.Add(gameRule);
        }

        foreach (string gameRuleName in gameProfile.m_LossConditions)
        {
            IGameRule gameRule = GenerateGameProfile(gameRuleName);
            if (gameRule != null)
                m_LossConditions.Add(gameRule);
        }
    }

    IGameRule GenerateGameProfile(string profileType)
    {
        System.Type classType = System.Type.GetType(profileType);
        IGameRule newObj = (IGameRule)System.Activator.CreateInstance(classType);

        return newObj;
    }
}

public class NoREDLeftRule : IGameRule
{
    List<AgentRED> m_REDList = new List<AgentRED>();

    void IGameRule.Init()
    {
        foreach (Pawn pawn in LevelManager.GetREDPawns())
        {
            AgentRED agent = pawn.GetComponent<AgentRED>();
            if (agent != null)
                m_REDList.Add(agent);
        }
    }

    bool IGameRule.HasEndConditionMet()
    {
        bool conditionMet = true;
        foreach (AgentRED agent in m_REDList)
        {
            if(!agent.IsDead)
            {
                conditionMet = false;
                break;
            }
        }
        return conditionMet;
    }
}

public class NoBLUELeftRule : IGameRule
{
    List<AgentBLUE> m_BLUE_List = new List<AgentBLUE>();

    void IGameRule.Init()
    {
        foreach (Pawn pawn in LevelManager.GetBLUEPawns())
        {
            AgentBLUE agent = pawn.GetComponent<AgentBLUE>();
            if (agent != null)
                m_BLUE_List.Add(agent);
        }
    }

    bool IGameRule.HasEndConditionMet()
    {
        bool conditionMet = true;
        foreach (AgentBLUE agent in m_BLUE_List)
        {
            if (!agent.IsDead)
            {
                conditionMet = false;
                break;
            }
        }
        return conditionMet;
    }
}