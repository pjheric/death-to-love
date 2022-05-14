using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private List<EnemySpawner> spawners;
    [SerializeField] private float HenchmanWeight;
    [SerializeField] private float MuggerWeight;
    [SerializeField] private float BouncerWeight;
    [SerializeField] private float enemies = 30f;
    [SerializeField] private bool infinite = false;

    [Header("Dialogue variables")] //put any necessary variables for dialogue system under here
    //if true, dialogue plays right when player enters the trigger
    [SerializeField] private bool startWithDialogue;

    //if true, dialogue plays when the wave is done
    [SerializeField] private bool endWithDialogue;

    // Start is called before the first frame update
    void Start()
    {
        spawners = new List<EnemySpawner>();
        foreach(GameObject spawner in GameObject.FindGameObjectsWithTag("EnemySpawner"))
        {
            spawners.Add(spawner.GetComponent<EnemySpawner>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            if (startWithDialogue)
            {
                //do the dialogue, have dialogue call activate spawners when done
            }
            else
            {
                activateSpawners();
            }
            this.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void activateSpawners()
    {
        foreach(EnemySpawner spawner in spawners)
        {
            spawner.setEnemyWeights(HenchmanWeight, MuggerWeight, BouncerWeight);
            spawner.setMaxEnemies(Mathf.Ceil(enemies / spawners.Count));
            spawner.startSpawning();
            spawner.setInfinite(infinite);
        }
    }
}
