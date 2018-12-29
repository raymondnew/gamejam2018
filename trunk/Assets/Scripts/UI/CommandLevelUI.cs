using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLevelUI : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text m_CommandLevelValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StateManager.CurrentState == StateManager.GameState.Staging || PlanningManager.Instance.CurrentGoCommand < 0)
            m_CommandLevelValue.text = "STAGING";
        else
            m_CommandLevelValue.text = PlanningManager.Instance.CurrentGoCommand.ToString();
    }
}
