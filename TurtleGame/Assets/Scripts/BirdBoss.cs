using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBoss : MonoBehaviour
{
    public float SpeedX = 40f;
    public float SpeedY = 20f;
    public float ThrowSpeedX = 60f;
    public float ThrowSpeedYMultiplier = 2f;
    public Rigidbody2D rb;
    public GameObject Self;
    public GameObject followR;
    public GameObject followL;
    public GameObject Home;
    public GameObject stoneObject;
    public Rigidbody2D rbStoneObject;
    public GameObject player;
    public float moveX;
    public float moveY;
    public bool reachedR = false;
    public bool reachedL = true;
    public bool reachedHome = true;

    public bool facingR = true;

    public float throwTimer = 5f;
    public float throwTimerCounter = 0f;

    public float projectileTime = 2.5f;
    public float ProjectileTimeCounter = 0f;
    public Vector2 throwingVelocity = new Vector2(0, 0);

    public LayerMask whatIsGround;
    public GameObject GroundCheck;
    public bool StoneGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StoneGrounded = Physics2D.OverlapCircle(GroundCheck.transform.position, 2f, whatIsGround);
        if (!reachedR)
        {
            if (followR.transform.position.x > transform.position.x)
            {
                moveX = SpeedX;
            }
            else
            {
                moveX = -SpeedX;
            }
            if (followR.transform.position.y > transform.position.y)
            {
                moveY = SpeedY;
            }
            else
            {
                moveY = -SpeedY;
            }
            if (transform.position.x >= followR.transform.position.x)
            {
                reachedR = true;
                reachedL = false;
            }
        }
        else if (!reachedL)
        {
            if (followL.transform.position.x > transform.position.x)
            {
                moveX = SpeedX;
            }
            else
            {
                moveX = -SpeedX;
            }
            if (followL.transform.position.y > transform.position.y)
            {
                moveY = SpeedY;
            }
            else
            {
                moveY = -SpeedY;
            }
            if (transform.position.x <= followL.transform.position.x)
            {
                reachedL = true;
                reachedR = false;
            }
        }
        //home
        if (!reachedHome)
        {
            if (Home.transform.position.x > transform.position.x)
            {
                moveX = SpeedX;
            }
            else
            {
                moveX = -SpeedX;
            }
            if (Home.transform.position.y > transform.position.y)
            {
                moveY = SpeedY;
            }
            else
            {
                moveY = -SpeedY;
            }
            if (transform.position.x >= Home.transform.position.x && transform.position.y >= Home.transform.position.y)
            {
                reachedHome = true;
                Self.SetActive(false);
            }
        }
        rb.velocity = new Vector2(moveX, moveY);
        if (moveX > 0 && !facingR)
        {
            Flip();
        }
        else if (moveX < 0 && facingR)
        {
            Flip();
        }

        // throw
        if (projectileTime > 0 && StoneGrounded) projectileTime = 0;
        if (throwTimerCounter < throwTimer)
        {
            throwTimerCounter += Time.deltaTime;
        }
        else if (throwTimerCounter >= throwTimer && reachedHome)
        {
            throwTimerCounter = 0;
            throwStone();
        }

        if (ProjectileTimeCounter > 0)
        {
            ProjectileTimeCounter -= Time.deltaTime;
            rbStoneObject.velocity = new Vector2(throwingVelocity.x * (ProjectileTimeCounter / projectileTime), rbStoneObject.velocity.y * ThrowSpeedYMultiplier);
        }
    }

    void Flip()
    {
        facingR = !facingR;
        transform.Rotate(0f, 180f, 0f);
    }

    void throwStone()
    {
        if (player.transform.position.x > transform.position.x)
        {
            throwingVelocity = new Vector2(ThrowSpeedX * (player.transform.position.x - transform.position.x) / 50f, rbStoneObject.velocity.y);
        }
        else
        {
            throwingVelocity = new Vector2(ThrowSpeedX * (player.transform.position.x - transform.position.x) / 50f, rbStoneObject.velocity.y);
        }
        ProjectileTimeCounter = projectileTime;
        stoneObject.transform.position = transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.transform.position, 2f);
    }
}
