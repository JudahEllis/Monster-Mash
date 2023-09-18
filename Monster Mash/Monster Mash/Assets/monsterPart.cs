using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPart : MonoBehaviour
{
    public monsterAttackSystem myMainSystem;
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
    public bool isLeadingLeg;
    private bool isAttacking = false;
    private bool isRunning = false;
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

        if (isLeg || isArm) //this will be expanded to include arms, tails, wings and heads
        {
            myAnimator.SetInteger("Attack Animation ID", attackAnimationID);
        }

        if (isLeadingLeg)
        {
            myAnimator.SetBool("Is Leading Leg", true);
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
                myAnimator.SetFloat("Run Offset", 0.0f);
            }
            else if (isLeftSidedLimb)
            {
                float leftWalkOffset = myAnimator.GetFloat("Left Walk Offset");
                float leftRunOffset = myAnimator.GetFloat("Left Run Offset");
                myAnimator.SetFloat("Idle Offset", 0.5f);
                myAnimator.SetFloat("Walk Offset", leftWalkOffset);
                myAnimator.SetFloat("Run Offset", leftRunOffset);
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
            connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
            connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            isAttacking = true;
            myAnimator.SetTrigger(animationName);
        }
        else if (connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Run") || connectedMonsterPart.GetBool("Running") == true)
        {
            if (attackAnimationID == -1 && isRunning == true && isGroundedLimb == true)
            {
                return;
            }
            else
            {
                isAttacking = true;
                myAnimator.SetTrigger(animationName);
                myMainSystem.attackFocusOn();
            }
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
            if (attackAnimationID == -1) //backwards attack coming from the leg area
            {
                if (isRightSidedLimb)
                {
                    connectedMonsterPart.SetTrigger("Left Upper Attack - Anticipate");//because its a backwards attack from the leg area it twists exact opposite
                }
                else if (isLeftSidedLimb)
                {
                    connectedMonsterPart.SetTrigger("Right Upper Attack - Anticipate");
                }
            }
            else
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

        if (isGroundedLimb)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || 
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Backward Brace");
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Forward Brace");
                }

                myAnimator.SetBool("Walking", false);
            }
        }
        else if (isArm)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || 
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
    }

    public void triggerRightAttackStance()
    {
        if (isGroundedLimb)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || 
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Backward Brace");
                }
                else if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Forward Brace");
                }

                myAnimator.SetBool("Walking", false);
            }
        }
        else if (isArm)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || 
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
                else if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
    }

    public void triggerAttackRelease()
    {
        connectedMonsterPart.SetBool("Ready to Swing", true);
        connectedMonsterPart.SetBool("Walking", false);
        connectedMonsterPart.SetBool("Running", false);
        isRunning = false;
        myMainSystem.correctWalkingAttackAnimations();
    }

    public void walkToAttackCorrections()
    {
        if (isTorso)
        {
            if (myAnimator.GetBool("Walking") == true)
            {
                myAnimator.SetBool("Walking", false);
                myAnimator.SetTrigger("Walk to Idle");
            }
        }

        if (isGroundedLimb)
        {
            myAnimator.SetBool("Walking", false);
        }

        if (isHead)
        {
            myAnimator.SetBool("Walking", false);
        }
    }

    public void triggerRunAttackCorrections()
    {
        myMainSystem.correctRunningAttackAnimations();
        myMainSystem.attackFocusOff();
    }

    public void runToAttackCorrections()
    {
        if (isTorso)
        {
            if (myAnimator.GetBool("Running") == true)
            {
                myAnimator.SetBool("Running", false);
                myAnimator.SetTrigger("Run to Idle");
            }
        }

        if (isGroundedLimb)
        {
            myAnimator.SetBool("Running", false);
        }

        if (isArm || isHead)
        {
            myAnimator.SetBool("Running", false);
        }
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
        if (isGroundedLimb || isTorso)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                myAnimator.SetBool("Walking", true);
            }
        }
        else if (isHead)
        {
            myAnimator.SetBool("Walking", true);
        }
    }

    public void triggerStopWalking()
    {
        if (isGroundedLimb || isTorso)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                myAnimator.SetBool("Walking", false);
                myAnimator.SetTrigger("Walk to Idle");

            }
        }
        else if (isHead)
        {
            myAnimator.SetBool("Walking", false);
        }
    }

    public void triggerRun()
    {
        if (isGroundedLimb || isTorso)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                myAnimator.SetBool("Running", true);
                myAnimator.SetBool("Walking", false);
                isRunning = true;
            }
        }

        if (isArm)
        {
            myAnimator.SetBool("Running", true);
            isRunning = true;
        }

        if (isHead)
        {
            myAnimator.SetBool("Running", true);
            myAnimator.SetBool("Walking", false);
            isRunning = true;
        }
    }

    public void triggerScreechingStop()
    {
        if (isGroundedLimb || isTorso)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                myAnimator.SetBool("Running", false);
                myAnimator.SetTrigger("Run to Screech");
                isRunning = false;

            }
        }

        if (isArm || isHead)
        {
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }
    }

    public void triggerJump()
    {
        myAnimator.SetBool("Grounded", false);
        myAnimator.SetTrigger("Jump");

        if (isGroundedLimb || isTorso || isHead)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }

        if (isArm)
        {
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }
    }

    //This fall function is saved for when the player is knocked off an edge or walks over an edge (not a jump related fall)
    public void triggerFall()
    {
        myAnimator.SetBool("Grounded", false);
        myAnimator.SetTrigger("Fall");

    }

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

        if (isGroundedLimb || isTorso || isHead)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }

        if (isArm)
        {
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }
    }

    #endregion

}
