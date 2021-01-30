using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue
{
    public string name { get; set; }


    public Sentence[] dialogueText { get; set; }

    public bool isDialogueFinish { get; set; }

    public float delayTime { get; set; }

    public Dialogue(string name, Sentence[] dialogueText)
    {
        this.name = name;
        this.dialogueText = dialogueText;
        delayTime = 0.05f;
        isDialogueFinish = true;


    }

    public Dialogue(string name, Sentence[] dialogueText, float delay)
    {
        this.name = name;
        this.dialogueText = dialogueText;
        delayTime = delay;
        isDialogueFinish = true;


    }




}
