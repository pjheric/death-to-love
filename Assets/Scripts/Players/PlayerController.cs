// Contributor(s): Nathan More
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro; 

public enum ComboState {
    NONE,
    LP1,
    LP2,
    LP3
}

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
    private float _lightHitstun = 1f;
    [SerializeField]
    private int _heavyDamage = 4;
    [SerializeField]
    private float _heavyHitstun = 1f;
    [SerializeField]
    private FloatAsset _health;
    [SerializeField]
    private float _lightAtkCooldown = 0.5f;
    [SerializeField]
    private float _heavyAtkCooldown = 1.5f;

    //UI Elements
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _heatPanel;
    [SerializeField]
    private TextMeshProUGUI _heatNumber; 

    private Rigidbody2D _rigidBody;
    private Animator _playerAnim;
    private Vector2 _movementInput = Vector2.zero;
    private bool _facingRight = true;
    private bool _sliding = false;
    private bool _canSlide = true;
    private bool _canAttack = true;
    private bool _isPaused = false; 
    private PlayerInput _input;

    // light attack combo fields
    private bool _comboCheck;
    private float _defaultComboTimer = 0.4f;
    private float _currentComboTimer;
    private int _lightComboDamageIncrease = 1;
    private ComboState _currentComboState;


    //Heat System Fields
    private bool _heatCheck = true;
    private float _currentHeatNum = 0;
    private float _heatFalloffTime; 

    private void Start()
    {
        // Sets Character Controller component
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();

        // Sets player animator
        _playerAnim = gameObject.GetComponent<Animator>();

        _input = gameObject.GetComponent<PlayerInput>();
        Debug.Log(_input.currentControlScheme);

        _currentComboTimer = _defaultComboTimer;
        _currentComboState = ComboState.NONE;
    }

    // Gets direction from player input
    public void OnMove(InputAction.CallbackContext context)
    {
        // not 100% sure why, but if you hold another direction while sliding,
        // the player will keep moving in the same direction
        // will reset when you let go of a direction/input a different direction
        //if (!_sliding && _canSlide) {
        if (!_sliding) {
            _movementInput = context.ReadValue<Vector2>();
        }
    }

    // Called when player presses light attack button
    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // Ensures functions only performed once on button press
        {
            //Debug.Log("light attack input");
            if (_canAttack)
            {
                //Debug.Log("Light Attack!");
                if (_currentComboState == ComboState.LP3)
                    return;


                // starts on ComboState.NONE by the Start()
                _currentComboState++;
                _comboCheck = true;
                _currentComboTimer = _defaultComboTimer;
                int _currentLightDamage = _lightDamage;
                // need to add code to change the actual animations to match
                if(_currentComboState == ComboState.LP1) {

                    Debug.Log("light attack1 performed");
                }

                if (_currentComboState == ComboState.LP2) {
                    _currentLightDamage += _lightComboDamageIncrease;
                    Debug.Log("light attack2 performed");
                }

                if (_currentComboState == ComboState.LP3) {
                    _currentLightDamage += 2 * _lightComboDamageIncrease;
                    Debug.Log("light attack3 performed");
                }

                _playerAnim.SetTrigger("Light Attack");
                AttackEnemies(_currentLightDamage, _lightHitstun);
                //StartCoroutine(AttackCooldown(_lightAtkCooldown));
            }
        }
    }

    private void UpdateHeat()
    {
        //Step 1: determine heat falloff
        //Heat falls off either if no one attacked within the falloff time OR if the damaged received (total combined) exceeds 5
        if (_heatCheck)
        {
            _heatPanel.SetActive(true);
            _currentHeatNum += 1;
            _heatNumber.SetText(_currentHeatNum.ToString()); 
            if(_currentHeatNum >= 10 && _currentHeatNum < 20)
            {
                _heatNumber.color = Color.cyan; 
            }
            else if(_currentHeatNum >= 20)
            {
                _heatNumber.color = Color.magenta; 
            }
        }
        else
        {
            _heatPanel.SetActive(false);
            _currentHeatNum = 0; 
        }

    }


    void ResetComboState() {
        // if the player has initiated a light attack
        if (_comboCheck) {
            _currentComboTimer -= Time.deltaTime;

            if(_currentComboTimer <= 0f) {
                // resets the fields to the default state to check for combos again
                _currentComboState = ComboState.NONE;
                _comboCheck = false;
                _currentComboTimer = _defaultComboTimer;
                // used to enforce a cooldown, but it is after the combo window has expired
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
                //Debug.Log("Heavy Attack!");
                _playerAnim.SetTrigger("Heavy Attack");
                AttackEnemies(_heavyDamage, _heavyHitstun);
                StartCoroutine(AttackCooldown(_heavyAtkCooldown));
            }
        }
    }

    public void OnSlide(InputAction.CallbackContext context) {
        if (context.performed && _canSlide) // Ensures functions only performed once on button press
        {
            //Debug.Log("Slide");
            _playerAnim.SetTrigger("Slide");
            _sliding = true;
            _canSlide = false;
            _playerSpeed *= 1.5f;
            float duration = _playerAnim.GetFloat("Slide Duration");
            StartCoroutine(SlideSpeedReset(duration));
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isPaused)
            {
                pause(); 
            }
            else
            {
                Resume(); //I put these into their own functions because we need the continue button on the pause menu to Resume and set _isPaused to false
            }
        }
    }

   

    private IEnumerator SlideSpeedReset(float duration) {
        yield return new WaitForSeconds(duration);
        _playerAnim.SetTrigger("Slide");
        _playerSpeed /= 1.5f;
        _sliding = false;
        // temporary bandage for game not taking input during a slide
        _movementInput = Vector2.zero;
        // extra delay so a player can't spam sliding?
        yield return new WaitForSeconds(0.5f);
        _canSlide = true;
    }

    void Update()
    {
        PlayerMovement(); // Calls method to handle movement
        ResetComboState();
    }

    // Handles player movement and animations
    public void PlayerMovement()
    {
        // Adjusts vertical movement speed (halves it) and multiplies vector by time and speed
        Vector2 velocity = (new Vector2(_movementInput.x, _movementInput.y / 2)) * Time.deltaTime * _playerSpeed;

        if (velocity != Vector2.zero && _sliding == false) // Controls walking animation
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
    public void AttackEnemies(int damage, float Hitstun)
    {
        Debug.Log("damage: " + damage);
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(_attackPos.position, _attackRange, _enemyLayer);
        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            enemiesToHit[i].GetComponent<EnemyAgent>().TakeDamage(damage, Hitstun);
        }
        UpdateHeat(); 
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
                //Debug.Log("Player Dead");
            }
        }
    }

    public IEnumerator AttackCooldown(float cooldown)
    {
        _canAttack = false;
        yield return new WaitForSeconds(cooldown);
        _canAttack = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
        _isPaused = false;
    }

    public void pause()
    {
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
        _isPaused = true;
    }
}
