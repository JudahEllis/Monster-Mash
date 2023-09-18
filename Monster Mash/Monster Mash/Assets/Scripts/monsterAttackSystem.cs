using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterAttackSystem : MonoBehaviour
{
    public bool facingRight = false;
    public bool isGrounded = true;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool focusedAttackActive = false;

    private Animator myAnimator;
    public monsterPart[] attackSlotMonsterParts = new monsterPart[8];
    private int[] attackSlotMonsterID = new int[8];
    public List<monsterPart> allMonsterParts;

    public void awakenTheBeast()
    {
        myAnimator = this.GetComponent<Animator>();
        grabAttackSlotInfo();

        bool hasLeftGroundedLegs = false;
        bool hasRightGroundedLegs = false;
        List<monsterPart> allGroundedRightLegs = new List<monsterPart>();
        List<monsterPart> allGroundedLeftLegs = new List<monsterPart>();

        #region Figuring out if this Monster has one leg, many legs, or is a legless guy

        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            if (allMonsterParts[i].isGroundedLimb)
            {
                if (allMonsterParts[i].isRightSidedLimb)
                {
                    allGroundedRightLegs.Add(allMonsterParts[i]);
                    hasRightGroundedLegs = true;
                }
                else if (allMonsterParts[i].isLeftSidedLimb)
                {
                    allGroundedLeftLegs.Add(allMonsterParts[i]);
                    hasLeftGroundedLegs = true;
                }
            }
        }

        if (hasRightGroundedLegs == true && hasLeftGroundedLegs == true)
        {
            for (int u = 0; u < allGroundedRightLegs.Count; u++)
            {
                allGroundedRightLegs[u].isLeadingLeg = true;
            }
        }
        else
        {
            if (hasRightGroundedLegs == true) //high possibility that this is a one legged creature
            {
                for (int u = 0; u < allGroundedRightLegs.Count; u++)
                {
                    allGroundedRightLegs[u].isLeadingLeg = true;
                }
            }
            else if (hasLeftGroundedLegs == true) //high possibility that this is a one legged creature
            {
                for (int u = 0; u < allGroundedLeftLegs.Count; u++)
                {
                    allGroundedLeftLegs[u].isLeadingLeg = true;
                }
            }
            else //oh this creature has no grounded legs, high likelihood of it being a floating/flying monster
            {

            }
        }

        #endregion

        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].myMainSystem = this;
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
        if (attackSlotMonsterParts[attackSlot] != null && focusedAttackActive == false)
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
                    //isWalking = false;

                    #region Bracing for Attacks
                    if (attackSlotMonsterParts[attackSlot].isRightSidedLimb)
                    {
                        if (attackSlotMonsterParts[attackSlot].attackAnimationID == -1)
                        {
                            if (isRunning == false)
                            {
                                braceForLeftImpact();
                            }
                            else if (isRunning == true && attackSlotMonsterParts[attackSlot].isGroundedLimb == false)
                            {
                                braceForLeftImpact();
                                isRunning = false;
                            }
                        }
                        else
                        {
                            braceForRightImpact();
                            isRunning = false;
                        }
                    }
                    else if (attackSlotMonsterParts[attackSlot].isLeftSidedLimb)
                    {

                        if (attackSlotMonsterParts[attackSlot].attackAnimationID == -1)
                        {

                            if (isRunning == false)
                            {
                                braceForRightImpact();
                            }
                            else if (isRunning == true && attackSlotMonsterParts[attackSlot].isGroundedLimb == false)
                            {
                                braceForRightImpact();
                                isRunning = false;
                            }
                        }
                        else
                        {
                            braceForLeftImpact();
                            isRunning = false;
                        }
                    }
                    #endregion
                }
                else
                {
                    attackSlotMonsterParts[attackSlot].triggerAttack("Airborn Attack");
                }
            }
            else if (attackSlotMonsterID[attackSlot] == 2)
            {
                //scientific module
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
            //isWalking = true;

            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerWalk();
            }
        }
    }

    public void stopWalking()
    {
        if (isGrounded)
        {
            //isWalking = false;

            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerStopWalking();
            }
        }
    }

    public void run()
    {
        if (isGrounded)
        {
            isRunning = true;

            for (int i = 0; i < allMonsterParts.Count; i++)
            {
                allMonsterParts[i].triggerRun();
            }
        }
    }

    public void screechingStop()
    {
        if (isGrounded)
        {
            isRunning = false;

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
            isRunning = false;
            focusedAttackActive = false;

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
        isRunning = false;
        focusedAttackActive = false;

        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].triggerHit();
        }

        //The hit animations are going to flip torso back and forth from this animator (so that flipping directions doesnt affect back and forth hit animations)
        //It does need to know which way to start though so that it is facing the camera
    }

    public void correctWalkingAttackAnimations()
    {
        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].walkToAttackCorrections();
        }
    }

    public void correctRunningAttackAnimations()
    {
        for (int i = 0; i < allMonsterParts.Count; i++)
        {
            allMonsterParts[i].runToAttackCorrections();
        }
    }

    public void attackFocusOn()
    {
        focusedAttackActive = true;
    }

    public void attackFocusOff()
    {
        focusedAttackActive = false;
    }

    #endregion
}