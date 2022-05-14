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
    [SerializeField] private bool lockCam = false;

    private CinemachineController Cam;
    private bool spawning = false;
    private bool Fighting = false;

    [Header("Dialogue variables")] //put any necessary variables for dialogue system under here
    //Array containing DialogueData. One DialogueData = one wave worth of dialogue
    [SerializeField] private DialogueData[] _dialogueList;
    private int _currentDialogueSection = 0; 
    //Dialogue Manager object
    [SerializeField] private DialogueManager _dialogueManager; 
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
        Cam = GameObject.FindGameObjectWithTag("Cmvcam").GetComponent<CinemachineController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawning)
        {
            int i = 0;
            foreach (EnemySpawner spawner in spawners)
            {
                if (spawner.checkComplete())
                {
                    i++;
                }
            }

            if (i == spawners.Count)
            {
                resetSpawners();
                spawning = false;
            }
            Fighting = true;
        }
        else if(Fighting)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log(enemies.Length);
            if (enemies.Length <= 0)
            {
                //Cam.unlockCam();
                Fighting = false;
            }
            /*
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = null;
            }*/
        }
        else if(_dialogueManager.IsDialogueOver())
        {
            activateSpawners(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            if(lockCam)
            {
                //locks camera to current position, commented out because this inadvertedly disables screenshake
                //Cam.lockCam();
            }

            if (startWithDialogue)
            {
                //do the dialogue, have dialogue call activate spawners when done
                _dialogueManager.StartDialogue(_dialogueList[_currentDialogueSection]);
                
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
            spawning = true;
        }
    }

    private void resetSpawners()
    {
        Debug.Log("Reset");
        foreach (EnemySpawner spawner in spawners)
        {
            spawner.resetSpawner();
        }
    }
}
