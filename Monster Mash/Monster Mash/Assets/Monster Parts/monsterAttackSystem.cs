using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterAttackSystem : MonoBehaviour
{
    public bool facingRight = false;
    public bool isGrounded = true;
    private bool isWinged = false;
    private int jumpsAllowed_DoubleJump = 2;
    private int jumpsAllowed_FlightedJump = 4;
    private int jumpsLeft = 0;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isGliding = false;
    private bool focusedAttackActive = false;
    private bool canRoll = true;
    private bool canDashAttack = true;

    private Animator myAnimator;
    private Animator mainTorso;
    public monsterPart[] attackSlotMonsterParts = new monsterPart[8];
    private int[] attackSlotMonsterID = new int[8];
    private List<monsterPartReference> listOfInternalReferences = new List<monsterPartReference>();
    private monsterPart[] allMonsterParts;
    public GameObject dashSplat;
    private Vector3 leftDashSplatRotation = new Vector3 (0, 210, 0);
    private Vector3 rightDashSplatRotation = new Vector3(0, 270, 0);

    public void awakenTheBeast()
    {
        myAnimator = this.GetComponent<Animator>();
        grabAttackSlotInfo();
        myAnimator.SetBool("Facing Right", facingRight);

        autoLimb_Connection[] limbConnections = GetComponentsInChildren<autoLimb_Connection>();

        for (int i = 0; i < limbConnections.Length; i++)
        {
            limbConnections[i].disableColliders(); //this essentially stops the scripts from looking to connect pieces at runtime
        }

        allMonsterParts = GetComponentsInChildren<monsterPart>();

        #region Figuring out if this Monster has one leg, many legs, or is a legless guy

        bool hasLeftGroundedLegs = false;
        bool hasRightGroundedLegs = false;
        bool hasWings = false;
        List<monsterPart> allGroundedRightLegs = new List<monsterPart>();
        List<monsterPart> allGroundedLeftLegs = new List<monsterPart>();
        List<monsterPart> allWings = new List<monsterPart>();

        for (int i = 0; i < allMonsterParts.Length; i++)
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

            if (allMonsterParts[i].isWing)
            {
                allWings.Add(allMonsterParts[i]);
                hasWings = true;
            }

            if (allMonsterParts[i].isTorso)
            {
                mainTorso = allMonsterParts[i].GetComponent<Animator>();
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
                if (hasWings)
                {
                    for (int u = 0; u < allWings.Count; u++)
                    {
                        allWings[u].hasFlightedIdle = true;
                    }
                }
            }
        }

        #endregion

        monsterPartReference[] internalPartReferences = GetComponentsInChildren<monsterPartReference>();

        for (int i = 0; i < internalPartReferences.Length; i++)
        {
            listOfInternalReferences.Add(internalPartReferences[i]);
        }

        /*
        //remove this, have collider to ignore info handled by individual monster parts
        for (int u = 0; u < listOfInternalReferences.Count; u++)
        {
            listOfInternalReferences[u].referencesToIgnore = listOfInternalReferences;
        }
        */

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].myMainSystem = this;
            allMonsterParts[i].mainTorso = mainTorso;
            allMonsterParts[i].referencesToIgnore = listOfInternalReferences;
            allMonsterParts[i].triggerCollisionLogic();
            allMonsterParts[i].triggerAnimationSetUp();
            allMonsterParts[i].triggerAnimationOffsets();
            allMonsterParts[i].triggerIdle();
        }

        myAnimator.SetBool("Idle Bounce Allowed", true);

    }

    public void grabAttackSlotInfo()
    {
        for (int i = 0; i < attackSlotMonsterParts.Length; i++)
        {
            attackSlotMonsterID[i] = attackSlotMonsterParts[i].monsterPartID; //this tells us what type of part class it is

            if (attackSlotMonsterParts[i].isWing == true)
            {
                isWinged = true;
            }
        }

        if (isWinged)
        {
            jumpsLeft = jumpsAllowed_FlightedJump;
        }
        else
        {
            jumpsLeft = jumpsAllowed_DoubleJump;
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
                    if (focusedAttackActive == false)
                    {
                        attackSlotMonsterParts[attackSlot].triggerAttack("Ground Attack");
                        //isWalking = false;

                        #region Bracing for Attacks

                        if (attackSlotMonsterParts[attackSlot].requiresRightStance)
                        {
                            braceForRightImpact();
                        }

                        if (attackSlotMonsterParts[attackSlot].requiresLeftStance)
                        {
                            braceForLeftImpact();
                        }

                        if (attackSlotMonsterParts[attackSlot].requiresForwardStance)
                        {
                            braceForForwardImpact();
                        }

                        if (attackSlotMonsterParts[attackSlot].requiresBackwardStance)
                        {
                            braceForBackwardImpact();
                        }

                        isRunning = false;
                        #endregion
                    }
                }
                else
                {
          
                    if (focusedAttackActive == false)
                    {
                        attackSlotMonsterParts[attackSlot].triggerAttack("Airborn Attack");

                        #region Bracing for Attacks

                        if (attackSlotMonsterParts[attackSlot].requiresRightStance)
                        {
                            braceForRightImpact();
                        }

                        if (attackSlotMonsterParts[attackSlot].requiresLeftStance)
                        {
                            braceForLeftImpact();
                        }

                        if (attackSlotMonsterParts[attackSlot].requiresForwardStance)
                        {
                            braceForForwardImpact();
                        }

                        if (attackSlotMonsterParts[attackSlot].requiresBackwardStance)
                        {
                            braceForBackwardImpact();
                        }

                        isRunning = false;
                        #endregion
                    }
                    //Add to this later so that we can having bracing while attacking in the air
                }
            }
            else if (attackSlotMonsterID[attackSlot] == 2)
            {
                //scientific module
            }
        }
    }

    public void attackCancel(int attackSlot)
    {
        if (attackSlotMonsterID[attackSlot] == 1)
        {
            attackSlotMonsterParts[attackSlot].triggerNeutralOrHeavyRefresh(true);
        }
    }

    public void dashAttack()
    {
        if (isRunning && canDashAttack && canRoll && focusedAttackActive == false)
        {
            isRunning = false;
            canRoll = false;
            canDashAttack = false;
            attackFocusOn();
            StartCoroutine(dashVisuals());
        }
    }

    IEnumerator dashVisuals()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerRoll(true);
        }

        myAnimator.SetFloat("Flipping Speed", 1);
        myAnimator.ResetTrigger("Roll");
        myAnimator.SetTrigger("Roll");

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerVisualDissappearance();
        }

        dashSplat.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerVisualReappearance();
            allMonsterParts[i].triggerRoll(true);
        }

        myAnimator.ResetTrigger("Roll");
        myAnimator.SetTrigger("Roll");
        dashSplat.SetActive(false);
        attackFocusOff();
        canDashAttack = true;
        canRoll = true;
    }


    #endregion

    #region Movement

    public void flipCharacter()
    {
        if (isRunning)
        {
            screechingStop();
            StartCoroutine(characterFlipDelay(true));
        }
        else
        {
            StartCoroutine(characterFlipDelay(false));
        }
    }

    IEnumerator characterFlipDelay(bool needsDelay)
    {
        if (needsDelay)
        {
            yield return new WaitForSeconds(0.35f);
            myAnimator.SetFloat("Flipping Speed", 1);
        }
        else
        {
            yield return new WaitForSeconds(0);
            myAnimator.SetFloat("Flipping Speed", 1.5f);
        }

        if (facingRight)
        {
            facingRight = false;
            myAnimator.SetBool("Facing Right", facingRight);
            myAnimator.SetTrigger("Flip to Left");
            dashSplat.transform.eulerAngles = leftDashSplatRotation;
        }
        else
        {
            facingRight = true;
            myAnimator.SetBool("Facing Right", facingRight);
            myAnimator.SetTrigger("Flip to Right");
            dashSplat.transform.eulerAngles = rightDashSplatRotation;
        }
    }

    public void walk()
    {
        if (isGrounded)
        {
            isWalking = true;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerWalk();
            }

            myAnimator.SetBool("Idle Bounce Allowed", false);
        }
        else
        {
            isWalking = true;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerWalk();
            }
        }
    }

    public void stopWalking()
    {
        if (isGrounded)
        {
            isWalking = false;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerStopWalking();
            }

            myAnimator.SetBool("Idle Bounce Allowed", true);
        }
    }

    public void run()
    {
        if (isGrounded)
        {
            isRunning = true;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerRun();
            }

            myAnimator.SetBool("Idle Bounce Allowed", false);
        }
        else
        {
            isRunning = true;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerRun();
            }
        }
    }

    public void stopRunning()
    {
        if (isGrounded)
        {
            isRunning = false;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerStopRunning();
            }

            myAnimator.SetBool("Idle Bounce Allowed", true);
        }
        else
        {
            isRunning = false;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerStopRunning();
            }
        }
    }

    public void screechingStop()
    {
        if (isGrounded && isRunning)
        {
            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerScreechingStop();
            }
        }
    }

    public void jump()
    {
        if (isGrounded && jumpsLeft != 0)
        {
            isGrounded = false;
            isRunning = false;
            isWalking = false;
            focusedAttackActive = false;
            isGliding = false;
            jumpsLeft--;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerJump();
            }

            myAnimator.SetTrigger("Jump");
            myAnimator.SetBool("Idle Bounce Allowed", false);
        }
        else
        {
            doubleJump();
        }
    }

    public void doubleJump()
    {
        if (isWinged)
        {
            if (jumpsLeft != 0)
            {
                isGliding = false;
                jumpsLeft--;

                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].triggerWingFlap();
                }

            }
        }
        else
        {
            if (jumpsLeft != 0)
            {
                isGliding = false;
                jumpsLeft--;

                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].triggerRoll(false);
                }

                myAnimator.SetFloat("Flipping Speed", 1.5f);
                myAnimator.SetTrigger("Roll");
            }
        }
    }

    public void glide()
    {
        if (isGliding == false)
        {
            isGliding = true;
            focusedAttackActive = true;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerGlide();
            }
        }
        else
        {
            glideToFall();
        }
    }

    private void glideToFall()
    {
        focusedAttackActive = false;
        isGliding = false;

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerFall();
        }
    }

    public void walkToFall()
    {
        if (isGrounded)
        {
            isGrounded = false;
            isRunning = false;
            isWalking = false;
            focusedAttackActive = false;
            isGliding = false;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerFall();
            }

            myAnimator.SetBool("Idle Bounce Allowed", false);
        }
    }

    public void goThroughPlatform()
    {
        if (isGrounded)
        {
            isGrounded = false;
            isRunning = false;
            isWalking = false;
            focusedAttackActive = false;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerRoll(false);
            }

            myAnimator.SetFloat("Flipping Speed", 1.5f);
            myAnimator.SetTrigger("Roll");
            myAnimator.SetBool("Idle Bounce Allowed", false);
        }
    }

    public void land()
    {
        if(isGrounded == false)
        {
            isGrounded = true;
            focusedAttackActive = false;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerLand();
            }

            myAnimator.SetTrigger("Land");

            if (isRunning == false && isWalking == false)
            {
                myAnimator.SetBool("Idle Bounce Allowed", true);
            }

            if (isWinged)
            {
                jumpsLeft = jumpsAllowed_FlightedJump;
            }
            else
            {
                jumpsLeft = jumpsAllowed_DoubleJump;
            }
        }
    }

    public bool IsAttacking()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            if (allMonsterParts[i].isAttackingCheck())
            {
                return true;
            }
        }

        return false;
    }

    public void roll()
    {
        if (isGrounded && canRoll)
        {
            isGrounded = true;
            isRunning = false;
            isWalking = false;
            focusedAttackActive = false;
            canRoll = false;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerRoll(true);
            }

            myAnimator.SetFloat("Flipping Speed", 1.5f);
            myAnimator.SetTrigger("Roll");
            myAnimator.SetBool("Idle Bounce Allowed", false);
        }
    }

    #endregion

    #region Reactions
    public void braceForRightImpact()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerRightAttackStance();
        }
    }

    public void braceForLeftImpact()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerLeftAttackStance();
        }
    }

    public void braceForForwardImpact()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerForwardStance();
        }
    }

    public void braceForBackwardImpact()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerBackwardStance();
        }
    }

    public void switchBraceStance()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerHeavyLegStance();
        }
    }

    public void endBracing()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerUnbrace();
        }
    }

    public void hit()
    {
        isRunning = false;
        isWalking = false;
        focusedAttackActive = false;

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerHit();
        }

        //The hit animations are going to flip torso back and forth from this animator (so that flipping directions doesnt affect back and forth hit animations)
        //It does need to know which way to start though so that it is facing the camera
    }

    public void attackFocusOn()
    {
        focusedAttackActive = true;
        myAnimator.SetBool("Idle Bounce Allowed", false);
        myAnimator.ResetTrigger("Back to Prior State");
        //myAnimator.SetTrigger("Back to Prior State");
    }

    public void attackFocusOff()
    {
        focusedAttackActive = false;
        if (isGrounded)
        {
            myAnimator.SetBool("Idle Bounce Allowed", true);
        }
        myAnimator.ResetTrigger("Right Attack Release");
        myAnimator.ResetTrigger("Left Attack Release");
        myAnimator.SetTrigger("Back to Prior State");
    }

    #endregion

    #region Corrections

    public void correctAttackDirection(int limbPlacement)
    {
        if (limbPlacement == -1) //punch sent from left limb while facing left --> full turn; punch sent from left limb while facing right --> half turn
        {
            myAnimator.SetTrigger("Left Attack Release");
        }
        else if(limbPlacement == 1) //punch sent from right limb while facing right --> full turn; punch sent from right limb while facing left --> half turn
        {
            myAnimator.SetTrigger("Right Attack Release");
        }
        else
        {
            myAnimator.SetTrigger("Forward Attack Release");
        }
    }

    public void correctWalkingAttackAnimations()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].walkToAttackCorrections();
        }
    }

    public void correctRunningAttackAnimations()
    {
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].runToAttackCorrections();
        }
    }

    public void correctRollControl()
    {
        canRoll = true;
        if (isGrounded)
        {
            myAnimator.SetBool("Idle Bounce Allowed", true);
        }
    }

    #endregion
}
