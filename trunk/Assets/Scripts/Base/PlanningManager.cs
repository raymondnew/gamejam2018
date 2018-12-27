using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningManager : MonoBehaviour
{
    static public PlanningManager Instance { get; private set; }
    public int GoCommandsCount { get; private set; } = 0;

    public delegate void GoCommandHandler(int goCommand);
    static public event GoCommandHandler OnGoCommand;

    public delegate void BeginHandler();
    static public event BeginHandler OnBegin;

    public int CurrentGoCommand { get; private set; } = -1;
    public int NextGoCommand { get; private set; } = 0;

    private bool m_Begin = false;

    public int m_TempGoCommandCount = 0;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddNewGoCommand(m_TempGoCommandCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            IssueNextGoCommand();

        if (Input.GetKeyUp(KeyCode.Return))
            Begin();
    }

    public void AddNewGoCommand(int numGoCommands = 1)
    {
        for (int i = 0; i < numGoCommands; i++)
            CreateNewGoCommand();
    }

    void CreateNewGoCommand()
    {
        GoCommandsCount++;
    }

    public void IssueNextGoCommand()
    {
        if (NextGoCommand > GoCommandsCount || !m_Begin)
            return;

        CurrentGoCommand = NextGoCommand;
        NextGoCommand++;

        OnGoCommand?.Invoke(CurrentGoCommand);
    }

    public void Begin()
    {
        if (!m_Begin)
        {
            m_Begin = true;
            OnBegin?.Invoke();
            IssueNextGoCommand();
        }
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 300, 20), "CURRENT GO COMMAND: " + CurrentGoCommand);
    }
}