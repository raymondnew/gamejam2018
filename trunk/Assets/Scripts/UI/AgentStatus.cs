using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class AgentStatus : MonoBehaviour
{
    Agent m_Agent = null;

    [SerializeField]
    Text m_UnitName;

    [SerializeField]
    Text m_Status;

    public void SetAgent(Agent agent)
    {
        SetAgent(agent, Color.white);
    }

    public void SetAgent(Agent agent, Color color)
    {
        m_Agent = agent;
        m_UnitName.text = agent.name + " Status:";
        GetComponent<Image>().color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Agent != null)
        {
            if (m_Agent.IsDead)
                m_Status.text = "<color=red>DEAD</color>";
            else
            {
                m_Status.text = m_Agent.CurrentState.ToString();
            }
        }
    }
}
