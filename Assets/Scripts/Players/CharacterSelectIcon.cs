// Contributor(s): Nathan More
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterSelectIcon : MonoBehaviour
{

    public enum CharacterChoices { Liz, Jay, None};
    public CharacterChoices _selectedChar;

    [SerializeField] private Sprite _keyboardSprite;
    [SerializeField] private Sprite _controllerSprite;
    [SerializeField] private float _characterSelectOffsetX;

    private Vector3 _initialPos;
    private Vector3 _leftCharacterPos;
    private Vector3 _rightCharacterPos;

    private PlayerInput _playerInput;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        SetSprite();
        _selectedChar = CharacterChoices.None;
    }

    private void Start()
    {
        _initialPos = gameObject.transform.position;
        _leftCharacterPos = _initialPos - new Vector3(_characterSelectOffsetX, 0, 0);
        _rightCharacterPos = _initialPos + new Vector3(_characterSelectOffsetX, 0, 0);
    }

    public void SetSprite()
    {
        if (_playerInput.currentControlScheme == "Keyboard")
        {
            _spriteRenderer.sprite = _keyboardSprite;
        }
        else if (_playerInput.currentControlScheme == "Controller")
        {
            _spriteRenderer.sprite = _controllerSprite;
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //SetSprite();

            MoveSelector(context.ReadValue<Vector2>().x);
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
