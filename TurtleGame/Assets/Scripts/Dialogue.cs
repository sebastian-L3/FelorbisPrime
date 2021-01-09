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
        delayTime = 0.1f;
        isDialogueFinish = true;


    }




}
