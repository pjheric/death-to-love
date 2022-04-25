using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public bool lizChosen;
    public bool jayChosen;

    private PlayerInputManager _playerInputManager;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerManager>();
            }

            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        _playerInputManager = GetComponent<PlayerInputManager>();

        lizChosen = false;
        jayChosen = false;
    }

    public void Update()
    {
        CheckStartGame();
        
    }

    public void CheckStartGame()
    {
        if (_playerInputManager.playerCount == 1)
        {
            if (lizChosen || jayChosen)
            {
                GameManagerScript.Instance.NewGame();
            }
        }
        else if (_playerInputManager.playerCount == 2)
        {
            if (lizChosen && jayChosen)
            {
                GameManagerScript.Instance.NewGame();
            }
        }
    }
}
