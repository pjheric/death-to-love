using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Linq; 
using UnityEngine.Audio;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject DialoguePanel; 
    [SerializeField] GameObject inGameUI; 
    
    [SerializeField] Image Speaker1Image;
    [SerializeField] Image Speaker2Image;

    [SerializeField] GameObject SpeakerName1;
    [SerializeField] GameObject SpeakerName2;
    [SerializeField] TextMeshProUGUI DialogueLine;

    [SerializeField] AudioSource musicSource;

    
    //All dialogue is just an array of list of strings
    //Array index: wave (0, 1, 2, 3, 4)
    //List index: actual dialogue 
    [System.Serializable]
    public struct DialogueWrapper
    {
        public List<string> lines;
    }
    public List<DialogueWrapper> dialogue = new List<DialogueWrapper>(); 
    //Each dialogue starts with either M or G then space then the actual dialogue;
    //This is to distinguish the name of the speaker
    public int currentIndex = 0; 

    public void StartDialogue()
    {
        musicSource.volume = 0.30f;
        DialoguePanel.SetActive(true); 
        inGameUI.SetActive(false);
        //First, freeze time
        Time.timeScale = 0;
        //DisplayDialogue(dialogue[waveManager.currentWaveNum].lines[currentIndex]); 
    }
    public void DisplayDialogue(string content)
    {
        
        char speakername = content[0];
        string speakerBody = content.Substring(2);
        if(speakername == 'M')
        {
            // Vector3 audioPos = Vector3.right*1;      
            // GameObject temp = Instantiate(runner,audioPos,Quaternion.identity);//spawns in left ear
            // temp.GetComponent<SFXRunner>().clip = speechClip;
            DialogueLine.alignment = TextAlignmentOptions.TopRight;
            SpeakerName1.SetActive(false);
            SpeakerName2.SetActive(true); 
        }
        else if(speakername == 'G')
        {
            // Vector3 audioPos = Vector3.left*1;      
            // GameObject temp = Instantiate(runner,audioPos,Quaternion.identity);//spawns in left ear
            // temp.GetComponent<SFXRunner>().clip = speechClip;
            DialogueLine.alignment = TextAlignmentOptions.TopLeft;
            SpeakerName1.SetActive(true);
            SpeakerName2.SetActive(false);
        }
        DialogueLine.text = speakerBody; 
    }
    public void OnPressNextButton()
    {
        if(currentIndex == dialogue[waveManager.currentWaveNum].lines.Count - 1)
        {
            EndDialogue();
            return; 
        }
        currentIndex += 1;
        DisplayDialogue(dialogue[waveManager.currentWaveNum].lines[currentIndex]);
    }

    public void EndDialogue()
    {
        lockPlayerManager.ganglimLock = false;
        lockPlayerManager.morriganLock = false;
        musicSource.volume = .70f;
        currentIndex = 0;
        DialoguePanel.SetActive(false);
        inGameUI.SetActive(true);
        Time.timeScale = 1f;
        if (waveManager.currentWaveNum == 0)
        {
            mm.gameStartFunc();
        }
        else
        {
            wm.startWave();
        }
    }
}
