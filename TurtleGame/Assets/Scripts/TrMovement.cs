using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrMovement : GeneralMovement
{
    //timers
    [SerializeField]
    protected float slideTime = 1.3f;
    [SerializeField]
    protected float slideTimeCounter;
    [SerializeField]
    protected float slideDecay = 0.5f;
    [SerializeField]
    protected float slideDecayCounter;

    protected float shellThrowBuffer = 0.2f;
    [SerializeField]
    protected float shellThrowBufferCounter = 0f;
    protected float shellSummon = 1f;
    [SerializeField]
    protected float shellSummonCounter = 0f;

    [SerializeField]
    protected bool isSliding = false;
    [SerializeField]
    protected bool facingSlide;

    [SerializeField]
    protected Vector2 SlideVelocity;

    public float slideDashCooldown = 3f;
    public float slideDashCooldownCounter = 0f;

    protected float slideDashSpeed = 80f;

    [SerializeField]
    protected GameObject ShellObject;
    [SerializeField]
    protected Rigidbody2D rbShell;

    [SerializeField]
    protected GameObject ShellOnBack;
    [SerializeField]
    protected GameObject ShellOnBackClimbing;

    protected int summonAmmoMax = 1;
    public int summonAmmoCount = 1;

    public float time=1;


    protected override void Start()
    {
        envi = GetComponent<TrEnviroment>();
        anScript = GetComponent<TrAnimation>();

        //value
        gravity = rb.gravityScale;
        Time.timeScale = time;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!manager.getIsDialogueOn())
        {
            envi.CheckSurroundings();
            ChangeBGColor();
            CheckInput();
            EnviromentalEffects();
            specificChecks();
            Move();
            specificMovements();
        }
    }

    void specificChecks()
    {
        //slidings
        if (isSliding)
        {
            if (envi.isBumpedR)
            {
                facingSlide = !facingSlide;
            }
            if (envi.isBumpedL)
            {
                facingSlide = !facingSlide;
            }
        }
    }

    protected override void SpringEffect()
    {
        if (envi.isSpring)
        {
            springTimeCounter = springTime;
            AM.play("spring");
        }
        if (springTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1f);
            springTimeCounter -= Time.deltaTime;
            if (!(slideTimeCounter > 0))
            {
                Debug.Log("SpringEffectANJUMP");
                anScript.AnimationSet("Jump");
            }
        }
    }

    protected override void ShellBoxEffect()
    {
        if (envi.isOnShellJump)
        {
            springTimeCounter = springTime;
        }
        if (springTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * shellMultiplier);
            springTimeCounter -= Time.deltaTime;
            if (!(slideTimeCounter > 0))
            {
                anScript.AnimationSet("Jump");
            }
        }
    }

    protected override void FloatingWaterEffect()
    {
        if (envi.isFloating)
        {
            if(!isSliding)anScript.AnimationSet("Swimming");
            if (Input.GetButton("Crouch") && currentFloat > -(floatVelocity * 5))
            {
                currentFloat -= floatVelocity;
            }
            else if (currentFloat < (floatVelocity * 5))
            {
                currentFloat += floatVelocity;
    
            }
            rb.velocity = new Vector2(rb.velocity.x, currentFloat);
        }
        else if (!envi.isFloating)
        {
            currentFloat = 0;
        }
    }

    public override void RefreshOrb()
    {
        summonAmmoCount = summonAmmoMax;
        AM.play("refresh");
    }

    void specificMovements()
    {
        slideDash();
        shellBlock();
    }

    void slideDash()
    {
        if (slideDashCooldownCounter > 0) slideDashCooldownCounter -= Time.deltaTime;

        if (Input.GetButton("Skill 1") && slideDashCooldownCounter<=0)
        {
            if (!isSliding && ShellOnBack.activeSelf)
            {
                AM.play("dash");
                isSliding = true;
                slideTimeCounter = slideTime;
                facingSlide = facingR;
                if (crouchDisableCollider != null) crouchDisableCollider.enabled = false;
                if (crouchEnableCollider != null) crouchEnableCollider.enabled = true;
                Debug.Log("Slodi"+ crouchEnableCollider + " " + crouchEnableCollider.enabled +" / " + crouchDisableCollider + " " + crouchDisableCollider.enabled);
                anScript.AnimationSet("Sliding");
            }
        }
        if (isSliding)
        {
            if (crouchDisableCollider != null) crouchDisableCollider.enabled = false;
            if (crouchEnableCollider != null) crouchEnableCollider.enabled = true;
            crouch = true;
            if (facingSlide) tMoveSpeed = slideDashSpeed;
            else tMoveSpeed = -slideDashSpeed;

            rb.velocity = new Vector2(tMoveSpeed, rb.velocity.y);
            slideTimeCounter -= Time.deltaTime;
            if (Input.GetButton("Jump"))
            {
                slideTimeCounter = 0;
            }
        }
        if (slideTimeCounter <= 0 && isSliding)
        {

            isSliding = false;
            anScript.AnimationSet("SlidingCancel");
            slideDecayCounter = slideDecay;
            if (crouchDisableCollider != null) crouchDisableCollider.enabled = true;
            if (crouchEnableCollider != null) crouchEnableCollider.enabled = false;
            swimAnimationStop = true;
            crouch = false;
            slideDashCooldownCounter = slideDashCooldown;
        }
        if (slideDecayCounter > 0)
        {

            if (rb.velocity.x == 0)
            {

                float tempSpeedModif;
                if (slideDecayCounter < slideDecay / 4) tempSpeedModif = slideDecay / 4;
                else tempSpeedModif = slideDecayCounter;
                if (facingSlide) tMoveSpeed = slideDashSpeed * tempSpeedModif;
                else tMoveSpeed = -slideDashSpeed * tempSpeedModif;

                rb.velocity = new Vector2(tMoveSpeed, rb.velocity.y);
            }
            slideDecayCounter -= Time.deltaTime;
        }
    }

    void shellBlock()
    {
        if (!ShellObject.activeSelf && Input.GetButtonDown("Skill 2") && shellThrowBufferCounter <= 0)
        {
            ShellObject.transform.position = envi.FirePoint.position;
            ShellOnBack.SetActive(false);
            ShellOnBackClimbing.SetActive(false);
            ShellObject.SetActive(true);
            shellThrowBufferCounter = shellThrowBuffer;
        }
        else if (envi.isOnShell && ShellObject.activeSelf && shellThrowBufferCounter<=0 && Input.GetButtonDown("Skill 2") && envi.isGrounded)
        {
            ShellOnBack.SetActive(true);
            ShellOnBackClimbing.SetActive(true);
            ShellObject.SetActive(false);
            shellThrowBufferCounter = shellThrowBuffer;
        }
        if (shellThrowBufferCounter > 0) shellThrowBufferCounter-=Time.deltaTime;

        //summon
        if(!envi.isOnShell && ShellObject.activeSelf && shellThrowBufferCounter <= 0 && Input.GetButton("Skill 2") && envi.isGrounded && summonAmmoCount>0)
        {
            anScript.AnimationSet("ShootSummoning");
            if (shellSummonCounter > 0) shellSummonCounter -= Time.deltaTime;
            else if(shellSummonCounter <= 0)
            {
                anScript.AnimationSet("ShootSummoningFinish");
                ShellObject.SetActive(false);
                ShellOnBack.SetActive(true);
                ShellOnBackClimbing.SetActive(true);
                shellThrowBufferCounter = shellThrowBuffer;
                summonAmmoCount--;
            }
        }
        if(Input.GetButtonUp("Skill 2"))
        {
            shellSummonCounter = shellSummon;
            anScript.AnimationSet("ShootSummoningCancel");
        }
    }

    protected override void doCrouch()
    {
        if (!isSliding)
        {
            Debug.Log(crouchDisableCollider + " " + crouchEnableCollider);
            tMoveSpeed *= crouchSpeed;
            if (crouchDisableCollider != null) crouchDisableCollider.enabled = false;
            if (crouchEnableCollider != null) crouchEnableCollider.enabled = true;
            anScript.AnimationSet("Crouch");
            Debug.Log(crouchDisableCollider.enabled + " " + crouchEnableCollider.enabled);
        }
    }
}
