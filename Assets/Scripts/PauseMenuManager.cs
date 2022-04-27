using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    private LevelLoader levelLoader;
    private PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        if(levelLoader)
        {
            levelLoader.LoadLevel("Start Menu");
        }
        else
        {
            Debug.LogError("Level Loader not found in Pause Menu Script");
        }
    }

    public void quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        PC.Resume();
    }

}
