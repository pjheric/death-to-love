using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Compiled_Test_Scene");
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }

}
