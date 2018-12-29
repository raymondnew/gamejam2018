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

    bool m_Begin = false;

    private void Awake()
    {
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
