// Contributor(s): Ben, Nathan More
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    [SerializeField] public int Health = 5;

    //the object this agent is moving towards
    [SerializeField] protected Vector3 target;
    [SerializeField] protected float attackRange = 1f; //distance the enemy's attack can reach
    [SerializeField] protected float MoveSpeed = 1.5f; //speed that the agent moves at
    [SerializeField] protected float AttackSpeed = 3f; //rate at which enemy can attack, higher number means less frequent attacks
    [SerializeField] protected float BackupDist = 3f; //rate at which enemy can attack, higher number means less frequent attacks
    [SerializeField] private Transform attackPos; // Location of attack range circle
    [SerializeField] private LayerMask playerLayer; // Player layer mask
    [SerializeField] private int attackDamage; // Damage dealt
    [SerializeField] private float attackArea; // Area of circle for melee attacks

    // Determines if enemy is facing right
    private bool facingRight = false;

    //reference to the NavmeshAgent component on this gameobject
    protected NavMeshAgent agent;

    //references the sprite renderer for the enemy
    protected SpriteRenderer Sprite;

    //Controls the animations for the AI
    protected Animator Anim;

    //bool that determines if the AI is chasing the player or not, in theory the AI should chase if the player is outside of attack range
    protected bool chase = true;

    //if true, AI can attack, if false, AI is in a state where he cannot attack
    protected bool Attack = true;

    //if true, AI is walking somewhere
    protected bool walking = false;

    //reference to the player
    protected GameObject player;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            target = player.transform.position; //get the player's transform
        }

        Anim = GetComponent<Animator>();
        Sprite = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = MoveSpeed; //sets the movement speed of the agent

        //need these to keep the agent from moving weirdly, not sure how they work but the guy
        //in the tutorial used them 
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        Anim.SetBool("Walking", walking);
        if(target != null) //if a player was found
        {
            checkSide(); //make sure the enemy is oriented to be facing the target
            
            if(Attack) //if we're able to attack and we are targeting a player
            {
                attack();
            }
            else
            {
                float distanceToTarget = Vector3.Distance(transform.position, target);
                if (distanceToTarget < 0.5)
                {
                    idle();
                }
            }
                
            

        }
        else
        {
            //because the player doesnt spawn until input is given, we have to keep checking for a player
            //This will 100% be removed before the final version
            player = GameObject.FindGameObjectWithTag("Player");
            if(player)
            {
                target = player.transform.position; //get the player's transform
            }
            else
            {
                idle();
            }
        }
    }

    //when the agent isnt attacking, make them run away by the backup dist
    protected virtual void backup()
    {
        int headsOrTails = Random.Range(0, 2);
        if (headsOrTails == 0)
        {
            //Debug.Log("Minus");
            target -= new Vector3(BackupDist, 0f, 0f);
        }
        else
        {
            //Debug.Log("Plus");
            target += new Vector3(BackupDist, 0f, 0f);
        }
        agent.SetDestination(target); 
        walking = true;
    }

    //I imagine flipping an enemy will take more than just flipping the sprite, so this flips the enemy
    protected void checkSide()
    {
        //check the sprite's relative distance to the player, if the distance on the X is negative, then the enemy is on the left of the player and we need to flip it
        if ((this.transform.position - player.transform.position).x < 0 && facingRight == false)
        {
            //Sprite.flipX = true;
            Flip();
        }
        else if ((this.transform.position - player.transform.position).x > 0 && facingRight == true)
        {
            //Sprite.flipX = false;
            Flip();
        }
    }

    // Flips character horizontally
    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    protected virtual void attack()
    {
        
        target = player.transform.position;
        walking = true;
        if (Vector3.Distance(target, transform.position) < attackRange) //if enemy is in range of player, attack
        {
            //Debug.Log("Enemy attacks!");
            Anim.SetTrigger("Attack");
            MeleeAttack();
            StartCoroutine(AttackCooldown());
            
        }
        else
        {
            agent.SetDestination(target);
        }
    }

    // Handles incoming attacks
    public void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage.");
        Health -= damage; 
        if(Health <= 0)
        {
            Debug.Log("He's dead, you can stop mashing now");
            Destroy(this.gameObject); //destroy actually has the ability to add a delay, so once we get an animation for death we can delay destroying until the animation is done
        }
    }

    //set agent to just stand where it is located
    protected void idle()
    {
        agent.SetDestination(agent.transform.position);
        walking = false;
    }

    //waits AttackSpeed Seconds between attacks
    protected IEnumerator AttackCooldown()
    {
        backup();
        Attack = false;
        yield return new WaitForSeconds(Mathf.Abs(AttackSpeed));
        Attack = true;
    }

    // Creates a gizmo for attack area in editor
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackArea);
    }

    public void MeleeAttack()
    {
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPos.position, attackArea, playerLayer);
        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            Debug.Log("At least one player");
            enemiesToHit[i].GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }
}
