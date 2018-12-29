using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    Menu_LevelButton m_LevelButton;

    [SerializeField]
    Toggle m_GameModeToggle;

    [SerializeField]
    InputField m_TimeLimitInput;

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



        // GET LIST OF GAME MODES AND SET UP TOGGLES
        GameProfile[] gameProfiles = StateManager.GetGameProfiles;
        for (int i = 0; i < gameProfiles.Length; i++)
        {
            Toggle gameModeToggle = (i > 0) ? Instantiate(m_GameModeToggle, m_GameModeToggle.transform.parent) : m_GameModeToggle;
            gameModeToggle.GetComponentInChildren<Text>().text = gameProfiles[i].name;

            if (i == 0)
                StateManager.SetGameProfile(gameProfiles[i]);
        }
    }

    public void HandleSetGameProfile(Toggle tog)
    {
        if (tog.isOn)
        {
            GameProfile[] gameProfiles = StateManager.GetGameProfiles;
            foreach (GameProfile profile in gameProfiles)
            {
                if (profile.name == tog.GetComponentInChildren<Text>().text)
                    StateManager.SetGameProfile(profile);
            }
        }
    }

    public void HandleTimeLimitChanged()
    {
        string limitTxt = m_TimeLimitInput.text;
        float.TryParse(limitTxt, out float timeLimit);

        StateManager.SetLevelSettings(timeLimit);
    }

    public void HandleQuit()
    {
        Application.Quit();
    }
}