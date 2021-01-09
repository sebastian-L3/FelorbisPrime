using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite turtleShellOnSprite;

    [SerializeField]
    private Sprite turtleShellOffSprite;

    [SerializeField]
    private GameObject turtleUI;

    [SerializeField]
    private Image turtleShellIImg;

    [SerializeField]
    private GameObject tigerUI;
    [SerializeField]
    private TextMeshProUGUI skillAmountTxt;

    [SerializeField]
    private TrMovement TurtleScript;

    [SerializeField]
    private GrMovement TigerScript;

    [SerializeField]
    private string playerRightNow;

    [SerializeField]
    public DialogueManager dManager;

    private bool isDialogueOn = false;
    private bool canPressedFDialogue;


    [SerializeField]
    private CooldownBar turtleBar;

    [SerializeField]
    private CooldownBar tigerBar;

    // Start is called before the first frame update
    void Start()
    {
        turtleShellIImg.sprite = turtleShellOnSprite;
        turtleBar.setMaxValue(TurtleScript.slideDashCooldown);
        turtleBar.setValueToMax();
        tigerBar.setMaxValue(TurtleScript.slideDashCooldown);
        tigerBar.setValueToMax();
    }

    // Update is called once per frame
    void Update()
    {
        checkSkill();
        turtleBar.replaceValue(TurtleScript.slideDashCooldown - TurtleScript.slideDashCooldownCounter);
        tigerBar.replaceValue(TurtleScript.slideDashCooldown - TigerScript.mineThrowBCooldownCounter);
    }

    private void checkSkill()
    {
        string ammoTxt;
        if (playerRightNow == "Turtle")
        {
            turtleSkill();
        }
        else if (playerRightNow == "Tiger")
        {
            tigerSkill();
        }
    }

    private void turtleSkill()
    {
        tigerUI.gameObject.SetActive(false);
        turtleUI.gameObject.SetActive(true);
        if (TurtleScript.summonAmmoCount == 1) //on
        {
            turtleShellIImg.sprite = turtleShellOnSprite;
        }
        else if (TurtleScript.summonAmmoCount == 0) // off
        {
            turtleShellIImg.sprite = turtleShellOffSprite;
        }
    }

    private void tigerSkill()
    {
        turtleUI.gameObject.SetActive(false);
        tigerUI.gameObject.SetActive(true);
        string ammoTxt = ": " + TigerScript.skillAmmoCount;
        skillAmountTxt.text = ammoTxt;
    }


    public void changePlayer(string name)
    {
        playerRightNow = name;
        // turtle / tiger
    }

    public void setIsDialogueOn(bool var)
    {
        isDialogueOn = var;
    }

    public bool getIsDialogueOn()
    {
        return isDialogueOn;
    }

    public void setCanPresesed(bool var)
    {
        canPressedFDialogue = var;
    }

    public bool getCanPressed()
    {
        return canPressedFDialogue;
    }
}
