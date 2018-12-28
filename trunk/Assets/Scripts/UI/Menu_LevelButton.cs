using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(UnityEngine.UI.Button))]
public class Menu_LevelButton : MonoBehaviour
{
    [SerializeField]
    string m_LevelName;

    bool init = false;

    public void SetLevel(string levelName)
    {
        m_LevelName = levelName;
        if (GetComponentInChildren<UnityEngine.UI.Text>() != null)
            GetComponentInChildren<UnityEngine.UI.Text>().text = "Launch " + levelName;
        init = true;
    }

    public void HandleCall()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(m_LevelName);
    }
}