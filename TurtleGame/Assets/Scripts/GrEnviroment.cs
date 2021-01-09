using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrEnviroment : GeneralEnviroment
{

    public override void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        overlayInGameObj = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsInGameObj);
        if (overlayInGameObj) isGrounded = true;
        isUnderBlock = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsGround);
        isNearLadder = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsLadder);
        if (!isNearLadder) isNearLadder = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsLadder);
        isSpring = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsSpring);
        isFloating = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsWater);
        isBumpedL = Physics2D.OverlapCircle(BumpCheckL.position, bumpCheckRadius, whatIsGround);
        isBumpedR = Physics2D.OverlapCircle(BumpCheckR.position, bumpCheckRadius, whatIsGround);
        isOnSwitchHouse = Physics2D.OverlapCircle(BodyCheck.position, bodyCheckRadius, whatIsSwitchHouse);
        isOnShellJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsShellBox);

        isOnMines = Physics2D.OverlapCircle(SkillObject.position, explosionRadius, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(HeadCheck.position, headCheckRadius);
        Gizmos.DrawWireSphere(BodyCheck.position, bodyCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckL.position, bumpCheckRadius);
        Gizmos.DrawWireSphere(BumpCheckR.position, bumpCheckRadius);
        Gizmos.DrawWireSphere(SkillObject.position, explosionRadius);
        Gizmos.DrawWireSphere(FirePoint.position, bumpCheckRadius);
    }
}
