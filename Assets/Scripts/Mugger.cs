using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mugger : EnemyAgent
{
    [SerializeField] protected GameObject Knife;

    // Start is called before the first frame update
    override protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
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

        BackupDist += 5; //we want the mugger to be far away, so whatever the basic backup distance is, make it farther
    }

    // Update is called once per frame
    override protected void Update()
    {
        Anim.SetBool("Walking", walking);
        if (target != null) //if a player was found
        {
            target = player.transform.position;
            checkSide(); //make sure the enemy is oriented to be facing the target

            if (Attack) //if we're able to attack and we are targeting a player
            {
                attack();
            }
            else
            {
                
                backup();
            }
        }
        else
        {
            //because the player doesnt spawn until input is given, we have to keep checking for a player
            //This will 100% be removed before the final version
            player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                target = player.transform.position; //get the player's transform
            }
            else
            {
                idle();
            }
        }
    }

    protected override void attack()
    {
        walking = true;
        if(Vector3.Distance(target, transform.position) < BackupDist)
        {
            if(Knife)
            {
                GameObject newKnife;
                if(facingRight)
                {
                    newKnife = Instantiate(Knife, transform.position + new Vector3(1f, Sprite.bounds.size.y / 2f, 0f), Quaternion.identity);
                }
                else
                {
                    newKnife = Instantiate(Knife, transform.position - new Vector3(1f, -1 * (Sprite.bounds.size.y / 2f), 0f), Quaternion.identity);
                }
                KnifeDamage KD  = newKnife.GetComponentInChildren<KnifeDamage>();
                KD.setThrower(this.gameObject);
            }
            Anim.SetTrigger("Attack");
            StartCoroutine(AttackCooldown());
        }
        else
        {
            backup();
        }
    }

    protected override void backup()
    {
        Vector3 tempTarget = target;
        if(this.transform.position.x - tempTarget.x > 0)
        {
            //Debug.Log("Target on left");
            tempTarget += new Vector3(BackupDist, 0f, 0f);
        }
        else if (this.transform.position.x - tempTarget.x < 0)
        {
            //Debug.Log("Target on right");
            tempTarget -= new Vector3(BackupDist, 0f, 0f);
        }
        agent.SetDestination(tempTarget);
        walking = true;
    }
}
