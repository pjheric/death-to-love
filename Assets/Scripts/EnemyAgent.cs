using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    //the object this agent is moving towards
    [SerializeField] Transform target;

    //reference to the NavmeshAgent component on this gameobject
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; //get the player's transform
        agent = GetComponent<NavMeshAgent>();

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
            agent.SetDestination(target.position); //goes to wherever the player is
        }
        else
        {
            //because the player doesnt spawn until input is given, we have to keep checking for a player
            //This will 100% be removed before the final version
            target = GameObject.FindGameObjectWithTag("Player").transform; //get the player's transform
        }
    }
}
