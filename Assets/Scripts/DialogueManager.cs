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
    private bool _isDialogue = false; 
    [SerializeField] GameObject DialoguePanel; 
    
    [SerializeField] Image Speaker1Image;
    [SerializeField] Image Speaker2Image;
 
    [SerializeField] TextMeshProUGUI SpeakerName; 
    [SerializeField] TextMeshProUGUI DialogueLine;
    [SerializeField] private float imageTransparency = 0.6f; 
    private int currentIndex = 0;
    private DialogueData privateData; 
    
    
    public void StartDialogue(DialogueData data)
    {
        _isDialogue = true; 
        DialoguePanel.SetActive(true);
        //First, freeze time
        Time.timeScale = 0;
        privateData = data;
        DisplayDialogue();
    }
    private void DisplayDialogue()
    {
        DialogueLine.text = privateData.GetLine(currentIndex);
        SpeakerName.text = privateData.GetSpeaker(currentIndex);
        DisplayImage(); 
    }

    private void DisplayImage()
    {
        Speaker1Image.sprite = privateData.GetImage(currentIndex); 
        if(privateData.GetLength() != currentIndex + 1)
        {
            Speaker2Image.sprite = privateData.GetImage(currentIndex);
            var tempColor = Speaker2Image.color;
            tempColor.a = imageTransparency; 
        }
    }
    public void OnPressNextButton()
    {
       if(privateData.GetLength() != currentIndex + 1)
       {
            currentIndex += 1;
            DisplayDialogue(); 
       }
       else
       {
            EndDialogue(); 
       }
    }

    public void EndDialogue()
    {
        _isDialogue = false; 
        Time.timeScale = 1;
        currentIndex = 0;
        DialoguePanel.SetActive(false); 
    }

    public bool IsDialogueOver()
    {
        return _isDialogue; 
    }
}
