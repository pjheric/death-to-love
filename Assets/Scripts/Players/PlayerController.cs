// Contributor(s): Nathan More
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))] // Ensures object will have a Rigidbody2D
[RequireComponent(typeof(Animator))] // Ensures object will have an Animator
public class PlayerController : MonoBehaviour
{
    //ScriptableObjects
    [SerializeField]
    private CharacterData _characterData;
    [SerializeField]
    private FloatAsset _health;

    [SerializeField]
    private bool Invincible;

    [SerializeField]
    private Transform _attackPos;
    [SerializeField]
    private LayerMask _enemyLayer;

    //UI Elements
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private GameObject _playerUIPanel;


    //[SerializeField]
    private string _characterName;
    //[SerializeField]
    private float _characterSpeed = 5.0f;
    //[SerializeField]
    private float _attackRange;
    //[SerializeField]
    private int _lightDamage = 1;
    //[SerializeField]
    private float _lightHitstun = 1f;
    //[SerializeField]
    private int _heavyDamage = 4;
    //[SerializeField]
    private float _heavyHitstun = 1f;  
    //[SerializeField]
    private float _lightAtkCooldown = 0.5f;
    //[SerializeField]
    private float _heavyAtkCooldown = 1.5f;
    //[SerializeField] 
    private GameObject _hitParticleEmitter;
    //[SerializeField]
    private GameObject _slideEffect;

    

    private Animator _playerAnim;
    private SpriteRenderer _playerSpriteRenderer;
    private Vector2 _movementInput = Vector2.zero;
    private bool _facingRight = true;
    private bool _sliding = false;
    private bool _canSlide = true;
    private bool _canAttack = true;
    private bool _isPaused = false; 
    private PlayerInput _input;
    private bool _canMove = true;
    private bool _Downed = false;

    //Heat System: 
    public HeatManager _heatManager; 

    private Vector3 _startPos;
    private Vector3 _centerPos;
    private Vector3 _endPos;

    public CharacterData CharacterData { get { return _characterData; } set { _characterData = value; } }

    [Header("Down Settings")]
    [SerializeField] private float mashGoal = 100;
    [SerializeField] private float IncreasePerPress = 5;
    [SerializeField] private float DecreaseRate = 5;
    [SerializeField] private Canvas DownCanvas;
    private Slider ReviveBar;
    private float mashProgress = 0;
    private void Start()
    {
        ReviveBar = GetComponentInChildren<Slider>();
        ReviveBar.maxValue = mashGoal;
        DownCanvas.gameObject.SetActive(false);
        _playerAnim = gameObject.GetComponent<Animator>();

        _playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        _input = gameObject.GetComponent<PlayerInput>();

        Debug.Log(_input.currentControlScheme);
        _health.Value = 30;
        SetupCharacter();

        EnableUI(); // Activates player UI if player is spawned in
    }

    public void SetupCharacter()
    {
        _characterName = _characterData.CharacterName;
        _characterSpeed = _characterData.CharacterSpeed;
        _attackRange = _characterData.AttackRange;
        _lightDamage = _characterData.LightAtkDamage;
        _lightHitstun = _characterData.LightHitStun;
        _heavyDamage = _characterData.HeavyAtkDamage;
        _heavyHitstun = _characterData.HeavyHitStun;
        _lightAtkCooldown = _characterData.LightAtkCooldown;
        _heavyAtkCooldown = _characterData.HeavyAtkCooldown;
        _hitParticleEmitter = _characterData.HitParticleEmitter;
        _slideEffect = _characterData.SlideEffect;

        if (_playerAnim == null)
        {
            _playerAnim = gameObject.GetComponent<Animator>();
        }
        if (_playerSpriteRenderer == null)
        {
            _playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        _playerAnim.runtimeAnimatorController = _characterData.AnimatorController;
        _playerSpriteRenderer.sprite = _characterData.DefaultSprite;
    }

    // Gets direction from player input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (_canMove)
        {
            // not 100% sure why, but if you hold another direction while sliding,
            // the player will keep moving in the same direction
            // will reset when you let go of a direction/input a different direction
            //if (!_sliding && _canSlide) {
            if (!_sliding)
            {
                _movementInput = context.ReadValue<Vector2>();
            }
        }
        else
        {
            _movementInput = Vector2.zero;
        }
    }

