using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    Menu_LevelButton m_LevelButton;

    // Start is called before the first frame update
    void Start()
    {
        // GET LIST OF SCENES IN BUILD SETTINGS
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        string[] scenes = new string[sceneCount - 1];
        int currCount = 0;
        for (int i = 0; i < sceneCount; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            if (sceneName != "Menu") // Ignore this scene "Menu"
            {
                scenes[currCount] = sceneName;
                currCount++;
            }
        }
        if (sceneCount < 2)
            m_LevelButton.gameObject.SetActive(false);

        // SET UP MENU BUTTONS
        for (int i = 0; i < sceneCount - 1; i++)
        {
            Menu_LevelButton levelButton = (i > 0) ? Instantiate(m_LevelButton, m_LevelButton.transform.parent) : m_LevelButton;
            levelButton.SetLevel(scenes[i]);
        }
    }
}