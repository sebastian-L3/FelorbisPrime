using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEnviroment : MonoBehaviour
{
    //float
    public float minGroundNormalY = 0.65f;
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
    public bool isOnSwitchHouse;
    public bool isOnShellJump;

    public bool isOnMines;
    public bool isOnShell;
    
    //checkers
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform HeadCheck;
    [SerializeField] protected Transform BodyCheck;
    [SerializeField] protected Transform BumpCheckL;
    [SerializeField] protected Transform BumpCheckR;
    [SerializeField] public Transform FirePoint;
    [SerializeField] public Transform SkillObject;

    //Checker size
    [SerializeField] protected float groundCheckRadius = 1.1f;
    protected float headCheckRadius = 2f;
    protected float bodyCheckRadius = 1.5f;
    protected float bumpCheckRadius = 0.5f;

    [SerializeField] public float explosionRadius = 5.5f;
    [SerializeField] public float grabRadius = 5.5f;

    //mask
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsLadder;
    [SerializeField] protected LayerMask whatIsSpring;
    [SerializeField] protected LayerMask whatIsWater;
    [SerializeField] protected LayerMask whatIsInGameObj;
    [SerializeField] protected LayerMask whatIsSwitchHouse;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsShellBox;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called once per physics frame
    void FixedUpdate()
    {

    }

    public virtual void CheckSurroundings()
    {
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(HeadCheck.position, headCheckRadius);
        Gizmos.DrawWireSphere(BodyCheck.position, bodyCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckL.position, bumpCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckR.position, bumpCheckRadius);
    }
}
