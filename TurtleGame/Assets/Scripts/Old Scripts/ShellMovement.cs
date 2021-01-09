using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{
    private float movementInputDirection;
    private Rigidbody2D rb;
    private CharacterController2D cc2d;
    private Animator an;
    private Transform t;
    private CapsuleCollider2D col;


    public float FloatVelocity = 5.0f;
    public float SpringVelocity = 120.0f;
    public float gravityScale;

    public float slideSpeed = 100.0f;
    public float touchedSlideSpeed = 20.0f;
    public float sliding;
    private float facing;

    public float bumpTimer = 0.3f;
    public float bumpTimeCounter;
    public float bumpTimerPlayer = 0.1f;
    public float bumpTimeCounterPlayer;
    public float shellWaitTime = 1f;
    public float shellCurrentWait;


    public Transform groundCheck;
    public Transform HeadCheck;
    public Transform BodyCheck;
    public Transform BumpCheckL;
    public Transform BumpCheckR;

    public bool isGrounded;

    public bool isNearLadder;
    public bool isSpring;
    public bool isFloating;
    public bool isSliding;
    public bool isBumpedL;
    public bool isBumpedR;
    public bool isBumpedLPlayer;
    public bool isBumpedRPlayer;
    public bool JustSpawned;

    public float groundCheckRadius = 1f;
   
    public float bodyCheckRadius = 2f;
    public float bumpCheckRadius = 0.5f;

    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public LayerMask whatIsSpring;
    public LayerMask whatIsWater;
    public LayerMask whatIsPlayer;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        cc2d = GetComponent<CharacterController2D>();
        t = GetComponent<Transform>();
        col = GetComponent<CapsuleCollider2D>();

        gravityScale = rb.gravityScale;
        spawnSimulation();
    }



    void Update()
    {
        checkCanSpring();
        checkCanFLoat();
        checkBumps();

        inputSlideDash();
 
    }

    private void FixedUpdate()
    {
        CheckSurroundings();
    }

    public void spawnSimulation()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        cc2d = GetComponent<CharacterController2D>();
        t = GetComponent<Transform>();
        col = GetComponent<CapsuleCollider2D>();

        gravityScale = rb.gravityScale;
            
            an.ResetTrigger("FinishedSliding");
            an.SetTrigger("IsSliding");
            
            sliding = facing * slideSpeed;
            isSliding = true;
            bumpTimeCounter = 0;
            bumpTimeCounterPlayer = 0.3f;
            shellCurrentWait = shellWaitTime;
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
            if (rb.velocity.y < FloatVelocity * 2.5) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + FloatVelocity);
        }
    }

    private void checkBumps()
    {
        if (bumpTimeCounter > 0) bumpTimeCounter -= Time.deltaTime;
        if(bumpTimeCounterPlayer > 0) bumpTimeCounterPlayer -= Time.deltaTime;

        if (isBumpedL && isSliding && bumpTimeCounter <= 0)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            sliding = sliding * -1;
            bumpTimeCounter = bumpTimer;
        }
        if (isBumpedR && isSliding && bumpTimeCounter <= 0)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            sliding = sliding * -1;
            bumpTimeCounter = bumpTimer;
        }
        if (isBumpedLPlayer && bumpTimeCounterPlayer <= 0)
        {
            bumpTimeCounterPlayer = bumpTimerPlayer;
            sliding += touchedSlideSpeed*t.right.x;
      
            isSliding = true;
        }
        if (isBumpedRPlayer && bumpTimeCounterPlayer <= 0)
        {
            bumpTimeCounterPlayer = bumpTimerPlayer;
            sliding += -touchedSlideSpeed * t.right.x;
            isSliding = true;
        }
    }


    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isSpring = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsSpring);
        isFloating = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsWater);
        isBumpedL = Physics2D.OverlapCircle(BumpCheckL.position, bumpCheckRadius, whatIsGround);
        isBumpedR = Physics2D.OverlapCircle(BumpCheckR.position, bumpCheckRadius, whatIsGround);
        isBumpedLPlayer = Physics2D.OverlapCircle(BumpCheckL.position, bumpCheckRadius, whatIsPlayer);
        isBumpedRPlayer = Physics2D.OverlapCircle(BumpCheckR.position, bumpCheckRadius, whatIsPlayer);
    }


    private void inputSlideDash()
    {
        if (isSliding)
        {
            //Debug.Log(sliding);
            rb.velocity = new Vector2(sliding, rb.velocity.y);
            if (sliding > 0 && isGrounded) sliding -= 1;
            if (sliding < 0 && isGrounded) sliding += 1;
        }

    }

    public void setFacing(float face)
    {
        if (face > 0) facing = 1;
        else facing = -1;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
 
        Gizmos.DrawWireSphere(BodyCheck.position, bodyCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckL.position, bumpCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckR.position, bumpCheckRadius);
    }
}
