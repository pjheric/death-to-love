using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterSelectIcon : MonoBehaviour
{
    [SerializeField] private Sprite _keyboardSprite;
    [SerializeField] private Sprite _controllerSprite;

    private PlayerInput _playerInput;
    private SpriteRenderer _spriteRenderer;
    //private List<Transform> _

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //if (_playerInput.currentControlScheme == "Keyboard")
        //{
        //    _spriteRenderer.sprite = _keyboardSprite;
        //}
        //else if (_playerInput.currentControlScheme == "Controller")
        //{
        //    _spriteRenderer.sprite = _controllerSprite;
        //}
    }
}
