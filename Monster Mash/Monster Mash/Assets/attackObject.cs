using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackObject : MonoBehaviour
{
    public Animator mainMonsterBody;
    public int objectType;
    public string objectClassName;
    public bool isRightSidedLimb;
    public bool isLeftSidedLimb;
    public bool isUpperLimb;
    public bool isLowerLimb;
    public bool isGroundedLimb;
    public bool isPrimaryLeg;
    public bool isAttacking = false;
    public Animator myAnimator;
    public Animator connectedBodyPart_Animator;

    public void activateAttack(string animationName)
    {
        if (connectedBodyPart_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || connectedBodyPart_Animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
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
                connectedBodyPart_Animator.SetTrigger("Right Upper Attack - Anticipate");
            }
            else if (isLeftSidedLimb)
            {
                connectedBodyPart_Animator.SetTrigger("Left Upper Attack - Anticipate");
            }
        }
        else if (isLowerLimb)
        {
            if (isRightSidedLimb)
            {
                connectedBodyPart_Animator.SetTrigger("Right Lower Attack - Anticipate");
            }
            else if (isLeftSidedLimb)
            {
                connectedBodyPart_Animator.SetTrigger("Left Lower Attack - Anticipate");
            }
        }
        else
        {
            if (isRightSidedLimb)
            {
                connectedBodyPart_Animator.SetTrigger("Right Attack - Anticipate");
            }
            else if (isLeftSidedLimb)
            {
                connectedBodyPart_Animator.SetTrigger("Left Attack - Anticipate");
            }
        }
    }

    public void triggerAttackRelease()
    {
        connectedBodyPart_Animator.SetBool("Ready to Swing", true);
    }

    public void triggerLeftAttackStance()
    {
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (isGroundedLimb && isRightSidedLimb)
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
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (isGroundedLimb && isLeftSidedLimb)
            {
                myAnimator.SetTrigger("Backward Brace");
            }
            else if (isGroundedLimb && isRightSidedLimb && isAttacking == false)
            {
                myAnimator.SetTrigger("Forward Brace");
            }
        }
    }

    public void triggerAttackToIdle()
    {
        connectedBodyPart_Animator.SetBool("Attack to Idle", true);
        connectedBodyPart_Animator.SetBool("Ready to Swing", false);
        isAttacking = false;
    }

    public void triggerStretchJump()
    {
        myAnimator.SetBool("Primary Leg", true);
    }

    public void triggerWalk()
    {
        if (isGroundedLimb)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                myAnimator.SetBool("Walking", true);
                myAnimator.SetTrigger("Walk");
                //trigger torso to sway hips
            }
        }
    }

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

    public void triggerAnimationOffsets()
    {
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
}
