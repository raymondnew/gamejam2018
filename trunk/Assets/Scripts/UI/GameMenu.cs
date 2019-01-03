using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CanvasGroup))]
public class GameMenu : MonoBehaviour
{
    CanvasGroup cnvGrp;
    bool menuOn = false;

    private static GameMenu g_Inst;

    public static bool IsOn { get { return (g_Inst != null) ? g_Inst.menuOn : false; } }

    private void Awake()
    {
        g_Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cnvGrp = GetComponent<CanvasGroup>();
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            ToggleMenu();
    }

    void ToggleMenu()
    {
        if (menuOn)
            Close();
        else
            Open();
    }

    void Open()
    {
        TimeManager.Pause();
        cnvGrp.interactable = true;
        cnvGrp.blocksRaycasts = true;
        cnvGrp.alpha = 1.0f;
        menuOn = true;
    }

    void Close()
    {
        TimeManager.Play();
        cnvGrp.interactable = false;
        cnvGrp.blocksRaycasts = false;
        cnvGrp.alpha = 0.0f;
        menuOn = false;
    }

    public void HandleReturnToGame()
    {
        Close();
    }

    public void HandleMainMenu()
    {
        if(StateManager.CurrentState == StateManager.GameState.Staging)
            StateManager.InvokeNextState();

        if (StateManager.CurrentState == StateManager.GameState.Playing)
            StateManager.InvokeNextState();

        if (StateManager.CurrentState == StateManager.GameState.EndGame)
            StateManager.InvokeNextState();
    }

    public void HandleExitGame()
    {
        Application.Quit();
    }
}