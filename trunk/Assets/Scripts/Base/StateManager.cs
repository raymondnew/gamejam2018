using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    static private StateManager g_Inst;
    public delegate void StateHandler(GameState gameState);
    static public event StateHandler OnNewState;

    static public void InvokeNextState()
    {
        if (g_Inst != null)
            g_Inst.MoveToNextState();
    }

    public enum GameState
    {
        Menu,
        Staging,
        Playing,
        EndGame,
        NoState
    }

    private GameState m_CurrentState;
    static public GameState CurrentState { get { return (g_Inst != null) ? g_Inst.m_CurrentState : GameState.NoState; } }

    private void Awake()
    {
        g_Inst = this;
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<StateManager>().Length > 1)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentState = GameState.Menu;
    }

    void MoveToNextState()
    {
        if (m_CurrentState == GameState.EndGame)
            m_CurrentState = GameState.Menu;
        else
            m_CurrentState++;

        SetState();
    }

    void SetState()
    {
        switch (m_CurrentState)
        {
            case GameState.Menu:
                SetMenu();
                break;
            case GameState.Staging:
                SetStaging();
                break;
            case GameState.Playing:
                SetPlaying();
                break;
            case GameState.EndGame:
                SetEndGame();
                break;
            default:
                break;
        }
    }

    void SetMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        OnNewState?.Invoke(GameState.Menu);
    }

    void SetStaging()
    {
        OnNewState?.Invoke(GameState.Staging);
    }

    void SetPlaying()
    {
        PlanningManager.BeginGame();
        OnNewState?.Invoke(GameState.Playing);
    }

    void SetEndGame()
    {
        OnNewState?.Invoke(GameState.EndGame);
    }
}