using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    private List<Dialogue> dialogues = new List<Dialogue>();

    private Dialogue dialogueNow;
    private string textNow;
    private int idxDialogue;
    private bool isCharTalking;
    [SerializeField]
    private TextMeshProUGUI textUI;
    [SerializeField]
    private Canvas dialogueCanvas;
    [SerializeField]
    private TextMeshProUGUI continueText;
    [SerializeField]
    private Image char_1;
    [SerializeField]
    private Image char_2;

    [SerializeField]
    public List<SpriteCharacter> characterSprites;

    private string whoIsTalkingNow;
    private bool isChar_1Showing;

    [SerializeField]
    private GameManager manager;

    void Start()
    {
        initializeDialogues();
        startDialogue("Introduction", false);
    }

    // Update is called once per frame
    void Update()
    {
        checkDialogueFinish();
    }

    private void initializeDialogues()
    {
        string name;
        Sentence[] dialogueText;

        name = "Introduction";
        dialogueText = new Sentence[]
        {
            new Sentence("Welcome to Felorbis", null),
            new Sentence("My name is Turtle", null),
            new Sentence("And i will climb the Mountain", null),
            new Sentence("Let's Go", null),
        };
        dialogues.Add(new Dialogue(name, dialogueText));

        name = "Tiger";
        dialogueText = new Sentence[]
        {
            new Sentence("Oh, Hi Tiger!", "Turtle"),
            new Sentence("Are you climbing the Mountain too?", "Turtle"),
            new Sentence("Yeah! What are you doing here?", "Tiger"),
            new Sentence("Of course I'm climbing the Mountain too.", "Turtle"),
            new Sentence("Oh are you sure?", "Tiger"),
            new Sentence("I doubt a turtle can climb a mountain this high!", "Tiger"),
            new Sentence("Well.. if you insist I can help you climb this one!", "Tiger"),
            new Sentence("Sure, Thanks Tiger!", "Turtle"),
            new Sentence("Let's go to the house!", "Tiger"),
        };

        dialogues.Add(new Dialogue(name, dialogueText));

        name = "TigerWater";
        dialogueText = new Sentence[]
        {
            new Sentence("Uh, water I hate it!", "Tiger"),
            new Sentence("What's wrong Tiger?", "Turtle"),
            new Sentence("Uh the truth is i can't swim turtle", "Tiger"),
            new Sentence("Ah at this rate i won't be able to climb the Mountain!", "Tiger"),
            new Sentence("I can help you Tiger!", "Turtle"),
            new Sentence("You can ride on my shell", "Turtle"),
            new Sentence("Hmmmmm okay then", "Tiger"),
            new Sentence("Let's get going!", "Turtle"),
        };

        dialogues.Add(new Dialogue(name, dialogueText));

        name = "Eagle";
        dialogueText = new Sentence[]
        {
            new Sentence("What was that?", "Turtle"),
            new Sentence("WACKK!!", "Eagle"),
        };

        dialogues.Add(new Dialogue(name, dialogueText, 0.02f));

        name = "Ending";
        dialogueText = new Sentence[]
        {
            new Sentence("Wow, we finally made it!", "Turtle"),
            new Sentence("Meh it's kinda easy", "Tiger"),
            new Sentence("It was worth the effort.", "Turtle"),
            new Sentence("Yeah.. I guess", "Tiger"),
            new Sentence("Well..", "Turtle"),
            new Sentence("For now..", "Turtle"),
            new Sentence("Let's enjoy the view!", "Turtle"),
        };

        dialogues.Add(new Dialogue(name, dialogueText, 0.1f));

    }

    public void startDialogue(string name, bool charTalking)
    {
        //kasih info ke gamemanager udh nyala
        manager.setIsDialogueOn(true);

        //muncul dialogueCanvas
        dialogueCanvas.gameObject.SetActive(true);
        //animasi

        //freeze movement
        //kasih boolean di input

        //start dialogue
        idxDialogue = 0;
        dialogueNow = dialogues.Find(item => item.name == name);
        isChar_1Showing = false;
        whoIsTalkingNow = "";
        isCharTalking = charTalking;
        textNow = "";
        nextSentence();
    }

    public void endDialogue()
    {
        //kasitau gamemanager
        manager.setIsDialogueOn(false);

        //akhiri dialogue yg skrg dialogueNow
        textNow = "";
        dialogueCanvas.gameObject.SetActive(false);

    }

    public void nextSentence()
    {
        if (idxDialogue <= dialogueNow.dialogueText.Length - 1)
        {
            if (whoIsTalkingNow != dialogueNow.dialogueText[idxDialogue].talker)
            {
                if (isChar_1Showing) isChar_1Showing = false;
                else if (!isChar_1Showing) isChar_1Showing = true;
            }
            whoIsTalkingNow = dialogueNow.dialogueText[idxDialogue].talker;
            textNow = dialogueNow.dialogueText[idxDialogue].text;
            handleImageSprite();
            textUI.text = "";
            StartCoroutine(showDialogue());
            idxDialogue++;
        }
        else
        {
            endDialogue();
        }

    }

    private void handleImageSprite()
    {
        if (isCharTalking)
        {
            SpriteCharacter spriteNow = characterSprites.Find(character => character.name == whoIsTalkingNow);


            if (isChar_1Showing)
            {
                char_1.sprite = spriteNow.sprite;
                char_1.gameObject.SetActive(true);
                char_2.gameObject.SetActive(false);
            }
            else
            {
                char_2.sprite = spriteNow.sprite;
                char_2.gameObject.SetActive(true);
                char_1.gameObject.SetActive(false);
            }
        }
        else
        {
            char_1.gameObject.SetActive(false);
            char_2.gameObject.SetActive(false);
        }
    }

    private void checkDialogueFinish()
    {
        if (idxDialogue != 0)
        {
            if (textUI.text != dialogueNow.dialogueText[idxDialogue - 1].text)
            {
                continueText.gameObject.SetActive(false);
                manager.setCanPresesed(false);
            }
            else
            {
                continueText.gameObject.SetActive(true);
                manager.setCanPresesed(true);
            }
        }
    }

    IEnumerator showDialogue()
    {

        foreach (char c in textNow.ToCharArray())
        {
            textUI.text += c;
            yield return new WaitForSeconds(dialogueNow.delayTime);
        }

    }

    [System.Serializable]
    public class SpriteCharacter
    {
        public Sprite sprite;
        public string name;
    }
}
