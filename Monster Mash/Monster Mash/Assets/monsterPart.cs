using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPart : MonoBehaviour
{
    public Animator connectedMonsterPart;
    private Animator myAnimator;
    public int monsterPartHealth = 100;

    //monsterPartID - jumper parts = 0, organic parts = 1, and scientific parts = 2
    public int monsterPartID;
    public int attackAnimationID;
    public bool connected = true;
    public bool isArm;
    public bool isLeg;
    public bool isTail;
    public bool isWing;
    public bool isHead;
    public bool isEye;
    public bool isMouth;
    public bool isTorso;
    public bool isUpperLimb;
    public bool isLowerLimb;
    public bool isRightSidedLimb;
    public bool isLeftSidedLimb;
    public bool isGroundedLimb;
    private bool isAttacking = false;
    private int jumpsAllotted;
    private int regularJumpAmount = 2;
    private int wingedJumpAmount = 4;


    #region Animation Set Up

    public void triggerAnimationSetUp()
    {
        myAnimator = this.GetComponent<Animator>();

        if(isLeg && isGroundedLimb == false)
        {
            myAnimator.SetBool("Grounded Limb", false); //This is just so that loose legs do a spike kick instead of a stomp on ground
        }

        if (isLeg) //this will be expanded to include arms, tails, and heads
        {
            myAnimator.SetInteger("Attack Animation ID", attackAnimationID);
        }
    }

    public void triggerAnimationOffsets()
    {
        if (monsterPartID != 1 || connected == false)
        {
            return;
        }

        if (isGroundedLimb)
        {
            if (isRightSidedLimb)
            {
                myAnimator.SetFloat("Idle Offset", 0.0f);
                myAnimator.SetFloat("Walk Offset", 0.0f);
            }
            else if (isLeftSidedLimb)
            {
                myAnimator.SetFloat("Idle Offset", 0.5f);
                myAnimator.SetFloat("Walk Offset", 0.5f);
            }
        }
        else if ((isRightSidedLimb || isLeftSidedLimb))
        {
            float randomOffset = Random.Range(0, 0.5f);
            myAnimator.SetFloat("Idle Offset", randomOffset);
        }
    }

    public void triggerIdle()
    {
        if (monsterPartID != 1 || connected == false)
        {
            return;
        }

        myAnimator.SetTrigger("Idle");
    }
    #endregion

    #region Attack Animations
    public void triggerAttack(string animationName)
    {
        if (connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
            connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            isAttacking = true;
            myAnimator.SetTrigger(animationName);
        }
    }

    public void triggerAttackAnticipation()
    {

        if (isUpperLimb)
        {
            if (isRightSidedLimb)
            {
                connectedMonsterPart.SetTrigger("Right Upper Attack - Anticipate");
            }
            else if (isLeftSidedLimb)
            {
                connectedMonsterPart.SetTrigger("Left Upper Attack - Anticipate");
            }
        }
        else if (isLowerLimb)
        {
            if (isRightSidedLimb)
            {
                connectedMonsterPart.SetTrigger("Right Lower Attack - Anticipate");
            }
            else if (isLeftSidedLimb)
            {
                connectedMonsterPart.SetTrigger("Left Lower Attack - Anticipate");
            }
        }
        else
        {
            if (isRightSidedLimb)
            {
                connectedMonsterPart.SetTrigger("Right Attack - Anticipate");
            }
            else if (isLeftSidedLimb)
            {
                connectedMonsterPart.SetTrigger("Left Attack - Anticipate");
            }
        }
    }

    public void triggerLeftAttackStance()
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            if (isGroundedLimb && isRightSidedLimb && isAttacking == false)
            {
                myAnimator.SetTrigger("Backward Brace");
            }
            else if (isGroundedLimb && isLeftSidedLimb && isAttacking == false)
            {
                myAnimator.SetTrigger("Forward Brace");
            }
        }
    }

    public void triggerRightAttackStance()
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            if (isGroundedLimb && isLeftSidedLimb && isAttacking == false)
            {
                myAnimator.SetTrigger("Backward Brace");
            }
            else if (isGroundedLimb && isRightSidedLimb && isAttacking == false)
            {
                myAnimator.SetTrigger("Forward Brace");
            }
        }
    }

    public void triggerAttackRelease()
    {
        connectedMonsterPart.SetBool("Ready to Swing", true);
    }

    public void triggerAttackToIdle()
    {
        connectedMonsterPart.SetBool("Attack to Idle", true);
        connectedMonsterPart.SetBool("Ready to Swing", false);
        isAttacking = false;
    }

    #endregion

    #region Movement Animations
    public void triggerWalk()
    {
        if (isGroundedLimb)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                myAnimator.SetBool("Walking", true);
                myAnimator.SetTrigger("Walk");
                //trigger torso to sway hips
            }
        }
    }
    //KNOWN ISSUE: Jumping during Walk Animation, Jump needs to handled locally here on new version so that we can set walking to false when jumping
    //this should be fixed now, needs testing

    public void triggerScreechingStop()
    {
        if (isGroundedLimb)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                myAnimator.SetBool("Walking", false);
                myAnimator.SetTrigger("Walk to Screech");
                //trigger torso to sway hips

            }
        }
    }

    public void triggerJump()
    {
        myAnimator.SetBool("Grounded", false);
        myAnimator.SetTrigger("Jump");

        if (isGroundedLimb)
        {
            myAnimator.SetBool("Walking", false);
        }
    }

    //This fall function is saved for when the player is knocked off an edge or walks over an edge (not a jump related fall)
    public void triggerFall()
    {
        myAnimator.SetBool("Grounded", false);
        myAnimator.SetTrigger("Fall");

        //TESTING PURPOSES - Basically faking a fall and land
        //StartCoroutine(fakeJump());
    }

    /*
    IEnumerator fakeJump()
    {
        yield return new WaitForSeconds(3);
        triggerLand();
    }

    */

    public void triggerLand()
    {
        myAnimator.SetBool("Grounded", true);
        myAnimator.SetTrigger("Land");
    }
    #endregion

    #region Reaction Animations

    public void triggerHit()
    {
        myAnimator.SetTrigger("Hit");
        isAttacking = false;
    }

    #endregion

}
