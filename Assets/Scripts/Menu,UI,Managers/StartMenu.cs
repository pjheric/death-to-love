using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuPanel;
    [SerializeField] GameObject _gameModePopup; 
    private LevelLoader levelLoader;
    private void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
    }
    public void OpenMenu(GameObject _panel)
    {
        _mainMenuPanel.SetActive(false);
        _panel.SetActive(true); 
    }
    public void CloseMenu(GameObject _panel)
    {
        _mainMenuPanel.SetActive(true);
        _panel.SetActive(false); 
    }
    public void StartGame()
    {
        _gameModePopup.SetActive(true); 
    }

    public void Singleplayer()
    {
        GameManagerScript.Instance.IsMultiplayer = false;
        if (levelLoader)
        {
            levelLoader.LoadLevel("Level 1");
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    public void Multiplayer()
    {
        GameManagerScript.Instance.IsMultiplayer = true; 
        if (levelLoader)
        {
            levelLoader.LoadLevel("Character Select Screen");
        }
        else
        {
            SceneManager.LoadScene("Character Select Screen");
        }
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

}
