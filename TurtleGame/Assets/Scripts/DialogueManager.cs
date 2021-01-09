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
        startDialogue("Contoh");
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

        name = "Contoh";
        dialogueText = new Sentence[]
        {
            new Sentence("Halo semuanya", "Turtle"),
            new Sentence("Kami kembali lagi", "Tiger"),
            new Sentence("di pertemuan ini", "Turtle"),
        };

        dialogues.Add(new Dialogue(name, dialogueText));


    }

    public void startDialogue(string name)
    {
        //kasih info ke gamemanager udh nyala
        manager.setIsDialogueOn(true);

        //muncul dialogueCanvas
        dialogueCanvas.gameObject.SetActive(true);
        //animasi

        //freeze movement
        //kasih boolean di input

        //start dialogue
        dialogueNow = dialogues.Find(item => item.name == name);
        isChar_1Showing = false;
        whoIsTalkingNow = "";
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
