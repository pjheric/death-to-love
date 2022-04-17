// Contributor(s): Nathan More
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    private int _lightDamage = 1;
    [SerializeField]
    private int _heavyDamage = 4;
    [SerializeField]
    private FloatAsset _health;
    [SerializeField]
    private float _lightAtkCooldown = 0.5f;
    [SerializeField]
    private float _heavyAtkCooldown = 1.5f;

    private Rigidbody2D _rigidBody;
    private Animator _playerAnim;
    private Vector2 _movementInput = Vector2.zero;
    private bool _facingRight = true;
    private bool _sliding = false;
    private bool _canAttack = true;

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
            if (_canAttack)
            {
                Debug.Log("Light Attack!");
                _playerAnim.SetTrigger("Light Attack");
                AttackEnemies(_lightDamage);
                StartCoroutine(AttackCooldown(_lightAtkCooldown));
            }
        }
    }

    // Called when player presses heavy attack button
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // Ensures functions only performed once on button press
        {
            if (_canAttack)
            {
                Debug.Log("Heavy Attack!");
                _playerAnim.SetTrigger("Heavy Attack");
                AttackEnemies(_heavyDamage);
                StartCoroutine(AttackCooldown(_heavyAtkCooldown));
            }
        }
    }

    public void OnSlide(InputAction.CallbackContext context) {
        if (context.performed) // Ensures functions only performed once on button press
        {
            Debug.Log("Slide");
            _playerAnim.SetTrigger("Slide");
            _sliding = true;
            _playerSpeed *= 1.5f;
            float duration = _playerAnim.GetFloat("Slide Duration");
            StartCoroutine(SlideSpeedReset(duration));
        }
    }

    private IEnumerator SlideSpeedReset(float duration) {
        yield return new WaitForSeconds(duration);
        _playerAnim.SetTrigger("Slide");
        _playerSpeed /= 1.5f;
        _sliding = false;

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

        // Moves character
        transform.position += new Vector3(velocity.x, velocity.y, 0);
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

    public void TakeDamage(int damage)
    {
        if (!_sliding) {
            _health.Value -= damage;
            if (_health.Value <= 0) {
                Debug.Log("Player Dead");
            }
        }
    }

    public IEnumerator AttackCooldown(float cooldown)
    {
        _canAttack = false;
        yield return new WaitForSeconds(cooldown);
        _canAttack = true;
    }
}
