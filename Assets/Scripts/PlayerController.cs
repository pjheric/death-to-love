// Contributor(s): Nathan More
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] // Ensures object will have a CharacterController
[RequireComponent(typeof(Animator))] // Ensures object will have an Animator
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 3.0f;
    [SerializeField]
    private Transform attackPos;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private int lightDamage;
    [SerializeField]
    private int heavyDamage;

    private CharacterController _controller;
    private Animator playerAnim;
    private Vector2 _movementInput = Vector2.zero;

    private void Start()
    {
        // Sets Character Controller component
        _controller = gameObject.GetComponent<CharacterController>();

        // Sets player animator
        playerAnim = gameObject.GetComponent<Animator>();
    }

    // Gets direction from player input
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    // Called when player presses light attack button
    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // Ensures functions only performed once on button press
        {
            Debug.Log("Light Attack!");
            // TODO: Add light attack animation
            AttackEnemies(lightDamage);
        }
    }

    // Called when player presses heavy attack button
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // Ensures functions only performed once on button press
        {
            Debug.Log("Heavy Attack!");
            // TODO: Add heavy attack animation
            AttackEnemies(heavyDamage);
        }
    }

    void Update()
    {
        PlayerMovement(); // Calls method to handle movement
    }

    // Handles player movement and animations
    public void PlayerMovement()
    {
        // Adjusts vertical movement speed (halves it)
        Vector2 movement = new Vector2(_movementInput.x, _movementInput.y / 2);

        // Uses character controller component to move character
        _controller.Move(movement * Time.deltaTime * _playerSpeed);
    }

    // Finds enemies in range and calls their TakeDamage() method.
    public void AttackEnemies(int damage)
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayer);
        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            enemiesToHit[i].GetComponent<EnemyAgent>().TakeDamage(damage);
        }
    }

    // Creates a gizmo for attack area in editor
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
