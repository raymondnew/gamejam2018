using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    static private StateManager g_Inst;
    public delegate void StateHandler(GameState gameState);
    static public event StateHandler OnNewState;

    LevelSettings m_LevelSettings;

    [SerializeField]
    List<GameProfile> m_GameProfileLibrary = new List<GameProfile>();

    GameProfile m_GameProfile;
    string m_SelectedGameProfile;

    static public void InvokeNextState()
    {
        if (g_Inst != null)
            g_Inst.MoveToNextState();
    }

    static public void StartGame()
    {
        if (g_Inst != null)
        {
            if (CurrentState == GameState.Staging)
                InvokeNextState();
        }
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

    static public LevelSettings GetSelectedLevelSettings { get { return g_Inst.m_LevelSettings; } }
    static public GameProfile []GetGameProfiles { get { return (g_Inst != null) ? g_Inst.m_GameProfileLibrary.ToArray() : null; } }
    static public GameProfile GetSelectedGameProfile
    {
        get
        {
            return g_Inst.GetGameProfileByName(g_Inst.m_SelectedGameProfile);
        }
    }

    static public void SetGameProfile(GameProfile gameProfile)
    {
        if (g_Inst)
        {
            g_Inst.m_SelectedGameProfile = gameProfile.name;
        }
    }

    static public void SetLevelSettings(float timeLimit = 0.0f)
    {
        if (g_Inst)
            g_Inst.m_LevelSettings.SetSettings(timeLimit);
    }

    private GameProfile GetGameProfileByName(string name)
    {
        foreach(GameProfile gameProfile in m_GameProfileLibrary)
        {
            if (name == gameProfile.name)
                return gameProfile;
        }
        return new GameProfile();
    }

    private void Awake()
    {
        if (FindObjectsOfType<StateManager>().Length > 1)
            Destroy(gameObject);
        else
        {
            g_Inst = this;
            DontDestroyOnLoad(gameObject);
        }
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