    // Called when player presses light attack button
    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if(!_Downed)
        {
            if (context.performed) // Ensures functions only performed once on button press
            {
                //Debug.Log("light attack input");
                if (_canAttack)
                {
                    _playerAnim.SetTrigger("Light Attack");
                    _canSlide = false;
                    AttackEnemies(_lightDamage, _lightHitstun);
                    StartCoroutine(AttackCooldown(_lightAtkCooldown));
                }
            }
        }
        else
        {
            mashProgress += IncreasePerPress;
            ReviveBar.value = mashProgress;
            if(mashProgress >= mashGoal)
            {
                GetUp();
            }
        }
    }

    /*
    // Called when player presses heavy attack button
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if(!_Downed)
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
    }
    */

    public void OnSlide(InputAction.CallbackContext context) {
        if (context.performed && _canSlide && !_Downed) // Ensures functions only performed once on button press
        {
            //Debug.Log("Slide");
            _playerAnim.SetTrigger("Slide");
            _sliding = true;
            _canAttack = false;
            _canSlide = false;
            _characterSpeed *= 2f;
            _startPos = this.gameObject.transform.position;
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
        _characterSpeed /= 2f;
        _sliding = false;
        _endPos = this.gameObject.transform.position;
        _centerPos = (_startPos + _endPos) / 2;
        GameObject trail = Instantiate(_slideEffect, _centerPos, Quaternion.identity);
        float scaleX = Vector3.Distance(new Vector3(_startPos.x, 0, 0), new Vector3(_endPos.x, 0, 0));
        trail.transform.localScale = new Vector3(scaleX/5, 1, 1);
        _startPos = Vector3.zero;
        _endPos = Vector3.zero;
        _canAttack = true;
        // temporary bandage for game not taking input during a slide
        _movementInput = Vector2.zero;
        // extra delay so a player can't spam sliding?
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Slide disabled");
        _canSlide = true;
    }

    void FixedUpdate()
    {
        if(_Downed)
        {
            mashProgress -= DecreaseRate * Time.deltaTime;
            ReviveBar.value = mashProgress;
            
        }
        else 
        {
            PlayerMovement(); // Calls method to handle movement
        }
    }

    // Handles buffs that the player receives from Heat stacks
    public void BuffPlayer(int level)
    {
        if(level == 1)
        {
            _lightAtkCooldown *= .85f; 
        }
        else if (level >= 2)
        {
            _attackRange *= 1.25f; 
        }
        else
        {
            _lightAtkCooldown = CharacterData.LightAtkCooldown;
            _attackRange = CharacterData.AttackRange; 
        }
    }



    // Handles player movement and animations
    public void PlayerMovement()
    {
        if (!_playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // Adjusts vertical movement speed (halves it) and multiplies vector by time and speed
            Vector2 velocity = (new Vector2(_movementInput.x, _movementInput.y / 2)) * Time.deltaTime * _characterSpeed;

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
    }

    // Flips character horizontally
    public void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(Vector3.up * 180);
        DownCanvas.gameObject.GetComponent<RectTransform>().LookAt(Camera.main.transform.position);
    }

    // Finds enemies in range and calls their TakeDamage() method.
    public void AttackEnemies(int damage, float Hitstun)
    {
        Debug.Log("damage: " + damage);
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(_attackPos.position, _attackRange, _enemyLayer);
        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            enemiesToHit[i].GetComponent<EnemyAgent>().TakeDamage(damage, Hitstun);
            Instantiate(_hitParticleEmitter, enemiesToHit[i].gameObject.transform.position, Quaternion.identity);
            _heatManager.increaseHeat();
        }
        _canSlide = true;
    }

   
        // Creates a gizmo for attack area in editor
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPos.position, _attackRange);
    }

    public void TakeDamage(int damage)
    {
        if (!_sliding && !Invincible) 
        {
            _health.Value -= damage;
            if (_health.Value <= 0 && GameManagerScript.Instance.IsMultiplayer == false) 
            {
                //Debug.Log("Player Dead");
                gameOver();
            }
            else if (_health.Value <= 0 && GameManagerScript.Instance.IsMultiplayer == true)
            {
                PlayerDown();
            }
        }
    }

    public void PlayerDown()
    {
        _Downed = true;
        _playerAnim.SetBool("Walking", false);
        DownCanvas.gameObject.SetActive(true);
        ReviveBar.value = 0;
        Debug.Log(this.tag + " Down!");
        //commented this out because for some reason p1 and p2 have synched health
        /*
        if(GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>()._Downed) //if other player is also down
        {
            gameOver();
        }*/
    }

    public void GetUp()
    {
        _Downed = false;
        ReviveBar.value = 0;
        mashProgress = 0;
        DownCanvas.gameObject.SetActive(false);
        _health.Value = 15f;
        Debug.Log("Player got back up!");
    }

    public bool checkDown()
    {
        return _Downed;
    }
    public IEnumerator AttackCooldown(float cooldown)
    {
        _canAttack = false;
        yield return new WaitForSeconds(cooldown);
        _canAttack = true;
    }

    public IEnumerator AnimationTime(float cooldown)
    {
        _canMove = false;
        yield return new WaitForSeconds(cooldown);
        _canMove = true;
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

    public void gameOver() {
        Time.timeScale = 0;
        _gameOverPanel.SetActive(true);
        _isPaused = true;
    }

    public void EnableUI()
    {
        TextMeshProUGUI nametag = _playerUIPanel.GetComponentInChildren<TextMeshProUGUI>();
        nametag.text = _characterName;
        _playerUIPanel.SetActive(true);
    }
}
