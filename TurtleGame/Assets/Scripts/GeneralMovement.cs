using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GeneralMovement : MonoBehaviour
{
    [SerializeField]
    protected Collider2D crouchDisableCollider;
    [SerializeField]
    protected Collider2D crouchEnableCollider;
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected GeneralEnviroment envi;
    [SerializeField]
    protected GeneralAnimation anScript;
    [SerializeField]
    protected GameObject otherCharacter;
    [SerializeField]
    protected GameObject currentCharacter;
    [SerializeField]
    protected CinemachineVirtualCamera CM;
    [SerializeField]
    protected GameManager manager;



    //Speed
    protected float jumpForce = 30f;
    protected float crouchSpeed = .36f;
    protected float movementSmoothing = .05f;
    protected float moveSpeed = 40f;
    protected float floatVelocity = 5.0f;
    protected float ladderMS = 20f;
    protected float fallMultiplier = 1f;
    protected float lowJumpMultiplier = 2f;
    protected float airControlModifier = 0.85f;

    protected float slowingAir = 0.5f;
    protected float airTime = 1f;
    protected float airTimeCount = 0f;
    protected float shellMultiplier = 1.75f;

    //temporary value
    private float movementInputDirection;
    [SerializeField]
    protected float gravity;

    //bools
    [SerializeField]
    protected bool facingR = true;
    [SerializeField]
    protected bool crouch;
    [SerializeField]
    protected bool airControl = true;
    [SerializeField]
    protected bool canJump;
    [SerializeField]
    protected bool isLadder;
    [SerializeField]
    protected bool isJumping = false;

    //temporary bool
    [SerializeField]
    protected bool wasCrouching = false;
    [SerializeField]
    protected bool swimAnimationStop = true;
    [SerializeField]
    protected bool tempGrounded;
    [SerializeField]
    protected bool jumpleftDecreaseStopper = true;
    [SerializeField]
    protected bool jumpButtonPressed =false;

    // temporary Speed
    [SerializeField]
    protected Vector3 rVelocity = Vector3.zero;
    [SerializeField]
    protected float tMoveSpeed = 0f;
    [SerializeField]
    protected float currentFloat = 0f;

    //Timers
    protected float jumpTime = 0.25f;
    [SerializeField]
    protected float jumpTimeCounter;
    protected float springTime = 0.25f;
    [SerializeField]
    protected float springTimeCounter = 0;
    protected float houseInteractBuffer = 0;

    //limiters
    protected int jumpLimit = 1;
    [SerializeField]
    protected int jumpLeft;





    // Start is called before the first frame update
    protected virtual void Start()
    {
        gravity = rb.gravityScale;
    }

    private void Update()
    {
        dialogueHandler();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!manager.getIsDialogueOn())
        {
            envi.CheckSurroundings();
            CheckInput();
            EnviromentalEffects();
            Move();
        }
    }

    private void dialogueHandler()
    {
        if (manager.getIsDialogueOn())
        {
            if (manager.getCanPressed())
            {
                if (Input.GetButtonDown("Interact"))
                {
                    manager.dManager.nextSentence();
                }
            }
        }
    }

    public void CheckInput()
    {
        movementInputDirection = Input.GetAxis("Horizontal");
        tMoveSpeed = moveSpeed * movementInputDirection;

        if (movementInputDirection != 0)
            anScript.AnimationSet("WalkTrue");
        else
            anScript.AnimationSet("WalkFalse");

        if (Input.GetAxis("Vertical") != 0)
        {
            anScript.AnimationSet("isMovingVT");
        }
        else
        {
            anScript.AnimationSet("isMovingVF");
        }

        if (Input.GetButton("Crouch")) crouch = true;
        else { crouch = false; }

        //dialogue


    }

    //ENVIROMENTAL////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void EnviromentalEffects()
    {
        jumpEffect();
        FloatingWaterEffect();
        LadderEffect();
        SwitchHouseEffect();
        SpringEffect();
        ShellBoxEffect();
        airslow();
    }

    void jumpEffect()
    {
        //falling faster
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * rb.gravityScale * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (!Input.GetButton("Jump") && rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * rb.gravityScale * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        if (envi.isGrounded && !envi.isFloating)
        {
            if (!Input.GetButton("Jump"))
            {
                jumpLeft = jumpLimit;
                isJumping = false;
            }
            if(jumpTimeCounter<jumpTime-0.05f)anScript.AnimationSet("Landed");
            swimAnimationStop = true;

        }
        if (jumpLeft <= 0 || crouch)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    protected virtual void FloatingWaterEffect()
    {
        if (envi.isFloating)
        {
            rb.velocity = new Vector2(rb.velocity.x, floatVelocity);
            if (swimAnimationStop)
            {
                anScript.AnimationSet("Swimming");
                swimAnimationStop = false;
            }
        }
        else if (!envi.isFloating)
        {
            currentFloat = 0;
        }
    }

    void LadderEffect()
    {
        if (envi.isNearLadder && Input.GetButtonUp("Vertical"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (envi.isNearLadder && Input.GetButton("Vertical"))
        {
            isLadder = true;
            anScript.AnimationSet("Climb");
            tMoveSpeed = 0;
            float inputVertical = Input.GetAxis("Vertical");

            if (inputVertical > 0) rb.velocity = Vector2.up * ladderMS;
            if (inputVertical < 0) rb.velocity = Vector2.up * -ladderMS;
            rb.gravityScale = 0;

            anScript.AnimationSet("Landed");
        }
        else if (!envi.isNearLadder && isLadder)
        {
            isLadder = false;
            anScript.AnimationSet("ClimbCancel");
            rb.gravityScale = gravity;
            jumpLeft = jumpLimit;
        }
    }

    protected virtual void SpringEffect()
    {
        if (envi.isSpring)
        {
            springTimeCounter = springTime;
        }
        if (springTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1f);
            springTimeCounter-=Time.deltaTime;
            anScript.AnimationSet("Jump");
        }
    }

    protected virtual void ShellBoxEffect()
    {
        if (envi.isOnShellJump)
        {
            springTimeCounter = springTime;
        }
        if (springTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * shellMultiplier);
            springTimeCounter -= Time.deltaTime;
            anScript.AnimationSet("Jump");
        }
    }

    void SwitchHouseEffect()
    {
        if (houseInteractBuffer > 0) houseInteractBuffer -= Time.deltaTime;
        if (envi.isOnSwitchHouse)
        {

            if (Input.GetButtonDown("Interact") && houseInteractBuffer<=0)
            {
                Debug.Log("interact");
                otherCharacter.transform.position = transform.position;
                CM.Follow = otherCharacter.transform;
                otherCharacter.SetActive(true);
                currentCharacter.SetActive(false);
                houseInteractBuffer = 0.2f;
                manager.changePlayer(otherCharacter.name);
            }
        }
    }

    public virtual void RefreshOrb()
    {
        //
    }

    void airslow()
    {
        if (envi.isGrounded)
        {
            airTimeCount = 0.1f;
        }
        if (!envi.isGrounded)
        {
            if(airTimeCount<airTime)airTimeCount += Time.deltaTime;
            tMoveSpeed /= 1+(slowingAir*(airTimeCount/airTime));

        }
    }

    //MOVE////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Move()
    {
        //if stopped crouching check if can un-crouch
        if (!crouch)
        {
            if (envi.isUnderBlock)
            {
                crouch = true;
            }
            else
            {
                if (crouchDisableCollider != null) crouchDisableCollider.enabled = true;
                if (crouchEnableCollider != null) crouchEnableCollider.enabled = false;
                anScript.AnimationSet("CrouchCancel");
            }
        }

        if (envi.isGrounded || airControl)
        {
            if (crouch && envi.isGrounded)
            {
                doCrouch();
            }
            //MOVEMENT
            if (!envi.isGrounded) tMoveSpeed *= airControlModifier;
            rb.velocity = new Vector2(tMoveSpeed, rb.velocity.y);

            if (tMoveSpeed > 0 && !facingR)
            {
                Flip();
            }
            else if (tMoveSpeed < 0 && facingR)
            {
                Flip();
            }
            //JUMP
            if (jumpTimeCounter > 0) { jumpTimeCounter -= Time.deltaTime; }


            if (Input.GetButton("Jump"))
            {
                if (!isJumping && canJump)
                {
                    jumpButtonPressed = true;
                    isJumping = true;
                    if(!envi.isNearLadder)anScript.AnimationSet("Jump");
                    jumpTimeCounter = jumpTime;
                }
                if (jumpTimeCounter > 0 && isJumping)
                {

                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * (1 + jumpTime - jumpTimeCounter));
                }
                if (jumpTimeCounter <= 0 && isJumping && jumpleftDecreaseStopper)
                {

                    jumpleftDecreaseStopper = false;
                    jumpLeft--;
                    jumpTimeCounter = 0;
                }
            }
            else if (Input.GetButtonUp("Jump") && jumpButtonPressed)
            {
                jumpButtonPressed = false;

                isJumping = false;
                if (jumpleftDecreaseStopper)
                {
                    jumpLeft--;
                    jumpTimeCounter = 0;
                }
                jumpleftDecreaseStopper = true;
            }
        }
    }

           

        protected virtual void doCrouch()
        {
            Debug.Log(crouchDisableCollider + " " + crouchEnableCollider);
            tMoveSpeed *= crouchSpeed;
            if (crouchDisableCollider != null) crouchDisableCollider.enabled = false;
            if (crouchEnableCollider != null) crouchEnableCollider.enabled = true;
            anScript.AnimationSet("Crouch");
            Debug.Log(crouchDisableCollider.enabled + " " + crouchEnableCollider.enabled);
        }

        void Flip()
        {
            facingR = !facingR;
            transform.Rotate(0f, 180f, 0f);
        }
}
