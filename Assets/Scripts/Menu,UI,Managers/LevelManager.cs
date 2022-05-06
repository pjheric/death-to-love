// Contributor(s): Nathan More
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player1;
    [SerializeField] private PlayerController _player2;

    private void Start()
    {
        SetupMultiplayerCharacters();
    }

    public void SetupMultiplayerCharacters()
    {
        _player2.gameObject.SetActive(true);

        if (GameManagerScript.Instance.Player1Character == CharacterChoices.Liz)
        {
            _player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard");
            _player1.GetComponent<PlayerInput>().ActivateInput();
            _player1.CharacterName = "LIZ";
            _player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Controller");
            _player2.GetComponent<PlayerInput>().ActivateInput();
            _player2.CharacterName = "JAY";
        }
        else
        {
            _player2.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard");
            _player2.CharacterName = "LIZ";
            _player1.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Controller");
            _player1.CharacterName = "JAY";
        }
    }
}
