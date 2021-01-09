using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrAnimation : GeneralAnimation
{
    private Animator an;
    // Start is called before the first frame update
    private bool crouching=false;
    private bool climbing = false;
    void Start()
    {
        an = GetComponent<Animator>();
    }



    public override void AnimationSet(string Input)
    {
        switch (Input)
        {
            case "WalkTrue":
                an.SetBool("isWalking", true);
                break;
            case "WalkFalse":
                an.SetBool("isWalking", false);
                break;
            case "isMovingVF":
                an.SetBool("IsMovingVertical", false);
                break;
            case "isMovingVT":
                an.SetBool("IsMovingVertical", true);
                break;
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
            case "Climb":
                if (!climbing)
                {
                    an.ResetTrigger("FinishedClimbing");
                    an.SetTrigger("IsClimbing");
                    climbing = true;
                }
                break;
            case "ClimbCancel":
                an.ResetTrigger("IsClimbing");
                an.SetTrigger("FinishedClimbing");
                climbing = false;
                break;
            case "Crouch":
                if (!crouching)
                {
                    an.ResetTrigger("FinishedCrouching");
                    an.SetTrigger("IsCrouching");
                    crouching = true;
                }
                break;
            case "CrouchCancel":
                an.ResetTrigger("IsCrouching");
                an.SetTrigger("FinishedCrouching");
                crouching = false;
                break;
            case "Sliding":
                an.ResetTrigger("FinishedSliding");
                an.SetTrigger("IsSliding");
                break;
            case "SlidingCancel":
                an.SetTrigger("FinishedSliding");
                an.ResetTrigger("IsSliding");
                break;
            case "Swimming":
                if (!an.GetCurrentAnimatorStateInfo(0).IsName("AN_G-swim") && !an.GetCurrentAnimatorStateInfo(0).IsName("AN_G-SwimMove"))
                {
                    an.ResetTrigger("FinishedSwimming");
                    an.SetTrigger("IsSwimming");
                    an.ResetTrigger("IsLanded");
                }
                break;
            case "Landed":
                an.SetTrigger("IsLanded");
                an.SetTrigger("FinishedSwimming");
                an.ResetTrigger("IsSwimming");
                break;
        }
    }
}
