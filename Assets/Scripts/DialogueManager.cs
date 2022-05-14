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
    
    [SerializeField] Image Speaker1Image;
    [SerializeField] Image Speaker2Image;
 
    [SerializeField] TextMeshProUGUI SpeakerName; 
    [SerializeField] TextMeshProUGUI DialogueLine;
    private int currentIndex = 0;
    private DialogueData privateData; 
    
    
    public void StartDialogue(DialogueData data)
    {
        DialoguePanel.SetActive(true);
        //First, freeze time
        Time.timeScale = 0;
        privateData = data;
        DisplayDialogue();
    }
    public void DisplayDialogue()
    {
        DialogueLine.text = privateData.GetLine(currentIndex);
        SpeakerName.text = privateData.GetSpeaker(currentIndex); 
        
    }
    public void OnPressNextButton()
    {
       
    }

    public void EndDialogue()
    {
      
    }
}
