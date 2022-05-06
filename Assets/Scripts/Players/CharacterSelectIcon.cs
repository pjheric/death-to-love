// Contributor(s): Nathan More
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterSelectIcon : MonoBehaviour
{

    public CharacterChoices _selectedChar;
    public bool IsKeyboard { get; set; }

    [SerializeField] private CharacterSelectManager _characterSelectManager;
    [SerializeField] private Sprite _keyboardSprite;
    [SerializeField] private Sprite _controllerSprite;
    //[SerializeField] private float _characterSelectOffsetX;
    [SerializeField] private Transform[] _positionTransforms;

    private Vector3 _initialPos;
    private Vector3 _leftCharacterPos;
    private Vector3 _rightCharacterPos;
    private bool _lockedInChoice;

    private PlayerInput _playerInput;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //SetSprite();
        _selectedChar = CharacterChoices.None;
        _lockedInChoice = false;
    }

    private void Start()
    {
        //_initialPos = gameObject.transform.position;
        //_leftCharacterPos = _initialPos - new Vector3(_characterSelectOffsetX, 0, 0);
        //_rightCharacterPos = _initialPos + new Vector3(_characterSelectOffsetX, 0, 0);

        if (_positionTransforms.Length >= 3)
        {
            _initialPos = _positionTransforms[0].position;
            _leftCharacterPos = _positionTransforms[1].position;
            _rightCharacterPos = _positionTransforms[2].position;
        }
        else
        {
            Debug.Log("Icon position transforms not set. See CharacterSelectIcon Start() method.");
        }
        
    }

    public void SetSprite()
    {
        if (_playerInput.currentControlScheme == "Keyboard")
        {
            _spriteRenderer.sprite = _keyboardSprite;
            IsKeyboard = true;
        }
        else if (_playerInput.currentControlScheme == "Controller")
        {
            _spriteRenderer.sprite = _controllerSprite;
            IsKeyboard = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_lockedInChoice == false)
            {
                MoveSelector(context.ReadValue<Vector2>().x);
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed) {
            if (_selectedChar != CharacterChoices.None)
            {
                if (_selectedChar == CharacterChoices.Liz)
                {
                    if (_characterSelectManager.LizChosen == false)
                    {
                        _characterSelectManager.ChoseCharacter(CharacterChoices.Liz, IsKeyboard);
                        _lockedInChoice = true;
                    }
                    //if (PlayerManager.Instance.lizChosen == false)
                    //{
                    //    PlayerManager.Instance.ChoseCharacter(CharacterChoices.Liz, IsKeyboard);
                    //    _lockedInChoice = true;
                    //}
                }
                else if (_selectedChar == CharacterChoices.Jay)
                {
                    if (_characterSelectManager.JayChosen == false)
                    {
                        _characterSelectManager.ChoseCharacter(CharacterChoices.Jay, IsKeyboard);
                        _lockedInChoice= true;
                    }

                    //if (PlayerManager.Instance.jayChosen == false)
                    //{
                    //    PlayerManager.Instance.ChoseCharacter(CharacterChoices.Jay, IsKeyboard);
                    //    _lockedInChoice = true;
                    //}
                }

                _characterSelectManager.CheckStartGame();
            }
        }
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_lockedInChoice == true)
            {
                _lockedInChoice = false;
            }
            else
            {
                GameManagerScript.Instance.LoadMainMenu();
            }
        }
    }

    public void MoveSelector(float xDirection)
    {
        if (xDirection > 0.5)
        {
            if (gameObject.transform.position.x == _initialPos.x)
            {
                gameObject.transform.position = _rightCharacterPos;
                _selectedChar = CharacterChoices.Jay;
            }
            else if (gameObject.transform.position.x == _leftCharacterPos.x)
            {
                gameObject.transform.position = _initialPos;
                _selectedChar = CharacterChoices.None;
            }
        }
        else if (xDirection < -0.5)
        {
            if (gameObject.transform.position.x == _initialPos.x)
            {
                gameObject.transform.position = _leftCharacterPos;
                _selectedChar = CharacterChoices.Liz;
            }
            else if (gameObject.transform.position.x == _rightCharacterPos.x)
            {
                gameObject.transform.position = _initialPos;
                _selectedChar = CharacterChoices.None;
            }
        }
    }
}

public enum CharacterChoices { Liz, Jay, None };
