using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void RestartLevel() {
        Debug.Log("Restart Level");
            Time.timeScale = 1;
        if (levelLoader) {
            levelLoader.LoadLevel(SceneManager.GetActiveScene().name);
        } else {
            Debug.LogError("Level Loader not found in Pause Menu Script");
        }
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Main Menu");
        if(levelLoader) {
            Time.timeScale = 1;
            levelLoader.LoadLevel("Start Menu");
        }
        else
        {
            Debug.LogError("Level Loader not found in Pause Menu Script");
        }
    }

    public void quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Resume()
    {
        Debug.Log("Resume");
        PC.Resume();
    }

}
