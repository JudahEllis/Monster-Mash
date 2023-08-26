using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackObject : MonoBehaviour
{
    public int objectType;
    public string objectClassName;
    public bool isRightSided;
    public bool isLeftSided;
    public Animator myAnimator;
    public Animator connectedBodyPart_Animator;

    public void activateAttack(string animationName)
    {
        if (connectedBodyPart_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || connectedBodyPart_Animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            myAnimator.SetTrigger(animationName);
        }
    }

    public void triggerUpperAttackAnticipation()
    {
        if (isRightSided)
        {
            connectedBodyPart_Animator.SetTrigger("Right Upper Attack - Anticipate");
        }
        else if (isLeftSided)
        {
            connectedBodyPart_Animator.SetTrigger("Left Upper Attack - Anticipate");
        }
    }

    public void triggerUpperAttackRelease()
    {
        connectedBodyPart_Animator.SetBool("Ready to Swing", true);
    }

    public void triggerAttackToIdle()
    {
        connectedBodyPart_Animator.SetBool("Attack to Idle", true);
        connectedBodyPart_Animator.SetBool("Ready to Swing", false);
    }
}
