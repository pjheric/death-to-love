using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class StartMenu : MonoBehaviour
{
    private LevelLoader levelLoader;
    private void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
    }
    public void StartGame()
    {
        if(levelLoader)
        {
            levelLoader.LoadLevel("Character Select");
        }
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

}
