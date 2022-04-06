// Contributor(s): Nathan More
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))] // Ensures object will have a Rigidbody2D
[RequireComponent(typeof(Animator))] // Ensures object will have an Animator
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed = 5.0f;
    [SerializeField]
    private Transform _attackPos;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private int _lightDamage;
    [SerializeField]
    private int _heavyDamage;
    [SerializeField]
    private FloatAsset _health;

    private Rigidbody2D _rigidBody;
    private Animator _playerAnim;
    private Vector2 _movementInput = Vector2.zero;
    private bool _facingRight = true;

    private void Start()
    {
        // Sets Character Controller component
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();

        // Sets player animator
        _playerAnim = gameObject.GetComponent<Animator>();
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
            AttackEnemies(_lightDamage);
        }
    }

    // Called when player presses heavy attack button
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // Ensures functions only performed once on button press
        {
            Debug.Log("Heavy Attack!");
            // TODO: Add heavy attack animation
            AttackEnemies(_heavyDamage);
        }
    }

    void Update()
    {
        PlayerMovement(); // Calls method to handle movement
    }

    // Handles player movement and animations
    public void PlayerMovement()
    {
        // Adjusts vertical movement speed (halves it) and multiplies vector by time and speed
        Vector2 velocity = (new Vector2(_movementInput.x, _movementInput.y / 2)) * Time.deltaTime * _playerSpeed;

        if (velocity != Vector2.zero) // Controls walking animation
        {
            _playerAnim.SetBool("Walking", true);
        }
        else
        {
            _playerAnim.SetBool("Walking", false);
        }
        if (velocity.x < 0 && _facingRight) // Controls direction player is facing
        {
            Flip();
        }
        else if (velocity.x > 0 && !_facingRight)
        {
            Flip();
        }

        // Moves character using rigidbody
        _rigidBody.MovePosition(_rigidBody.position + velocity);
    }

    // Flips character horizontally
    public void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    // Finds enemies in range and calls their TakeDamage() method.
    public void AttackEnemies(int damage)
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(_attackPos.position, _attackRange, _enemyLayer);
        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            enemiesToHit[i].GetComponent<EnemyAgent>().TakeDamage(damage);
        }
    }

    // Creates a gizmo for attack area in editor
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPos.position, _attackRange);
    }
}
