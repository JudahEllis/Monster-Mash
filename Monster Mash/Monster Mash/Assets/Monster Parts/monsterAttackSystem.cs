using System.Collections;
using System.Collections.Generic;
using System;
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
    float timeSinceLastCall;
    private int flourishCounter = 0;
    bool requiresFlourish = false;
    bool requiresFlourishingTwirl = false;
    bool requiresFlourishingRoll = false;
    bool calm = false;
    bool emoteActive = false;
    public bool onPlatformEdge;

    private Animator myAnimator;
    private Animator mainTorso;
    public monsterPart[] attackSlotMonsterParts = new monsterPart[8];
    private int[] attackSlotMonsterID = new int[8];
    private List<monsterPartReference> listOfInternalReferences = new List<monsterPartReference>();
    public monsterPart[] allMonsterParts;
    private List<monsterPart> nonMappedMonsterParts = new List<monsterPart>();
    public GameObject dashSplat;
    public ParticleSystem partDestructionVisual;
    public ParticleSystem destructionVisual;
    public GameObject destructionPhysicsHelper;
    public ParticleSystem glideVisual;
    public ParticleSystem gasVisual;
    public ParticleSystem musicVisual;
    public ParticleSystem rollingAttackVisual;
    private Vector3 leftDashSplatRotation = new Vector3 (0, 210, 0);
    private Vector3 rightDashSplatRotation = new Vector3(0, 270, 0);
    public Collider stompCollider;
    private bool grabActivated;
    public Transform nativeReel;
    private Transform foreignReel;
    private monsterAttackSystem foreignMonster;

    #region Monster Start Up

    public void connectNecessaryLocomotionComponents()
    {
        //dash splat 
        //spin control
        //idle bounce
        //stomp collider
        //reel anchor
        //WIND ZONE
        //torso
        //heads
    }

    public void removeAllLimbParenting() //we use this to remove all parenting during a full refresh
    {
        monsterPart[] monsterPartList = GetComponentsInChildren<monsterPart>();
        for (int i = 0; i < monsterPartList.Length; i++)
        {
            if (monsterPartList[i].isTorso == false && monsterPartList[i].isHead == false && monsterPartList[i].monsterPartID == 1)
            {
                monsterPartList[i].transform.parent = null;
            }
        }

        autoLimb_Connection[] limbConnections = GetComponentsInChildren<autoLimb_Connection>();
        for (int i = 0; i < limbConnections.Length; i++)
        {
            limbConnections[i].clearAllMonsterPartMemory();
        }
    }

    /*
    public void removeSpecificLimbParenting(monsterPart partToRemoveFromMonster) //we use this anytime a limb is grabbed/altered in editor
    {
        partToRemoveFromMonster.transform.parent = null;

        autoLimb_Connection[] limbConnections = GetComponentsInChildren<autoLimb_Connection>();
        for (int i = 0; i < limbConnections.Length; i++)
        {
            limbConnections[i].clearSpecificMonsterPartMemory(partToRemoveFromMonster);
        }
    }
    */

    public void turnOnLimbConnectors()
    {
        autoLimb_Connection[] limbConnections = GetComponentsInChildren<autoLimb_Connection>();
        for (int i = 0; i < limbConnections.Length; i++)
        {
            limbConnections[i].enableColliders();
        }
    }

    public void turnOffLimbConnectors() //make sure to turn off the connectors before awakening the beast
    {
        autoLimb_Connection[] limbConnections = GetComponentsInChildren<autoLimb_Connection>();
        for (int i = 0; i < limbConnections.Length; i++)
        {
            limbConnections[i].disableColliders();
        }
    }

    public void connectCurrentLimbs()
    {
        autoLimb_Connection[] limbConnections = GetComponentsInChildren<autoLimb_Connection>();
        for (int i = 0; i < limbConnections.Length; i++)
        {
            limbConnections[i].connectMonsterParts();
        }
    }

    public void awakenTheBeast()
    {
        myAnimator = this.GetComponent<Animator>();
        grabAttackSlotInfo();
        myAnimator.SetBool("Facing Right", facingRight);

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
                isWinged = true;
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

                    mainTorso.SetBool("Flighted Monster", isWinged);
                }
            }
        }

        #endregion

        monsterPartReference[] internalPartReferences = GetComponentsInChildren<monsterPartReference>();
        vfxHolder[] internalVFXHolders = GetComponentsInChildren<vfxHolder>();

        for (int i = 0; i < internalVFXHolders.Length; i++)
        {
            internalVFXHolders[i].grabReferences();

            for (int u = 0; u < internalVFXHolders[i].damageGivingVFX.Length; u++)
            {
                listOfInternalReferences.Add(internalVFXHolders[i].damageGivingVFX[u]);
            }
        }

        for (int i = 0; i < internalPartReferences.Length; i++)
        {
            listOfInternalReferences.Add(internalPartReferences[i]);
            internalPartReferences[i].mainSystem = this;
        }

        for (int i = 0; i < internalVFXHolders.Length; i++)
        {
            internalVFXHolders[i].referencesToIgnore = listOfInternalReferences;
            internalVFXHolders[i].collisionOcclusion();
        }

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].myMainSystem = this;
            allMonsterParts[i].mainTorso = mainTorso;
            allMonsterParts[i].referencesToIgnore = listOfInternalReferences;
            allMonsterParts[i].triggerAnimationSetUp();
            allMonsterParts[i].triggerAnimationOffsets();
            allMonsterParts[i].triggerCollisionLogic(); //collision logic must come after animation set up because animation set up includes projectile set up 
            allMonsterParts[i].triggerIdle();
        }

        myAnimator.SetBool("Idle Bounce Allowed", true);
        myAnimator.SetBool("Calm", false);
        calm = false;
    }

    public void grabAttackSlotInfo()
    {
        for (int i = 0; i < attackSlotMonsterParts.Length; i++)
        {
            if (attackSlotMonsterParts[i] != null)
            {
                attackSlotMonsterID[i] = attackSlotMonsterParts[i].monsterPartID; //this tells us what type of part class it is

                /*
                if (attackSlotMonsterParts[i].isWing == true)
                {
                    isWinged = true;
                }

                */
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

    public void resetMonster(bool turnToTheRight)
    {
        jumpsLeft = 0;
        Array.Clear(attackSlotMonsterID, 0, attackSlotMonsterID.Length);
        facingRight = turnToTheRight;
        myAnimator.SetBool("Facing Right", facingRight);
        myAnimator.SetBool("Idle Bounce Allowed", false);
        myAnimator.SetBool("Calm", false);
        calm = false;
        myAnimator = null;
        Array.Clear(allMonsterParts, 0, allMonsterParts.Length);
        isWinged = false;
        mainTorso.SetBool("Flighted Monster", false);
        mainTorso = null;

        for (int i = 0; i < allMonsterParts.Length; i++)
        {

            if (allMonsterParts[i].isWing)
            {
                allMonsterParts[i].hasFlightedIdle = false;
            }

            if (allMonsterParts[i].isLeg)
            {
                allMonsterParts[i].isLeadingLeg = false;
            }
        }

        monsterPartReference[] internalPartReferences = GetComponentsInChildren<monsterPartReference>();
        vfxHolder[] internalVFXHolders = GetComponentsInChildren<vfxHolder>();

        for (int i = 0; i < internalPartReferences.Length; i++)
        {
            internalPartReferences[i].mainSystem = null;
        }

        listOfInternalReferences.Clear();


        for (int i = 0; i < internalVFXHolders.Length; i++)
        {
            internalVFXHolders[i].referencesToIgnore = listOfInternalReferences;
            internalVFXHolders[i].collisionOcclusion();
        }

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].myMainSystem = null;
            allMonsterParts[i].mainTorso = null;
            allMonsterParts[i].referencesToIgnore = listOfInternalReferences;
            //gotta take care of all the alt versions of these functions for monster parts specifically
            allMonsterParts[i].triggerAnimationSetUp();
            allMonsterParts[i].triggerAnimationOffsets();
            allMonsterParts[i].triggerCollisionLogic(); //collision logic must come after animation set up because animation set up includes projectile set up 
            allMonsterParts[i].triggerIdle();
        }
    }

    #endregion

    #region Attacks

    private void Update()
    {
        timeSinceLastCall += Time.deltaTime;

        if (grabActivated)
        {
            foreignReel.position = new Vector3(nativeReel.position.x, nativeReel.position.y, foreignReel.position.z);
        }
    }

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
                if (focusedAttackActive == false)
                {
                    flourishCounter++;
                    if (timeSinceLastCall <= 1f)
                    {
                        if (flourishCounter >= 3)
                        {
                            requiresFlourish = true;
                            flourishCounter = 0;
                        }
                    }
                    else
                    {
                        flourishCounter = 1;
                    }
                    timeSinceLastCall = 0;
                }

                if (attackSlotMonsterParts[attackSlot].attackAnimationID == 0)
                {
                    if (isGrounded || attackSlotMonsterParts[attackSlot].isTail)
                    {
                        requiresFlourishingTwirl = false;
                        requiresFlourishingRoll = false;
                    }
                    else
                    {
                        requiresFlourishingTwirl = false;
                        requiresFlourishingRoll = true;
                    }
                }
                else if (attackSlotMonsterParts[attackSlot].attackAnimationID == 1)
                {
                    requiresFlourishingTwirl = true;
                    requiresFlourishingRoll = false;
                }
                else if (attackSlotMonsterParts[attackSlot].attackAnimationID == -1)
                {
                    if (attackSlotMonsterParts[attackSlot].isTail)
                    {
                        requiresFlourishingTwirl = true;
                        requiresFlourishingRoll = false;
                    }
                    else if (attackSlotMonsterParts[attackSlot].isLeg)
                    {
                        requiresFlourishingTwirl = false;
                        requiresFlourishingRoll = false;
                    }
                    else
                    {
                        requiresFlourishingTwirl = false;
                        requiresFlourishingRoll = true;
                    }
                }
                else if (attackSlotMonsterParts[attackSlot].attackAnimationID == 2)
                {
                    requiresFlourishingTwirl = true;
                    requiresFlourishingRoll = false;
                }

                if (isGrounded)
                {
                    if (focusedAttackActive == false)
                    {
                        attackSlotMonsterParts[attackSlot].triggerAttack("Ground Attack");

                        #region Bracing for Attacks 
                        if (requiresFlourish && (requiresFlourishingTwirl || requiresFlourishingRoll))
                        {
                            braceForFlourishImpact();
                        }
                        else
                        {
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
                        }

                        isRunning = false;
                        requiresFlourish = false;
                        #endregion
                    }
                }
                else
                {
          
                    if (focusedAttackActive == false)
                    {
                        attackSlotMonsterParts[attackSlot].triggerAttack("Airborn Attack");

                        #region Bracing for Attacks

                        if (requiresFlourish && isGliding == false && (requiresFlourishingTwirl || requiresFlourishingRoll))
                        {
                            //roll
                            braceForFlourishImpact();
                        }
                        else
                        {
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
                        }

                        isRunning = false;
                        requiresFlourish = false;
                        #endregion

                        if (isGliding)
                        {
                            glideToAttack();
                        }
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
        if ((isRunning || isGrounded == false) && canDashAttack && canRoll && focusedAttackActive == false)
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
            allMonsterParts[i].triggerRoll(isGrounded);
        }

        myAnimator.SetFloat("Flipping Speed", 1);
        myAnimator.ResetTrigger("Roll");
        myAnimator.SetTrigger("Roll");
        isGliding = false;
        glideVisual.Stop();
        myAnimator.SetBool("Gliding", false);
        myAnimator.ResetTrigger("Glide to Attack");

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
            allMonsterParts[i].triggerRoll(isGrounded);
        }

        myAnimator.ResetTrigger("Roll");
        myAnimator.SetTrigger("Roll");
        dashSplat.SetActive(false);
        attackFocusOff();

        yield return new WaitForSeconds(5f);

        if (isGrounded)
        {
            canDashAttack = true;
            canRoll = true;
        }
    }

    public void stompAttack()
    {
        stompCollider.enabled = true;
        StartCoroutine(stompReset());
    }

    IEnumerator stompReset()
    {
        yield return new WaitForSeconds(0.2f);
        stompCollider.enabled = false;
    }

    public void rollingUpwardsAttack()
    {
        isGrounded = false;
        isRunning = false;
        isWalking = false;
        canRoll = false;
        glideVisual.Stop();
        myAnimator.SetBool("Gliding", false);
        myAnimator.ResetTrigger("Glide to Attack");

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerRollingAttack();
        }

        myAnimator.SetFloat("Flipping Speed", 1f);
        myAnimator.SetTrigger("Nonstop Roll");
        myAnimator.SetBool("Idle Bounce Allowed", false);
        myAnimator.SetBool("Calm", false);
        calm = false;
        forceEndEmote();
        StartCoroutine(rollingAttackTimer());
    }

    public void rollingDownwardsAttack()
    {
        isGrounded = false;
        isRunning = false;
        isWalking = false;
        canRoll = false;
        glideVisual.Stop();
        myAnimator.SetBool("Gliding", false);
        myAnimator.ResetTrigger("Glide to Attack");

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerRollingAttack();
        }

        myAnimator.SetFloat("Flipping Speed", 1f);
        myAnimator.SetTrigger("Nonstop Roll");
        myAnimator.SetBool("Idle Bounce Allowed", false);
        myAnimator.SetBool("Calm", false);
        calm = false;
        forceEndEmote();
        StartCoroutine(rollingAttackTimer());
    }

    IEnumerator rollingAttackTimer()
    {
        rollingAttackVisual.Play();
        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].stopInfiniteRoll();
        }

        rollingAttackVisual.Stop();
    }

    public void leapingUpwardAttack()
    {

    }

    #endregion

    #region Movement

    public void flipCharacter()
    {
        if (focusedAttackActive == false)
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
            forceEndEmote();
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
            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].facingRight = false;
            }
        }
        else
        {
            facingRight = true;
            myAnimator.SetBool("Facing Right", facingRight);
            myAnimator.SetTrigger("Flip to Right");
            dashSplat.transform.eulerAngles = rightDashSplatRotation;
            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].facingRight = true;
            }
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
            myAnimator.SetBool("Calm", false);
            calm = false;
            forceEndEmote();
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
        if (isWalking)
        {
            if (isGrounded)
            {
                isWalking = false;

                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].triggerStopWalking();
                }

                myAnimator.SetBool("Idle Bounce Allowed", true);
                myAnimator.SetBool("Calm", false);
                calm = false;
            }
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
            myAnimator.SetBool("Calm", false);
            calm = false;
            forceEndEmote();
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
        if (isRunning)
        {
            if (isGrounded)
            {
                isRunning = false;

                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].triggerStopRunning();
                }

                myAnimator.SetBool("Idle Bounce Allowed", true);
                myAnimator.SetBool("Calm", false);
                calm = false;
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
            canDashAttack = true;
            canRoll = true;

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerJump();
            }

            myAnimator.SetTrigger("Jump");
            myAnimator.SetBool("Idle Bounce Allowed", false);
            myAnimator.SetBool("Calm", false);
            calm = false;
            forceEndEmote();
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
        if (isGrounded == false && isWinged)
        {
            if (isGliding == false)
            {
                isGliding = true;
                //focusedAttackActive = true;
                glideVisual.Play();
                myAnimator.SetBool("Gliding", true);

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
    }

    private void glideToFall()
    {
        focusedAttackActive = false;
        isGliding = false;
        glideVisual.Stop();
        myAnimator.ResetTrigger("Glide to Attack");
        myAnimator.SetBool("Gliding", false);

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerFall();
        }
    }

    private void glideToAttack()
    {
        isGliding = false;
        glideVisual.Stop();
        myAnimator.SetTrigger("Glide to Attack");
        myAnimator.SetBool("Gliding", false);
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
            myAnimator.SetBool("Calm", false);
            calm = false;
            forceEndEmote();
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
            myAnimator.SetBool("Calm", false);
            calm = false;
            forceEndEmote();
        }
    }

    public void land()
    {
        if(isGrounded == false)
        {
            isGrounded = true;
            focusedAttackActive = false;
            canDashAttack = true;
            canRoll = true;
            glideVisual.Stop();
            myAnimator.SetBool("Gliding", false);
            myAnimator.ResetTrigger("Glide to Attack");

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerRoll(isGrounded);
            }
            /*
            if (isGliding)
            {
                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].triggerRoll(isGrounded);
                }
            }
            else
            {
                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].triggerLand();
                }
            }
            */

            isGliding = false;

            myAnimator.SetTrigger("Land");

            if (isRunning == false && isWalking == false)
            {
                myAnimator.SetBool("Idle Bounce Allowed", true);
                myAnimator.SetBool("Calm", false);
                calm = false;
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

    /*
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
    */

    public void roll()
    {
        if (isGrounded && canRoll)
        {
            isGrounded = true;
            isRunning = false;
            isWalking = false;
            focusedAttackActive = false;
            canRoll = false;
            glideVisual.Stop();
            myAnimator.SetBool("Gliding", false);
            myAnimator.ResetTrigger("Glide to Attack");

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerRoll(true);
            }

            myAnimator.SetFloat("Flipping Speed", 1.5f);
            myAnimator.SetTrigger("Roll");
            myAnimator.SetBool("Idle Bounce Allowed", false);
            myAnimator.SetBool("Calm", false);
            calm = false;
            forceEndEmote();
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

    public void braceForFlourishImpact()
    {
        /*
        if (requiresFlourishingTwirl)
        {
            myAnimator.SetTrigger("Flourish Twirl");
        }
        else
        {
            myAnimator.SetTrigger("Flourish Roll");
        }
        */

        if (requiresFlourishingTwirl)
        {
            myAnimator.SetTrigger("Flourish Twirl");
        }
        else if (requiresFlourishingRoll)
        {
            myAnimator.SetTrigger("Flourish Roll");
        }

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].triggerFlourishStance();
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

        myAnimator.ResetTrigger("Glide to Attack");

        if (requiresFlourishingTwirl)
        {
            myAnimator.ResetTrigger("Flourish Twirl");
        }
        else if(requiresFlourishingRoll)
        {
            myAnimator.ResetTrigger("Flourish Roll");
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

        forceEndEmote();
        //The hit animations are going to flip torso back and forth from this animator (so that flipping directions doesnt affect back and forth hit animations)
        //It does need to know which way to start though so that it is facing the camera
    }

    public void grabbingActivated(monsterAttackSystem grabbedMonster, Transform reelBone ,Vector3 pointOfContact)
    {
        grabActivated = true;
        foreignMonster = grabbedMonster;
        foreignReel = foreignMonster.nativeReel;
        foreignReel.parent = null;
        foreignReel.position = pointOfContact;
        nativeReel.position = pointOfContact;
        nativeReel.parent = reelBone;
        foreignMonster.transform.parent = foreignReel;
    }

    public void grabbingStabilized()
    {
        nativeReel.parent = null;
    }

    public void grabbingCanceled()
    {
        // /*
        if (grabActivated)
        {
            foreignMonster.transform.parent = null;
            foreignReel.parent = foreignMonster.transform;
            foreignReel = null;
            foreignMonster = null;
            nativeReel.parent = this.transform;
            nativeReel.position = Vector3.zero;
        }

        grabActivated = false;
        // */
    }

    public void attackFocusOn()
    {
        focusedAttackActive = true;
        myAnimator.SetBool("Idle Bounce Allowed", false);
        myAnimator.SetBool("Calm", false);
        calm = false;
        forceEndEmote();
        myAnimator.ResetTrigger("Back to Prior State");
        //myAnimator.SetTrigger("Back to Prior State");

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].bounceCorrections(false);
        }
    }

    public void attackFocusOff()
    {
        focusedAttackActive = false;
        if (isGrounded)
        {
            myAnimator.SetBool("Idle Bounce Allowed", true);
            myAnimator.SetBool("Calm", false);
            calm = false;
        }
        myAnimator.ResetTrigger("Right Attack Release");
        myAnimator.ResetTrigger("Left Attack Release");
        myAnimator.SetTrigger("Back to Prior State");

        
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
           allMonsterParts[i].resetBracing();
        }

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].bounceCorrections(true);
        }

    }

    public void calmedDown()
    {
        if (calm == false && isGrounded && emoteActive == false)
        {
            calm = true;
            myAnimator.SetBool("Calm", true);

            if (onPlatformEdge)
            {
                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].teeterOnEdge();
                }
            }
            else
            {
                for (int i = 0; i < allMonsterParts.Length; i++)
                {
                    allMonsterParts[i].calmedDown();
                }
            }
        }
    }

    public void enteredPlatformEdge()
    {
        onPlatformEdge = true;
    }

    public void exitedPlatformEdge()
    {
        onPlatformEdge = false;
    }

    #endregion

    #region Health

    public void popOffMonsterPart(monsterPart partRemoved)
    {
        for (int i = 0; i < attackSlotMonsterParts.Length; i++)
        {
            if (attackSlotMonsterParts[i] != null)
            {
                if (attackSlotMonsterParts[i] == partRemoved)
                {
                    partRemoved.disconnectThisPart();
                    destructionPhysicsHelper.SetActive(true);
                    StartCoroutine(removeMonsterPartFromStage(attackSlotMonsterParts[i].gameObject));
                    //store some sort of parental and location data
                    //bool saying disconnect
                    //blank animation for part
                    //remove part's parent
                    //turn on its rigidbody
                    //while we're at it, maybe just turn off the monster part script
                }
            }
        }

        healthCheck();
    }

    private void healthCheck()
    {
        int limbsStillActive = 0;
        for (int i = 0; i < attackSlotMonsterParts.Length; i++)
        {
            if (attackSlotMonsterParts[i] != null)
            {
                if (attackSlotMonsterParts[i].connected)
                {
                    limbsStillActive++;
                }
            }
        }

        if (limbsStillActive == 0)
        {
            totalDestruction();
        }
        else
        {
            partDestructionVisual.Play();
        }
    }

    public void totalDestruction()
    {

        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            if (allMonsterParts[i].connected)
            {
                nonMappedMonsterParts.Add(allMonsterParts[i]);
            }
        }

        for (int i = 0; i < nonMappedMonsterParts.Count; i++)
        {
            nonMappedMonsterParts[i].disconnectThisPart();
        }

        StartCoroutine(removeAllMonsterPartsFromStage());

        destructionVisual.Play();
        //spawn goo
    }

    private void turnOffAllMonsterPartPhysics()
    {

        for (int i = 0; i < nonMappedMonsterParts.Count; i++)
        {
            nonMappedMonsterParts[i].turnOffPhysics();
        }
    }


    private void deactivateAllMonsterParts()
    {

        for (int i = 0; i < nonMappedMonsterParts.Count; i++)
        {
            nonMappedMonsterParts[i].gameObject.SetActive(false);
        }
    }

    private void reactivateAllMonsterParts()
    {

        for (int i = 0; i < nonMappedMonsterParts.Count; i++)
        {
            nonMappedMonsterParts[i].gameObject.SetActive(true);
        }
    }

    IEnumerator removeMonsterPartFromStage(GameObject partToDisappear)
    {
        yield return new WaitForSeconds(0.1f);
        destructionPhysicsHelper.SetActive(false);
        yield return new WaitForSeconds(3);
        partToDisappear.gameObject.SetActive(false);
        partToDisappear.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(0.2f);
        partToDisappear.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        partToDisappear.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        partToDisappear.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        partToDisappear.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        partToDisappear.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        partToDisappear.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        partToDisappear.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        partToDisappear.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        partToDisappear.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        partToDisappear.SetActive(false);

    }

    IEnumerator removeAllMonsterPartsFromStage()
    {
        yield return new WaitForSeconds(0.1f);
        destructionPhysicsHelper.SetActive(false);
        yield return new WaitForSeconds(8);
        deactivateAllMonsterParts();
        turnOffAllMonsterPartPhysics();
        yield return new WaitForSeconds(0.2f);
        reactivateAllMonsterParts();
        yield return new WaitForSeconds(0.5f);
        deactivateAllMonsterParts();
        yield return new WaitForSeconds(0.2f);
        reactivateAllMonsterParts();
        yield return new WaitForSeconds(0.4f);
        deactivateAllMonsterParts();
        yield return new WaitForSeconds(0.3f);
        reactivateAllMonsterParts();
        yield return new WaitForSeconds(0.1f);
        deactivateAllMonsterParts();
        yield return new WaitForSeconds(0.1f);
        reactivateAllMonsterParts();
        yield return new WaitForSeconds(0.1f);
        deactivateAllMonsterParts();
        yield return new WaitForSeconds(0.1f);
        reactivateAllMonsterParts();
        yield return new WaitForSeconds(0.1f);
        deactivateAllMonsterParts();
    }

    #endregion

    #region Emotes
    public void fierceEmote()
    {
        if (focusedAttackActive == false && isGrounded && emoteActive == false && isRunning == false && isWalking == false)
        {
            emoteActive = true;
            calm = false;
            myAnimator.SetBool("Idle Bounce Allowed", false);

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].fierceEmote();
            }
        }
    }

    public void gasEmote()
    {
        if (focusedAttackActive == false && isGrounded && emoteActive == false && isRunning == false && isWalking == false)
        {
            emoteActive = true;
            calm = false;
            myAnimator.SetBool("Idle Bounce Allowed", false);
            gasVisual.Stop();
            gasVisual.Play();

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].gasEmote();
            }
        }
    }

    public void mockingEmote()
    {
        if (focusedAttackActive == false && isGrounded && emoteActive == false && isRunning == false && isWalking == false)
        {
            emoteActive = true;
            calm = false;
            myAnimator.SetBool("Idle Bounce Allowed", false);

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].mockingEmote();
            }
        }
    }

    public void danceEmote()
    {
        if (focusedAttackActive == false && isGrounded && emoteActive == false && isRunning == false && isWalking == false)
        {
            emoteActive = true;
            calm = false;
            myAnimator.SetBool("Idle Bounce Allowed", false);
            musicVisual.Stop();
            musicVisual.Play();

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].danceEmote();
            }
        }
    }

    public void jackEmote()
    {
        if (focusedAttackActive == false && isGrounded && emoteActive == false && isRunning == false && isWalking == false)
        {
            emoteActive = true;
            calm = false;
            myAnimator.SetBool("Idle Bounce Allowed", false);

            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].jackEmote();
            }
        }
    }

    public void emoteEnded()
    {
        emoteActive = false;
        musicVisual.Stop();
        for (int i = 0; i < allMonsterParts.Length; i++)
        {
            allMonsterParts[i].emoteCorrections();
        }
        calmedDown();
    }

    public void forceEndEmote()
    {
        if (emoteActive)
        {
            emoteActive = false;
            myAnimator.SetBool("Calm", false);
            gasVisual.Stop();
            musicVisual.Stop();
            for (int i = 0; i < allMonsterParts.Length; i++)
            {
                allMonsterParts[i].triggerForceStopEmote();
            }
        }
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
