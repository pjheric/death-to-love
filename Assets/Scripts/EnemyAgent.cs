using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    [SerializeField] public int Health = 5;

    //the object this agent is moving towards
    [SerializeField] Transform target;
    [SerializeField] private float attackRange = 1f; //distance the enemy's attack can reach
    [SerializeField] private float MoveSpeed = 1.5f; //speed that the agent moves at
    [SerializeField] private float AttackSpeed = 1f; //rate at which enemy can attack

    //reference to the NavmeshAgent component on this gameobject
    private NavMeshAgent agent;

    //references the sprite renderer for the enemy
    private SpriteRenderer Sprite;

    //bool that determines if the AI is chasing the player or not, in theory the AI should chase if the player is outside of attack range
    private bool chase = true;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; //get the player's transform

        Sprite = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();

        agent.speed = MoveSpeed; //sets the movement speed of the agent

        //need these to keep the agent from moving weirdly, not sure how they work but the guy
        //in the tutorial used them 
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(target) //if a player was found
        {
            //if the target is in attack range, stop chasing the player
            if(Vector3.Distance(target.position, transform.position) < attackRange)
            {
                chase = false;
            }
            else
            {
                chase = true;
            }

            if(chase)//if we're chasing the target
            {
                //bool stops agent's movement
                agent.isStopped = false;

                checkSide(); //make sure the enemy is oriented to be facing the player

                agent.SetDestination(target.position); //go to wherever the player is
            }
            else //if we're in attack range
            {
                attack();
                agent.isStopped = true;
            }
                
        }
        else
        {
            //because the player doesnt spawn until input is given, we have to keep checking for a player
            //This will 100% be removed before the final version
            target = GameObject.FindGameObjectWithTag("Player").transform; //get the player's transform
        }
    }

    //I imagine flipping an enemy will take more than just flipping the sprite, so this flips the enemy
    private void checkSide()
    {
        //check the sprite's relative distance to the player, if the distance on the X is negative, then the enemy is on the left of the player and we need to flip it
        if ((this.transform.position - target.position).x < 0)
        {
            Sprite.flipX = true;
        }
        else
        {
            Sprite.flipX = false;
        }
    }

    private void attack()
    {
        //Debug.Log("Enemy attacks!");
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
}
