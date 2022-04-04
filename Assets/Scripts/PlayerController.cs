// Contributor(s): Nathan More
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] // Ensures object will have a CharacterController
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 3.0f;

    private CharacterController _controller;

    private Vector2 _movementInput = Vector2.zero;
    private bool _lightAttack = false;
    private bool _heavyAttack = false;

    private void Start()
    {
        // Sets Character Controller component
        _controller = gameObject.GetComponent<CharacterController>();
    }

    // Gets direction from player input
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    // Called when player presses light attack button
    public void OnLightAttack(InputAction.CallbackContext context)
    {
        _lightAttack = context.action.triggered;
    }

    // Called when player presses heavy attack button
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        _heavyAttack = context.action.triggered;
    }

    void Update()
    {
        PlayerMovement(); // Calls method to handle movement

        if (_lightAttack) // If Light Attack is pressed
        {
            LightAttack();
        }
        else if (_heavyAttack) // If Heavy Attack is pressed
        {
            HeavyAttack();
        }
    }

    // Handles player movement and animations
    public void PlayerMovement()
    {
        // Adjusts vertical movement speed (halves it)
        Vector2 movement = new Vector2(_movementInput.x, _movementInput.y / 2);

        // Uses character controller component to move character
        _controller.Move(movement * Time.deltaTime * _playerSpeed);
    }

    public void LightAttack()
    {
        Debug.Log("Light Attack!");
        _lightAttack = false;
    }

    public void HeavyAttack()
    {
        Debug.Log("Heavy Attack!");
        _heavyAttack = false;
    }
}
