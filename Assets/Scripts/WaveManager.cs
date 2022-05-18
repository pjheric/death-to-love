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

    private List<EnemySpawner> UsableSpawners;

    private int completeSpawners = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawners = new List<EnemySpawner>();
        UsableSpawners = new List<EnemySpawner>();
        foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("EnemySpawner"))
        {
            spawners.Add(spawner.GetComponent<EnemySpawner>());
        }
        Cam = GameObject.FindGameObjectWithTag("Cmvcam").GetComponent<CinemachineController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_dialogueManager)
        {
            if(_dialogueManager.IsDialogueOver())
            {
                activateSpawners();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            if(lockCam)
            {
                //locks camera to current position, commented out because this inadvertedly disables screenshake
                Cam.Snap();
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
        int canUse = 0;
        foreach(EnemySpawner spawner in spawners)
        {
            if(spawner.checkGrounded())
            {
                UsableSpawners.Add(spawner);
                canUse++;
            }
            else
            {
                UsableSpawners.Remove(spawner);
            }
        }
        float tempEnemies = 0;
        foreach(EnemySpawner spawner in UsableSpawners)
        {
            spawner.setEnemyWeights(HenchmanWeight, MuggerWeight, BouncerWeight);
            spawner.setMaxEnemies(Mathf.Ceil(enemies / canUse));
            spawner.setInfinite(infinite);
            spawner.startSpawning(this);
            spawning = true;
            tempEnemies += spawner.getMaxEnemies();
        }
        enemies = tempEnemies;
    }

    public void completeSpawning()
    {
        completeSpawners++;
        if(completeSpawners >= UsableSpawners.Count)
        {
            resetSpawners();
            spawning = false;
            Fighting = true;
        }
    }

    public void removeEnemy()
    {
        enemies--;
        checkIfDoneFighting();
    }

    public void checkIfDoneFighting()
    {
        if(enemies <= 0)
        {
            Debug.Log("No more fighting");
            Cam.Unsnap();
            Fighting = false;
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
