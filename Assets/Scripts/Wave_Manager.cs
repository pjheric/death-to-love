using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Manager : MonoBehaviour
{
    struct WaveInfo //holds all the info for a single wave
    {
        bool locked;
        int numenemies;
        float HenchmanWeight;
        float MuggerWeight;
        float BouncerWeight;
    }

    [SerializeField] private EnemySpawner topRightSpawner;
    [SerializeField] private EnemySpawner bottomRightSpawner;
    [SerializeField] private EnemySpawner topLeftSpawner;
    [SerializeField] private EnemySpawner bottomLeftSpawner;
    [SerializeField] private List<WaveInfo> waves;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
