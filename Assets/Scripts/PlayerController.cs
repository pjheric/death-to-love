// Contributor(s): Nathan More
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;

    private CharacterController controller;
    private Vector3 playerVelocity;

    private Vector2 movementInput = Vector2.zero;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        Vector2 move = new Vector2(movementInput.x, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);
    }
}
