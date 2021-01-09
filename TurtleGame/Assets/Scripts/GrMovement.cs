using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrMovement : GeneralMovement
{
    protected float slowingSwim = 1.5f;

    protected float mineThrowSpeed = 20f;
    protected float mineSpeed = 40f;

    protected float mineJumpTime = 2f;
    [SerializeField]
    protected float mineJumpTimeCounter = 0f;

    protected float mineThrowBuffer = 0.2f;
    [SerializeField]
    protected float mineThrowBufferCounter = 0f;

    public float mineThrowCooldown = 3f;
    public float mineThrowBCooldownCounter = 0f;


    public int skillAmmoMax = 5;
    public int skillAmmoCount = 5;

    [SerializeField]
    protected float xDist;
    [SerializeField]
    protected float yDist;
    [SerializeField]
    protected float mineXDist;

    [SerializeField]
    protected bool isExploding = false;

    [SerializeField]
    protected GameObject MinesParentObject;
    [SerializeField]
    protected GameObject MinesObject;
    [SerializeField]
    protected Rigidbody2D rbMines;
    [SerializeField]
    protected Animator mineAn;

    protected override void Start()
    {
        envi = GetComponent<GrEnviroment>();
        anScript = GetComponent<GrAnimation>();
        //value
        jumpLimit = 2;
        slowingSwim = 1.5f;
        slowingAir = 0.75f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!manager.getIsDialogueOn())
        {
            envi.CheckSurroundings();
            CheckInput();
            EnviromentalEffects();
            specificChecks();
            Move();
            specificMovements();
        }
    }

    void specificChecks()
    {
        
    }

    void specificMovements()
    {
        mines();
    }


    
    protected override void FloatingWaterEffect()
    {
        if (envi.isFloating)
        {
            anScript.AnimationSet("Swimming");
            if (currentFloat < (floatVelocity * 5))
            {
                currentFloat += floatVelocity;
            }
            if (swimAnimationStop)
            {
                swimAnimationStop = false;
            }
            rb.velocity = new Vector2(rb.velocity.x, currentFloat);
        }
        else if (!envi.isFloating)
        {
            currentFloat = 0;
            
        }
        if (!swimAnimationStop)
        {
            tMoveSpeed = tMoveSpeed / slowingSwim;

        }
    }

    public override void RefreshOrb()
    {
        skillAmmoCount = skillAmmoMax;
    }

    void mines()
    {
        if(!MinesObject.activeSelf && Input.GetButtonDown("Skill 1") && mineThrowBufferCounter<=0 && skillAmmoCount>0 && mineThrowBCooldownCounter<=0)
        {
            MinesParentObject.transform.position = envi.FirePoint.position;
            float faceVar = 1;
            if (!facingR) faceVar = -1;
            rbMines.velocity = new Vector2(faceVar * mineThrowSpeed,0f);
            MinesObject.SetActive(true);
            rbMines.simulated = true;
            mineThrowBufferCounter = mineThrowBuffer;
            skillAmmoCount--;
        }
        else if (Input.GetButtonDown("Skill 1") && mineThrowBufferCounter <= 0 && MinesObject.activeSelf)
        {
            if (envi.isOnMines)
            {
                xDist = transform.position.x-envi.SkillObject.position.x;

                yDist = transform.position.y-envi.SkillObject.position.y+0.5f;//0.5f untuk padding center dari mine terlalu tinggi

                xDist = xDist / (envi.explosionRadius*1.2f);
                yDist = yDist / (envi.explosionRadius*2);


                //if (xDist < 0) xDist = -1 - xDist;
                //else xDist = 1 - xDist;
                if (yDist < 0) yDist = 1 + yDist;
                else yDist = 1 - yDist;

                //if (xDist > mineXDist || xDist < -mineXDist) xDist = 0;
                isExploding = true;
                mineJumpTimeCounter = mineJumpTime;
                mineThrowBCooldownCounter = mineThrowCooldown;

            }
            mineAn.SetTrigger("isExploding");
            rbMines.simulated = false;
            MinesObject.SetActive(false);
            mineThrowBufferCounter = mineThrowBuffer;
        }
        if (mineJumpTimeCounter > 0) mineJumpTimeCounter -= Time.deltaTime;
        if (mineJumpTimeCounter <= 0) isExploding = false;
        
        if (isExploding)
        {
            float XSpeed = rb.velocity.x + (xDist * mineSpeed);
            float YSpeed = rb.velocity.y + (yDist * mineSpeed);

            if (XSpeed > mineSpeed) XSpeed = mineSpeed;
            else if (XSpeed < -mineSpeed) XSpeed = -mineSpeed;
            if (YSpeed > mineSpeed) YSpeed = mineSpeed;
            else if (YSpeed < -mineSpeed) YSpeed = -mineSpeed;

            rb.velocity = new Vector2(XSpeed,YSpeed);
            //Debug.Log("Vel: "+rb.velocity);
        }
        if (mineThrowBufferCounter > 0) mineThrowBufferCounter -= Time.deltaTime;
        if (mineThrowBCooldownCounter > 0) mineThrowBCooldownCounter -= Time.deltaTime;
    }
    
    
}
