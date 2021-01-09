using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Game Components
    private float movementInputDirection;
    private Rigidbody2D rb;
    private CharacterController2D cc2d;
    private Animator an;
    private Transform t;
    public GameObject ShellObj;
    public ShellMovement shellMovementScript;
    
    //Variables
        //Speed
        public float movementSpeed= 160.0f;
        public float JumpForce = 25.0f;
        public float ladderMS = 20.0f;
        public float FloatVelocity = 5.0f;
        public float SpringVelocity = 120.0f;
        public float slideSpeed = 100.0f;
        public float shellJumpForce = 40f;
        //current Speed
        private float currentFloat;
        public float sliding;
        private float facing;

        private float gravityScale;


    //Timers
        public float shellWaitTime = 1f;
        public float shellCurrentWait;
        public float JumpTime = 0.25f;
        private float JumpTimeCounter;
        public float bumpTimer = 0.3f;
        private float bumpTimeCounter;
        public float summonTimer = 1f;
        public float summonTimeCounter;
        public float shellSpeedTime = 2f;
        public float shellSpeedCounter;

    //limiters
        public int jumpLimit = 1;
        private int jumpLeft;

   

    //bools
    public bool overlayInGameObj;
    public bool isGrounded;
    public bool isUnderBlock;
    public bool isNearLadder;
    public bool isSpring;
    public bool isFloating;
    public bool isSliding;
    public bool isBumpedL;
    public bool isBumpedR;
    //non overlay bools
    public bool canShoot;
    private bool isWalking;
    private bool isJumping;
    public bool isladder;
    public bool canJump;
    private bool cc2djump;
    private bool cc2dcrouch;
    //temporary bools
    private bool tempSwimAnim;
    private bool tempSummonAnim;
    public bool ButtonS1;

    //checkers
    public Transform groundCheck;
    public Transform HeadCheck;
    public Transform BodyCheck;
    public Transform BumpCheckL;
    public Transform BumpCheckR;
    public Transform FirePoint;

    //Checker size
    private float groundCheckRadius = 1f;
    private float headCheckRadius = 2f;
    private float bodyCheckRadius = 2.5f;
    private float bumpCheckRadius = 0.5f;

    //mask
    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public LayerMask whatIsSpring;
    public LayerMask whatIsWater;
    public LayerMask whatIsInGameObj;



    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        cc2d = GetComponent<CharacterController2D>();
        t = GetComponent<Transform>();
        shellMovementScript = ShellObj.GetComponent<ShellMovement>();

        gravityScale = rb.gravityScale;

        jumpLeft = jumpLimit;
        bumpTimeCounter = 0;
        shellSpeedCounter = 0;
        currentFloat = 0;
        shellCurrentWait = shellWaitTime;
        summonTimeCounter = summonTimer;

        canShoot = true;
        isSliding = false;
        tempSwimAnim = true;
        tempSummonAnim = true;
        isladder = false;
    }



    void Update()
    {
        UpdateAnimation();
        CheckMovementDirection();
        checkCanLadder();
        checkCanJump();
        checkCanSpring();
        checkCanFLoat();
        checkBumps();
        
        CheckInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void UpdateAnimation()
    {
        an.SetBool("isWalking", isWalking);
        if (isGrounded)
        {
            an.SetTrigger("IsLanded");
            an.SetTrigger("FinishedSwimming");
            an.ResetTrigger("IsSwimming");
            tempSwimAnim = true;
        }
        if (isladder) an.SetTrigger("IsLanded");
        if (isFloating && tempSwimAnim && !isSliding) //temp swim anim is true when out of water
        {
            an.ResetTrigger("FinishedSwimming");
            an.SetTrigger("IsSwimming");
            an.ResetTrigger("IsLanded");
            tempSwimAnim = false;
        }
    }

    private void AnimationButton(string Input)
    {
        switch (Input)
        {
            case "Jump":
                an.ResetTrigger("IsLanded");
                an.SetTrigger("IsJumping");
                break;
            case "Shoot":
                an.ResetTrigger("IsSummoning");
                an.SetTrigger("FinishedSummoning");
                break;
            case "ShootSummoning":
                an.SetTrigger("IsSummoning");
                an.ResetTrigger("FinishedSummoning");
                an.ResetTrigger("FailSummon");
                break;
            case "ShootSummoningCancel":
                an.SetTrigger("FailSummon");
                break;
            case "Crouch":
                an.ResetTrigger("FinishedCrouching");
                an.SetTrigger("IsCrouching");
                break;
            case "CrouchCancel":
                an.ResetTrigger("IsCrouching");
                an.SetTrigger("FinishedCrouching");
                break;
            case "Sliding":
                an.ResetTrigger("FinishedSliding");
                an.SetTrigger("IsSliding");
                break;
            case "SlidingCancel":
                an.SetTrigger("FinishedSliding");
                an.ResetTrigger("IsSliding");
                break;
        }
    }

    private void CheckMovementDirection()
    {
        if (movementInputDirection != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (movementInputDirection > 0)
        {
            facing = 1;
        }else if (movementInputDirection < 0)
        {
            facing = -1;
        }
    }

    

    private void checkCanJump()
    {

        if (isGrounded)
        {
            jumpLeft = jumpLimit;
            JumpTimeCounter = JumpTime;
        }
        if (jumpLeft <= 0 || isladder || isSliding || isFloating)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void checkCanLadder()
    {
        if(isNearLadder && Input.GetButtonUp("Vertical"))
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (isNearLadder && Input.GetButton("Vertical"))
        {
            isladder = true;
            rb.velocity = new Vector2(0, 0);
            if(isSliding)ButtonS1 = true;
            inputLadder();
        }
        else if(!isNearLadder && isladder)
        {
            isladder = false;
            rb.gravityScale = gravityScale;
            jumpLeft = jumpLimit;
        }
    }

    private void checkCanSpring()
    {
        if (isSpring)
        {
            rb.velocity = new Vector2(rb.velocity.x, SpringVelocity);
        }
    }

    private void checkCanFLoat()
    {
        if (isFloating)
        {
            if (rb.velocity.y < (FloatVelocity * 5))
            {
                currentFloat += FloatVelocity;
                rb.velocity = new Vector2(rb.velocity.x, (currentFloat));
            }
            
        }
        else if (!isFloating)
        {
            currentFloat = 0;
        }
    }

    private void checkBumps()
    {
        if (bumpTimeCounter > 0) bumpTimeCounter -= Time.deltaTime;
        if (isBumpedL &&  isSliding && bumpTimeCounter<=0)
        {
            cc2d.Flip();
            bumpTimeCounter = bumpTimer;
        }
        if (isBumpedR && isSliding && bumpTimeCounter <= 0)
        {
            cc2d.Flip();
            bumpTimeCounter = bumpTimer;
        }
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxis("Horizontal");
        inputJump();
        inputShoot();
        inputSlideDash();
        inputCrouch();
    }

    private void ApplyMovement()
    {
        cc2d.Move(movementInputDirection * movementSpeed * Time.fixedDeltaTime, cc2dcrouch, cc2djump, isGrounded);
        cc2djump = false;
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        overlayInGameObj = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsInGameObj);
            if(overlayInGameObj)isGrounded = true;
        isUnderBlock = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsGround);
        isNearLadder = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsLadder);
        isSpring = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsSpring);
        isFloating = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsWater);
        isBumpedL = Physics2D.OverlapCircle(BumpCheckL.position, bumpCheckRadius, whatIsGround);
        isBumpedR = Physics2D.OverlapCircle(BumpCheckR.position, bumpCheckRadius, whatIsGround);
    }


    private void inputJump()
    {
        if (Input.GetButton("Jump"))
        {
            if (canJump)
            {
                AnimationButton("Jump");
                if (JumpTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                    JumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    jumpLeft--;
                }
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpLeft--;
        }
        

    }

    private void inputShoot()
    {
        if(summonTimeCounter>0)summonTimeCounter -= Time.deltaTime;
        if (Input.GetButtonDown("Skill 2") && canShoot)
        {
            summonTimeCounter = summonTimer;
            ShellObj.transform.position = FirePoint.position;
            ShellObj.SetActive(true);
            shellMovementScript.setFacing(FirePoint.right.x);
            shellMovementScript.spawnSimulation();

            canShoot = false;

        }
        else if (Input.GetButton("Skill 2") && !canShoot && isGrounded &&!isWalking) //summon
        {
            if (shellCurrentWait <= 0)
            {
                if (!tempSummonAnim)
                {
                    AnimationButton("Shoot");
                    ShellObj.SetActive(false);
                    tempSummonAnim = true;
                }
                canShoot = true;
                shellCurrentWait = shellWaitTime;
            }
            else if(summonTimeCounter <= 0)
            {
                if (tempSummonAnim)
                {
                    AnimationButton("ShootSummoning");
                    tempSummonAnim = false;
                    summonTimeCounter = summonTimer;
                }
                shellCurrentWait -= Time.deltaTime;

            }

        }
        
        if (Input.GetButtonUp("Skill 2"))
        {
            shellCurrentWait = shellWaitTime;
            AnimationButton("ShootSummoningCancel");
            tempSummonAnim = true;
        }
    }

    private void inputCrouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            cc2dcrouch = true;
            AnimationButton("Crouch");
            
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            cc2dcrouch = false;
            if (!isUnderBlock)
            {
                AnimationButton("CrouchCancel");
            }
        }
        if (!isUnderBlock && !cc2dcrouch)
        {
            AnimationButton("CrouchCancel");
        }
    }

    private void inputSlideDash()
    {
        if(Input.GetButtonDown("Skill 1")){ ButtonS1 = true;}
        if(ButtonS1 && !isSliding && canShoot)
        {
            if (!isSliding)
            {
                AnimationButton("Sliding");
            }
            sliding = slideSpeed;
            Debug.Log(t.right + " " + t.right * sliding);
            rb.AddForce(t.right*sliding);
            isSliding = true;
            cc2dcrouch = true;
            shellSpeedCounter = shellSpeedTime;
            ButtonS1 = false;
        }
        else if(ButtonS1 && isSliding)
        {
            AnimationButton("SlidingCancel");
            isSliding = false;
            cc2dcrouch = false;
            ButtonS1 = false;
        }
        if (isSliding)
        {
   
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Skill 2"))
            {
                    AnimationButton("SlidingCancel");
                    isSliding = false;
                    cc2dcrouch = false;

                rb.AddForce(t.up * shellJumpForce);
                ShellObj.transform.position = FirePoint.position;
                    ShellObj.SetActive(true);

                    shellMovementScript.setFacing(FirePoint.right.x);
                    shellMovementScript.spawnSimulation();

                    canShoot = false;

            }
            
        }
        if (sliding > 0)
        {
            rb.AddForce(t.right * sliding);
            if(groundCheck)sliding -= shellSpeedCounter;
            shellSpeedCounter++;
            if (groundCheck && !isSliding) sliding -= shellSpeedCounter * 50;
        }
    }

    private void inputLadder()
    {
        float inputVertical = Input.GetAxis("Vertical");

        if(inputVertical>0)rb.velocity = new Vector2(rb.velocity.x,ladderMS);
        if (inputVertical < 0) rb.velocity = new Vector2(rb.velocity.x, -ladderMS);
        rb.gravityScale = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(HeadCheck.position, headCheckRadius);
        Gizmos.DrawWireSphere(BodyCheck.position, bodyCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckL.position, bumpCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckR.position, bumpCheckRadius);
    }
}
