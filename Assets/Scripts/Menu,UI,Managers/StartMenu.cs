using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 
public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuPanel;
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

    public void SelectButton(GameObject _selectedObject)
    {
        EventSystem.current.SetSelectedGameObject(_selectedObject); 
    }
    

    public void Singleplayer()
    {
        AkSoundEngine.PostEvent("Game_Start_Solo", gameObject);
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
        AkSoundEngine.PostEvent("Game_Start_Multi", gameObject);
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
        AkSoundEngine.PostEvent("Game_Quit", gameObject);
        Application.Quit(); 
    }

    public void ButtonClickSound()
    {
        Debug.Log("Click");
        AkSoundEngine.PostEvent("UI_Button_Press", gameObject);
    }

}
