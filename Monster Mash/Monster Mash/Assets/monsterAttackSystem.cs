using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterAttackSystem : MonoBehaviour
{
    public bool facingRight = false;
    public bool isGrounded = true;

    private Animator myAnimator;
    public monsterPart[] attackSlotMonsterParts = new monsterPart[8];
    private int[] attackSlotMonsterID = new int[8];
    public List<monsterPart> allMonsterParts;

    public void awakenTheBeast()
    {
        myAnimator = this.GetComponent<Animator>();
        grabAttackSlotInfo();

        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].triggerAnimationSetUp();
            allMonsterParts[i].triggerAnimationOffsets();
            allMonsterParts[i].triggerIdle();
        }
    }

    public void grabAttackSlotInfo()
    {
        for (int i = 0; i < attackSlotMonsterParts.Length; i++)
        {
            attackSlotMonsterID[i] = attackSlotMonsterParts[i].monsterPartID; //this tells us what type of part class it is
        }
    }

    #region Attacks
    public void attack(int attackSlot)
    {
        if (attackSlotMonsterParts[attackSlot] != null)
        {
            if (attackSlotMonsterID[attackSlot] == 0)
            {
                jump();
            }
            else if (attackSlotMonsterID[attackSlot] == 1)
            {
                if (isGrounded)
                {
                    attackSlotMonsterParts[attackSlot].triggerAttack("Ground Attack");

                    if (attackSlotMonsterParts[attackSlot].isRightSidedLimb)
                    {
                        if (attackSlotMonsterParts[attackSlot].attackAnimationID == -1)
                        {
                            braceForLeftImpact();
                        }
                        else
                        {
                            braceForRightImpact();
                        }
                    }
                    else if (attackSlotMonsterParts[attackSlot].isLeftSidedLimb)
                    {

                        if (attackSlotMonsterParts[attackSlot].attackAnimationID == -1)
                        {
                            braceForRightImpact();
                        }
                        else
                        {
                            braceForLeftImpact();
                        }
                    }
                }
                else
                {
                    attackSlotMonsterParts[attackSlot].triggerAttack("Airborn Attack");
                }
            }
            else if (attackSlotMonsterID[attackSlot] == 2)
            {

            }
        }
    }

    public void braceForRightImpact()
    {
        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].triggerRightAttackStance();
        }
    }

    public void braceForLeftImpact()
    {
        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].triggerLeftAttackStance();
        }
    }
    #endregion

    #region Movement

    public void flipCharacter()
    {
        if (facingRight)
        {
            facingRight = false;
            myAnimator.SetTrigger("Flip to Left");
            //if grounded then make grounded limbs step pivot
        }
        else
        {
            facingRight = true;
            myAnimator.SetTrigger("Flip to Right");
            //if grounded then make grounded limbs step pivot
        }
    }

    public void walk()
    {
        if (isGrounded)
        {
            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerWalk();
            }
        }
    }

    //SOME FUNCTION HERE WITH A SIMPLE STOP

    public void screechingStopWalking()
    {
        if (isGrounded)
        {
            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerScreechingStop();
            }
        }
    }

    public void jump()
    {
        if (isGrounded)
        {
            isGrounded = false;

            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerJump();
            }

            myAnimator.SetTrigger("Jump");
        }
    }

    public void walkToFall()
    {
        if (isGrounded)
        {
            isGrounded = false;

            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerFall();
            }
        }
    }

    public void land()
    {
        if(isGrounded == false)
        {
            isGrounded = true;

            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerLand();
            }

            myAnimator.SetTrigger("Land");
        }
    }

    #endregion

    #region Reactions

    public void hit()
    {
        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].triggerHit();
        }

        //The hit animations are going to flip torso back and forth from this animator (so that flipping directions doesnt affect back and forth hit animations)
        //It does need to know which way to start though so that it is facing the camera
    }

    #endregion
}
