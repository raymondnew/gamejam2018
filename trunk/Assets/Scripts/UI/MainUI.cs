using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(CanvasGroup))]
public class MainUI : MonoBehaviour
{
    private CanvasGroup m_CnvGrp;

    [SerializeField]
    Text m_TimeLimitIndicator;

    [SerializeField]
    AgentStatus m_AgentStatus;

    bool m_Begin = false;

    private void Awake()
    {
        m_CnvGrp = GetComponent<CanvasGroup>();
        m_CnvGrp.alpha = 0.0f;
        m_CnvGrp.interactable = false;
        m_CnvGrp.blocksRaycasts = false;
        PlanningManager.OnBegin += StartGame;
    }

    void StartGame()
    {
        m_CnvGrp.alpha = 1.0f;
        m_CnvGrp.interactable = true;
        m_CnvGrp.blocksRaycasts = true;
        m_Begin = true;
        InitializeAgentStatuses();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Begin)
            return;

        UpdateTimeLimitIndicator();
    }

    void InitializeAgentStatuses()
    {
        int count = 0;
        int squadCount = 0;
        List<AgentBLUE> agentList = new List<AgentBLUE>(FindObjectsOfType<AgentBLUE>());
        agentList.Sort(delegate (AgentBLUE x, AgentBLUE y)
        {
            return x.m_Rank.CompareTo(y.m_Rank);
        });

        Dictionary<int, List<AgentBLUE>> squadMap = new Dictionary<int, List<AgentBLUE>>();
        foreach (AgentBLUE agent in agentList)
        {
            if (!squadMap.ContainsKey(agent.m_SquadNumber))
                squadMap.Add(agent.m_SquadNumber, new List<AgentBLUE>());

            squadMap[agent.m_SquadNumber].Add(agent);
        }
        foreach (int squadNum in squadMap.Keys)
        {
            int i = 0;
            squadMap[squadNum].Sort(delegate (AgentBLUE x, AgentBLUE y)
            {
                return x.m_Rank.CompareTo(y.m_Rank);
            });
            foreach (AgentBLUE agent in squadMap[squadNum])
            {
                Color color = ColorSystem.Instance.SquadColors[squadCount].m_Colors[i];
                AgentStatus status = (count == 0) ? m_AgentStatus : Instantiate(m_AgentStatus, m_AgentStatus.transform.parent);
                status.SetAgent(agent, color);
                count++;
                i++;
            }
            squadCount++;
        }
    }

    void UpdateTimeLimitIndicator()
    {
        float timeLimit = GameRules.LvlSettings.m_TimeLimit;
        if (timeLimit == 0.0f)
        {
            m_TimeLimitIndicator.text = "NO TIME LIMIT";
            return;
        }

        float timeElapsed = Time.time - GameRules.StartTime;
        float timeLeft = timeLimit - timeElapsed;
        m_TimeLimitIndicator.text = "TIME LIMIT: " + timeLeft.ToString("F2");
    }
}
