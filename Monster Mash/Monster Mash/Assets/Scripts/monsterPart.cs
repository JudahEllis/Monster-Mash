using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPart : MonoBehaviour
{
    [Header("Monster Part Info")]
    public monsterAttackSystem myMainSystem;
    private SkinnedMeshRenderer[] mySkinnedMeshRenderers;
    private MeshRenderer[] myMeshRenderers;
    private ParticleSystem[] myIdleVFX;
    public Animator connectedMonsterPart;
    public Animator mainTorso;
    private Animator myAnimator;
    public int monsterPartHealth = 100;
    //monsterPartID - jumper parts = 0, organic parts = 1, and scientific parts = 2
    public int monsterPartID;
    public int attackAnimationID;
    public bool connected = true;

    //also because there is a pretty big oversight right now for "right" sided limbs that may end up being repositioned or rotated to act as a "left" sided limb
    [Header("Monster Part Questionaire")]
    public bool isArm;
    public bool isLeg;
    public bool isTail;
    public bool isWing;
    public bool isHead;
    public bool isEye;
    public bool isMouth;
    public bool isTorso;
    public bool isHorn;
    public bool isDecor;

    [Header("Monster Part Positioning Info")]
    public bool isJointed;
    public bool isRightShoulderLimb;
    public bool isLeftShoudlerLimb;
    public bool isRightPelvisLimb;
    public bool isLeftPelvisLimb;
    public bool isChestLimb;
    public bool isBellyLimb;
    public bool isNeckLimb;
    public bool isTailLimb;
    public bool isShoulderBladeLimb;
    public bool isTopHeadLimb;
    public bool isBacksideHeadLimb;
    public bool isRightEarLimb;
    public bool isLeftEarLimb;
    public bool isFacialLimb;
    public bool isRightSidedLimb;
    public bool isLeftSidedLimb;
    public bool isGroundedLimb;
    private string torsoCommand = "";
    private bool hasTorsoCommand = false;
    private string torsoCommandOverride = "";
    private bool hasTorsoCommandOverride = false; //refers to heads on torsos, torsos on torsos, torsos on heads on torsos, etc. that needs to move the main body
    private string headCommand = "";
    private bool hasHeadCommand = false;

    [Header("Internal Info - Don't Touch")]
    public bool attackMarkedHeavy = false;
    private bool heavyAttackInMotion = false;
    private bool fullActiveHeavy = false;
    public bool requiresRightStance = false;
    public bool requiresLeftStance = false;
    public bool requiresForwardStance = false;
    public bool requiresBackwardStance = false;
    public bool isLeadingLeg;
    public bool hasFlightedIdle = false;
    public Outline visualForAnimationTests;
    public bool hasHeavyBrace = false;
    private bool isAttacking = false;
    private bool attackFocusOn = false;
    private bool isRunning = false;
    private int jumpsAllotted;
    private int regularJumpAmount = 2;
    private int wingedJumpAmount = 4;

    #region Animation Team Tools

    public void changeAttackAnimationAtRuntime()
    {
        if (isLeg || isArm) //this will be expanded to include arms, tails, wings and heads
        {
            if (isLeg && attackAnimationID == 2) //corrections for if up attack is selected with leg
            {
                attackAnimationID = 1;
            }
            else if (isArm && attackAnimationID == -1) //corrections for if backwards attack is selected with arm
            {
                attackAnimationID = 1;
            }

            myAnimator.SetInteger("Attack Animation ID", attackAnimationID);

            #region Attack Reaction Calculations

            if (isRightShoulderLimb)
            {
                //we'll replace the useage of attack IDs here once we have gyroscopic understanding of limbs and then we can factor both
                //nothing wrong with it right now but if we have a forward attacking arm thats been rotated backwards then yeah we need something to account for that
                if (attackAnimationID == -1)
                {
                    torsoCommand = "Left Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
                else
                {
                    torsoCommand = "Right Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }

            if (isLeftShoudlerLimb)
            {

                if (attackAnimationID == -1)
                {
                    torsoCommand = "Right Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
                else
                {
                    torsoCommand = "Left Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
            }

            if (isRightPelvisLimb)
            {

                if (attackAnimationID == -1)
                {
                    torsoCommand = "Left Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
                else
                {
                    torsoCommand = "Right Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }

            if (isLeftPelvisLimb)
            {

                if (attackAnimationID == -1)
                {
                    torsoCommand = "Right Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
                else
                {
                    torsoCommand = "Left Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
            }

            if (isChestLimb || isNeckLimb)
            {
                torsoCommand = "Upper Attack";
                //isRightSidedLimb = true;
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }

            if (isBellyLimb)
            {
                torsoCommand = "Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }

            if (isTailLimb)
            {
                torsoCommand = "Upper Attack";
                //isRightSidedLimb = true;
                requiresBackwardStance = false;
                requiresForwardStance = true;
                requiresRightStance = false;
                requiresLeftStance = false;
            }

            if (isShoulderBladeLimb)
            {
                torsoCommand = "Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = true;
                requiresRightStance = false;
                requiresLeftStance = false;
            }

            if (isTopHeadLimb)
            {
                if (attackAnimationID == -1)
                {
                    headCommand = "Upward Attack";
                    torsoCommandOverride = "Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
                else
                {
                    headCommand = "Forward Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }

            if (isFacialLimb)
            {

                if (attackAnimationID == 2)
                {
                    if (isMouth || isEye)
                    {
                        headCommand = "Upward Attack";
                        torsoCommandOverride = "Lower Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
                    else
                    {
                        headCommand = "Forward Attack";
                        torsoCommandOverride = "Upper Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
                }
                else
                {
                    headCommand = "Forward Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }
            //Forgot back of the head attacks

            if (isRightEarLimb)
            {

                if (attackAnimationID == -1)
                {
                    headCommand = "Left Attack";
                    torsoCommandOverride = "Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
                else
                {
                    headCommand = "Right Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }

            if (isLeftEarLimb)
            {

                if (attackAnimationID == -1)
                {
                    headCommand = "Right Attack";
                    torsoCommandOverride = "Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
                else
                {
                    headCommand = "Left Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
            }

            if (torsoCommand != "")
            {
                hasTorsoCommand = true;
            }

            if (torsoCommandOverride != "")
            {
                hasTorsoCommandOverride = true;
            }

            if (headCommand != "")
            {
                hasHeadCommand = true;
            }

            #endregion
        }
    }

    public void setUpOutline()
    {
        if (monsterPartID == 1 || isWing) //fix this, I dont like having to clarify that a wing isn't a ID 1 just so that it can jump
        {
            if (gameObject.GetComponent<Outline>() != null)
            {
                visualForAnimationTests = gameObject.GetComponent<Outline>();
            }
            else
            {
                print("Oh No! You forgot to add an Outline component to" + " " + gameObject.name);
                return;
            }

            visualForAnimationTests.OutlineMode = Outline.Mode.OutlineVisible;
            visualForAnimationTests.OutlineColor = Color.yellow;
            visualForAnimationTests.OutlineWidth = 3f;
            disableOutline();
            myAnimator = this.GetComponent<Animator>();
        }
    }

    public void disableOutline()
    {
        if (visualForAnimationTests != null)
        {
            visualForAnimationTests.enabled = false;
        }
    }

    public void reenableOutline()
    {
        if (visualForAnimationTests != null)
        {
            visualForAnimationTests.enabled = true;
        }
    }

    #endregion

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

        if (isMouth && myAnimator != null) //this will be expanded to include arms, tails, wings and heads
        {
            myAnimator.SetInteger("Attack Animation ID", attackAnimationID);
        }

        if (isLeadingLeg)
        {
            myAnimator.SetBool("Is Leading Leg", true);
        }

        if (hasFlightedIdle)
        {
            myAnimator.SetBool("Has Flighted Idle", true);
        }

        #region Attack Reaction Calculations

        if (isRightShoulderLimb)
        {
            //we'll replace the useage of attack IDs here once we have gyroscopic understanding of limbs and then we can factor both
            //nothing wrong with it right now but if we have a forward attacking arm thats been rotated backwards then yeah we need something to account for that
            if (attackAnimationID == -1) 
            {
                torsoCommand = "Left Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = false;
                requiresLeftStance = true;
            }
            else
            {
                torsoCommand = "Right Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
        }

        if (isLeftShoudlerLimb)
        {

            if (attackAnimationID == -1)
            {
                torsoCommand = "Right Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
            else
            {
                torsoCommand = "Left Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = false;
                requiresLeftStance = true;
            }
        }

        if (isRightPelvisLimb)
        {

            if (attackAnimationID == -1)
            {
                torsoCommand = "Left Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = false;
                requiresLeftStance = true;
            }
            else
            {
                torsoCommand = "Right Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
        }

        if (isLeftPelvisLimb)
        {

            if (attackAnimationID == -1)
            {
                torsoCommand = "Right Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
            else
            {
                torsoCommand = "Left Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = false;
                requiresLeftStance = true;
            }
        }

        if (isChestLimb || isNeckLimb)
        {
            torsoCommand = "Upper Attack";
            //isRightSidedLimb = true;
            requiresBackwardStance = false;
            requiresForwardStance = false;
            requiresRightStance = true;
            requiresLeftStance = false;
        }

        if (isBellyLimb)
        {
            torsoCommand = "Lower Attack";
            requiresBackwardStance = false;
            requiresForwardStance = false;
            requiresRightStance = true;
            requiresLeftStance = false;
        }

        if (isTailLimb)
        {
            torsoCommand = "Upper Attack";
            //isRightSidedLimb = true;
            requiresBackwardStance = false;
            requiresForwardStance = true;
            requiresRightStance = false;
            requiresLeftStance = false;
        }

        if (isShoulderBladeLimb)
        {
            torsoCommand = "Lower Attack";
            requiresBackwardStance = false;
            requiresForwardStance = true;
            requiresRightStance = false;
            requiresLeftStance = false;
        }

        if (isTopHeadLimb)
        {
            if (attackAnimationID == -1)
            {
                headCommand = "Upward Attack";
                torsoCommandOverride = "Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
            else
            {
                headCommand = "Forward Attack";
                torsoCommandOverride = "Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
        }

        if (isFacialLimb)
        {

            if (attackAnimationID == 2)
            {
                if (isMouth || isEye)
                {
                    headCommand = "Upward Attack";
                    torsoCommandOverride = "Lower Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
                else
                {
                    headCommand = "Forward Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }
            else
            {
                headCommand = "Forward Attack";
                torsoCommandOverride = "Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
        }
        //Forgot back of the head attacks

        if (isRightEarLimb)
        {

            if (attackAnimationID == -1)
            {
                headCommand = "Left Attack";
                torsoCommandOverride = "Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = false;
                requiresLeftStance = true;
            }
            else
            {
                headCommand = "Right Attack";
                torsoCommandOverride = "Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
        }

        if (isLeftEarLimb)
        {

            if (attackAnimationID == -1)
            {
                headCommand = "Right Attack";
                torsoCommandOverride = "Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
            }
            else
            {
                headCommand = "Left Attack";
                torsoCommandOverride = "Upper Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = false;
                requiresLeftStance = true;
            }
        }

        if (torsoCommand != "")
        {
            hasTorsoCommand = true;
        }

        if (torsoCommandOverride != "")
        {
            hasTorsoCommandOverride = true;
        }

        if (headCommand != "")
        {
            hasHeadCommand = true;
        }

        #endregion

        #region Separating Visual and Combat Elements for Dash Attacks
        //search through all my objects and gather everything with a skinned mesh renderer
        List<GameObject> hitboxesAndHurtboxes = new List<GameObject>();

        Transform[] childrenInObject = GetComponentsInChildren<Transform>();
        for (int i = 0; i < childrenInObject.Length; i++)
        {
            if (childrenInObject[i].gameObject.tag == "Hurtbox" || childrenInObject[i].gameObject.tag == "Hitbox")
            {
                hitboxesAndHurtboxes.Add(childrenInObject[i].gameObject);
                childrenInObject[i].gameObject.SetActive(false);
            }
        }

        mySkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        myMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        myIdleVFX = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < hitboxesAndHurtboxes.Count; i++)
        {
            hitboxesAndHurtboxes[i].gameObject.SetActive(true);
        }
        #endregion


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
        else if ((isRightSidedLimb || isLeftSidedLimb) && isHorn == false)
        {
            float randomOffset = Random.Range(0, 0.5f);
            myAnimator.SetFloat("Idle Offset", randomOffset);
        }
        else if(isMouth && myAnimator != null)
        {
            float randomOffset = Random.Range(0, 0.5f);
            myAnimator.SetFloat("Idle Offset", randomOffset);
        }
    }

    public void triggerIdle()
    {
        if ((monsterPartID != 1 && isWing == false) || connected == false)
        {
            return;
        }

        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Idle");
        }
    }
    #endregion

    #region Attack Animations
    public void triggerAttack(string animationName)
    {
        //We'' be simplifying this section down once we know 100% whats happening with dash attacks
        if (connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
            connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
            connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Walk")) 
        {
            if (attackFocusOn == false && myAnimator != null)
            {
                isAttacking = true;
                myAnimator.SetTrigger(animationName);
                myMainSystem.attackFocusOn();
                attackFocusOn = true;
                runToAttackCorrections();
                attackMarkedHeavy = false;
                heavyAttackInMotion = false;
                fullActiveHeavy = false;
                triggerNeutralOrHeavyRefresh(false);
            }

        }
        else if (connectedMonsterPart.GetCurrentAnimatorStateInfo(0).IsName("Run") || connectedMonsterPart.GetBool("Running") == true)
        {
            if (attackAnimationID != 1 && isRunning == true && isGroundedLimb == true)
            {
                return;
            }
            else
            {
                if (attackFocusOn == false && myAnimator != null)
                {
                    isAttacking = true;
                    myAnimator.SetTrigger(animationName);
                    myMainSystem.attackFocusOn();
                    attackFocusOn = true;
                    runToAttackCorrections();
                    attackMarkedHeavy = false;
                    heavyAttackInMotion = false;
                    fullActiveHeavy = false;
                    triggerNeutralOrHeavyRefresh(false);
                }
            }
        }
    }

    public void triggerAttackAnticipation()
    {
        if (hasTorsoCommand)
        {
            connectedMonsterPart.SetTrigger(torsoCommand);
        }

        if (hasTorsoCommandOverride)
        {
            mainTorso.SetTrigger(torsoCommandOverride);
        }

        if (hasHeadCommand)
        {
            connectedMonsterPart.SetTrigger(headCommand);
        }
    }

    #region Attack Bracing Animations
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
                myAnimator.SetBool("Running", false);
            }
            else if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Airborn Brace");
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Airborn Brace");
                }
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
        else if (isMouth && myAnimator != null)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
        else if (isWing)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Fly") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Grounded") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
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
                myAnimator.SetBool("Running", false);
            }
            else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Airborn Brace");
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Airborn Brace");
                }
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
        else if (isMouth && myAnimator != null)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
        else if (isWing)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Fly") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Grounded") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
    }

    public void triggerForwardStance()
    {
        if (isGroundedLimb)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Forward Brace");
                }
                else if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Forward Brace");
                }

                myAnimator.SetBool("Walking", false);
                myAnimator.SetBool("Running", false);
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
        else if (isMouth && myAnimator != null)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
        else if (isWing)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Fly") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Grounded") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
    }

    public void triggerBackwardStance()
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
                    myAnimator.SetTrigger("Backward Brace");
                }

                myAnimator.SetBool("Walking", false);
                myAnimator.SetBool("Running", false);
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
        else if (isMouth && myAnimator != null)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
        else if (isWing)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Fly") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle Grounded") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
            {
                if (isAttacking == false)
                {
                    myAnimator.SetTrigger("Brace");
                }
            }
        }
    }

    public void triggerUnbrace()
    {
        if (isAttacking == false)
        {
            if (isGroundedLimb) // || isTorso
            {
                myAnimator.SetTrigger("Unbrace");
            }

            if (isArm)
            {
                myAnimator.SetTrigger("Unbrace");
            }

            if (isMouth && myAnimator != null)
            {
                myAnimator.SetTrigger("Unbrace");
            }

            if (isHead)
            {

            }
        }
    }

    #endregion

    public void triggerNeutralOrHeavyRefresh(bool inputCanceled)
    {
        //most likely a canceled input after system already has registered the difference between input but before the attack has been unleashed
        //aka a canceled heavy attack

        if (fullActiveHeavy == false && attackFocusOn == true)
        {
            if (inputCanceled)
            {
                if (heavyAttackInMotion)
                {
                    myAnimator.SetTrigger("Cancel Heavy");
                }
                else
                {
                    attackMarkedHeavy = false;
                    myAnimator.SetBool("Attack Marked Heavy", false);
                }
            }
            else
            {
                attackMarkedHeavy = true;
                myAnimator.SetBool("Attack Marked Heavy", true);
            }
        }
    }

    public void triggerNeutralOrHeavy()
    {
        if (attackMarkedHeavy)
        {
            heavyAttackInMotion = true;
            myMainSystem.switchBraceStance(); //for a stronger looking leg stance
        }
        else
        {
            myAnimator.SetTrigger("Force Neutral Attack");
        }
    }

    public void triggerHeavyLegStance()
    {
        if (isGroundedLimb)
        {
            myAnimator.SetTrigger("Switch Stance");
            hasHeavyBrace = true;
        }
    }

    public void inputCanceled()//this is when a heavy has been triggered but the player cancels it early
    {
        //if this part has a failure state or if it has the ability to cancel heavies early
    }

    public void triggerAttackRelease()
    {
        if (isJointed)
        {
            connectedMonsterPart.SetBool("Ready to Swing", true);
            connectedMonsterPart.SetBool("Walking", false);
            connectedMonsterPart.SetBool("Running", false);

            if (hasTorsoCommandOverride)
            {
                mainTorso.SetBool("Ready to Swing", true);
                mainTorso.SetBool("Walking", false);
                mainTorso.SetBool("Running", false);
            }

            isRunning = false;
            fullActiveHeavy = true;
            myMainSystem.correctWalkingAttackAnimations();

            if (isLeftSidedLimb)
            {
                myMainSystem.correctAttackDirection(-1);
            }
            else if (isRightSidedLimb)
            {
                myMainSystem.correctAttackDirection(1);
            }
            else
            {
                myMainSystem.correctAttackDirection(0);
            }
        }
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

            if (hasHeavyBrace)
            {
                myAnimator.SetTrigger("Switch Stance Quick");
                hasHeavyBrace = false;
            }
        }

        if (isHead)
        {
            myAnimator.SetBool("Walking", false);
        }
    }

    public void triggerAttackCorrections()
    {
        if (attackFocusOn)
        {
            myMainSystem.correctRunningAttackAnimations();
            myMainSystem.attackFocusOff();
            attackFocusOn = false;
            isAttacking = false;
            fullActiveHeavy = false;

            connectedMonsterPart.SetBool("Attack to Idle", false);

            if (hasTorsoCommandOverride)
            {
                mainTorso.SetBool("Attack to Idle", false);
            }
        }
    }

    public void runToAttackCorrections()
    {
        if (isGroundedLimb || isTorso)
        {
            myAnimator.SetBool("Running", false);
            myAnimator.SetBool("Walking", false);
            isRunning = false;
        }

        if (isArm)
        {
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }

        if (isHead)
        {
            myAnimator.SetBool("Running", false);
            myAnimator.SetBool("Walking", false);
            isRunning = false;
        }
    }

    public void triggerAttackToIdle()
    {
        connectedMonsterPart.SetBool("Attack to Idle", true);
        connectedMonsterPart.SetBool("Ready to Swing", false);

        if (hasTorsoCommandOverride)
        {
            mainTorso.SetBool("Attack to Idle", true);
            mainTorso.SetBool("Ready to Swing", false);
        }

        myMainSystem.endBracing();
    }

    #endregion

    #region Attack Effects

    public void triggerSprayAttack()
    {
        //get all the VFX from monster part reference, reparent, and reset them
    }

    public void triggerParticleUnparenting()
    {
        //get all the VFX from monster part reference and unparent them
    }

    #endregion

    #region Movement Animations
    public void triggerWalk()
    {
        if (isGroundedLimb || isTorso)
        {
            myAnimator.SetBool("Walking", true);
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }
        else if (isHead)
        {
            myAnimator.SetBool("Walking", true);
            myAnimator.SetBool("Running", false);
            isRunning = false;
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
            myAnimator.SetBool("Running", true);
            myAnimator.SetBool("Walking", false);
            isRunning = true;
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

    public void triggerStopRunning()
    {
        if (isGroundedLimb || isTorso)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                myAnimator.SetBool("Running", false);
                isRunning = false;

            }
        }

        if (isArm || isHead)
        {
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }
    }

    public void triggerScreechingStop()
    {
        if (isGroundedLimb || isTorso)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                //myAnimator.SetBool("Running", false);
                myAnimator.SetTrigger("Run to Screech");
                //isRunning = false;

            }
        }
    }

    public void triggerJump()
    {
        if (myAnimator != null)
        {
            myAnimator.SetBool("Grounded", false);
            myAnimator.SetTrigger("Jump");
        }

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

        if (isWing)
        {
            myAnimator.SetBool("Glide Activated", false);
        }
    }

    public void triggerRoll(bool groundedWhenTriggered)
    {
        if (isLeg || isArm ||isTorso || isHead || isMouth)
        {
            if (myAnimator != null)
            {
                myAnimator.SetBool("Grounded", groundedWhenTriggered);
                myAnimator.SetTrigger("Roll");
            }

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
    }

    public void triggerWingFlap()
    {
        if (isTorso)
        {
            myAnimator.SetTrigger("Upper Flap"); //change this so that its calculated at start with the other animations
            //allows us to use something like "lower flap" for wings on the butt
        }

        if (isWing)
        {
            myAnimator.SetTrigger("Big Flap");
        }

        if (isArm)
        {
            myAnimator.SetTrigger("Roll");
        }

        if (isLeg || isHead)
        {
            myAnimator.SetTrigger("Jump");
        }

        if ((isMouth || isEye) && myAnimator != null)
        {
            if (isMouth)
            {
                myAnimator.SetTrigger("Roll");
            }
            else
            {
                myAnimator.SetTrigger("Brace");
            }
        }
    }

    public void correctRollSpamControl()
    {
        myMainSystem.correctRollControl();
    }

    //This fall function is saved for when the player is knocked off an edge or walks over an edge (not a jump related fall)
    public void triggerFall()
    {
        if (myAnimator != null)
        {
            myAnimator.SetBool("Grounded", false);
            myAnimator.SetTrigger("Fall");
        }

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

        if (isWing)
        {
            myAnimator.SetBool("Glide Activated", false);
        }
    }

    public void triggerLand()
    {
        if (myAnimator != null)
        {
            myAnimator.SetBool("Grounded", true);
            myAnimator.SetTrigger("Land");
        }
    }

    public void triggerGlide()
    {
        if (isWing)
        {
            myAnimator.SetBool("Glide Activated", true);
        }
    }
    #endregion

    #region Reaction Animations

    public void triggerHit()
    {
        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Hit");
            isAttacking = false;
        }

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

    public void triggerVisualDissappearance()
    {
        for (int i = 0; i < mySkinnedMeshRenderers.Length; i++)
        {
            mySkinnedMeshRenderers[i].enabled = false;
        }

        for (int i = 0; i < myMeshRenderers.Length; i++)
        {
            myMeshRenderers[i].enabled = false;
        }

        for (int i = 0; i < myIdleVFX.Length; i++)
        {
            myIdleVFX[i].gameObject.SetActive(false);
        }
    }

    public void triggerVisualReappearance()
    {
        for (int i = 0; i < mySkinnedMeshRenderers.Length; i++)
        {
            mySkinnedMeshRenderers[i].enabled = true;
        }

        for (int i = 0; i < myMeshRenderers.Length; i++)
        {
            myMeshRenderers[i].enabled = true;
        }

        for (int i = 0; i < myIdleVFX.Length; i++)
        {
            myIdleVFX[i].gameObject.SetActive(true);
        }
    }

    #endregion

    #region Judah's BS
    public bool isAttackingCheck()
    {
        return isAttacking;
    }
    #endregion
}
