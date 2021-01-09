using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrEnviroment : GeneralEnviroment
{

    public override void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        overlayInGameObj = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsInGameObj);
        if (overlayInGameObj) isGrounded = true;
        isUnderBlock = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsGround);
        isNearLadder = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsLadder);
        if(!isNearLadder)isNearLadder = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsLadder);
        isSpring = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsSpring);
        isFloating = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsWater);
        isBumpedL = Physics2D.OverlapCircle(BumpCheckL.position, bumpCheckRadius, whatIsGround);
        if(!isBumpedL) isBumpedL = Physics2D.OverlapCircle(BumpCheckL.position, bumpCheckRadius, whatIsSpring);
        isBumpedR = Physics2D.OverlapCircle(BumpCheckR.position, bumpCheckRadius, whatIsGround);
        if (!isBumpedR) isBumpedR = Physics2D.OverlapCircle(BumpCheckR.position, bumpCheckRadius, whatIsSpring);
        isOnSwitchHouse = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsSwitchHouse);
        isOnShellJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsShellBox);

        isOnShell = Physics2D.OverlapCircle(SkillObject.position, grabRadius, whatIsPlayer);
    }

     protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(HeadCheck.position, headCheckRadius);
        Gizmos.DrawWireSphere(BodyCheck.position, bodyCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckL.position, bumpCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckR.position, bumpCheckRadius);
        Gizmos.DrawWireSphere(FirePoint.position, bumpCheckRadius);
    }
}
