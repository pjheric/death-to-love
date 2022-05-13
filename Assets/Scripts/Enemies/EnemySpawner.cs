using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Array that stores enemy prefabs, because of the weights it needs to be in a specific order: Henchman, Mugger, Bouncer")]
    [SerializeField] private GameObject[] Enemies;

    [Tooltip("Higher means more will spawn")]
    [SerializeField] private float HenchmanWeight = 1f;

    [Tooltip("Higher means more will spawn")]
    [SerializeField] private float MuggerWeight = 1f;

    [Tooltip("Higher means more will spawn")]
    [SerializeField] private float BouncerWeight = 1f;

    [Tooltip("the time between when this gameobject is activated and when InvokeRepeating gets called the first time")]
    [SerializeField] private float SpawnDelay;

    [Tooltip("the time between enemy spawns")]
    [SerializeField] private float SpawnRate;

    [Tooltip("the max enemies this spawner will produce")]
    [SerializeField] private int maxEnemies;

    [Tooltip("if true, keep spawning enemies until it is set to false, or the gameobject is disabled")]
    [SerializeField] private bool infinite;

    private bool complete = false; //if true then this spawner is done spawning
    private float netWeight;
    private Transform spawnpoint;
    private List<GameObject> spawnedEnemies;
    private List<float> weights;
   
    // Start is called before the first frame update
    void Start()
    {
        weights = new List<float>();

        weights.Add(HenchmanWeight);
        weights.Add(MuggerWeight);
        weights.Add(BouncerWeight);

        netWeight = HenchmanWeight + MuggerWeight + BouncerWeight;

        spawnedEnemies = new List<GameObject>();
        spawnpoint = this.transform;
        if(!complete)
        {
            startSpawning();
        }
    }

    public void SpawnEnemy()
    {
        if (spawnedEnemies.Count < maxEnemies || infinite)
        {
            if(!complete)
            {
                GameObject newEnemy;

                float randomWeight = Random.Range(0f, netWeight);
                for (int i = 0; i < weights.Count; i++)
                {
                    randomWeight -= weights[i];
                    if (randomWeight < 0)
                    {
                        newEnemy = Instantiate(Enemies[i], spawnpoint.position, Quaternion.identity);
                        spawnedEnemies.Add(newEnemy);
                        break;
                    }
                }
            }
        }
        else if(!infinite)
        {
            doneSpawning();
        }
    }

    public void doneSpawning()
    {
        complete = true;
    }

    public void startSpawning()
    {
        complete = false;
        InvokeRepeating("SpawnEnemy", SpawnDelay, SpawnRate);
    }

    public void setMaxEnemies(int max)
    {
        maxEnemies = max;
    }

    public void setEnemyWeights(float Henchman = 0f, float mugger = 0f, float bouncer = 0f)
    {
        HenchmanWeight = Henchman;
        MuggerWeight = mugger;
        BouncerWeight = bouncer;
    }

    public void setHencmanWeight(float Henchman = 0f)
    {
        HenchmanWeight = Henchman;
    }

    public void setMuggerWeight(float mugger = 0f)
    {
        MuggerWeight = mugger;
    }
    public void setBouncerWeight(float bouncer = 0f)
    {
        BouncerWeight = bouncer;
    }
}
