using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPart : MonoBehaviour
{
    [Header("Monster Part Info")]
    public monsterAttackSystem myMainSystem;
    private SkinnedMeshRenderer[] mySkinnedMeshRenderers;
    private MeshRenderer[] myMeshRenderers;
    public Animator connectedMonsterPart;
    public Animator mainTorso;
    private Animator myAnimator;
    public Collider stompDetection;
    public int monsterPartHealth = 100;
    //monsterPartID - jumper parts = 0, organic parts = 1, and scientific parts = 2
    public int monsterPartID = 1;
    public int attackAnimationID = 1;
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

    [Header("Neutral Attack Questionaire")]
    public bool jabNeutralAttack;
    public bool slashNeutralAttack;
    public bool sprayNeutralAttack;
    public bool projectileNeutralAttack;
    public bool beamNeutralAttack;
    public GameObject neutralHitVFXHolder;
    private Transform neutralHitVFXParent;
    public GameObject neutralMissVFXHolder;
    private Transform neutralMissVFXParent;
    public GameObject neutralDefaultSprayVFXHolder;
    private Transform neutralDefaultSprayVFXParent;
    private Vector3 neutralDefaultSprayVFXStoredPosition;
    private Quaternion neutralDefaultSprayVFXStoredRotation;
    public Collider neutralCollider;
    private vfxHolder neutralHitVFXManager;
    private vfxHolder neutralMissVFXManager;
    private vfxHolder neutralDefaultSprayVFXManager;
    public Transform neutralForwardMuzzle;
    public Transform neutralUpwardMuzzle;
    public Transform neutralDownwardMuzzle;
    public Transform neutralBackwardMuzzle;
    public Transform[] neutralAttackHitVFXArray;
    public Transform[] neutralAttackMissVFXArray;
    public Transform[] neutralAttackDefaultVFXArray;
    private Transform neutralVFXStoredParent;
    private Vector3 neutralVFXStoredPosition;
    private Quaternion neutralVFXStoredRotation;
    private int neutralVFXCount;
    private bool jabOrSlashLanded = false;
    //private Transform neutralVFXParent;
    //private Vector3 neutralVFXPosition;
    //private Vector3 neutralVFXRotation;

    [Header("Heavy Attack Questionaire")]
    public bool jabHeavyAttack;
    public bool slashHeavyAttack;
    public bool sprayHeavyAttack;
    public bool projectileHeavyAttack;
    public bool beamHeavyAttack;
    public bool reelHeavyAttack;
    public GameObject heavyHitVFXHolder;
    public GameObject heavyMissVFXHolder;
    public GameObject heavyDefaultSprayVFXHolder;
    public Collider heavyCollider;
    private vfxHolder heavyHitVFXManager;
    private vfxHolder heavyMissVFXManager;
    private vfxHolder heavyDefaultSprayVFXManager;
    public Transform heavyForwardMuzzle;
    public Transform heavyUpwardMuzzle;
    public Transform heavyDownwardMuzzle;
    public Transform heavyBackwardMuzzle;
    public Transform[] heavyAttackHitVFXArray;
    public Transform[] heavyAttackMissVFXArray;
    public Transform[] heavyAttackDefaultVFXArray;
    private Transform heavyVFXStoredParent;
    private Vector3 heavyVFXStoredPosition;
    private Quaternion heavyVFXStoredRotation;
    private int heavyVFXCount;
    //Jab and Slash Info
    //Projectile Info
    //Spray Info
    //Reel Attack Info
    private int reelAttackBuiltUpPower = 0;
    private int reelAttackCurrentThreshold = 0;
    private bool powerUpCheckAllowed = true;
    private bool reelAttackLanded = false;
    //Status Effects
    public bool burnedStatusEffect;
    public bool electrifiedStatusEffect;
    public bool poisonedStatusEffect;
    public bool stinkyStatusEffect;
    public bool hauntedStatusEffect;
    public bool confusedStatusEffect;
    public bool slimedStatusEffect;
    public bool stunnedStatusEffect;
    public bool frozenStatusEffect;
    public bool squashedStatusEffect;
    public bool slowedStatusEffect;
    private bool grabbedStatusEffect;

    [Header("Monster Part Positioning Info")]
    public bool isJointed = true;
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
    public bool facingRight;
    public bool grounded = true;
    private bool haveGrabbedAMonster;
    private monsterPartReference grabbedMonster;
    //private int jumpsAllotted;
    //private int regularJumpAmount = 2;
    //private int wingedJumpAmount = 4;
    public ParticleSystem[] myIdleVFX;
    public List<monsterPartReference> referencesToIgnore = new List<monsterPartReference>();

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
            myIdleVFX = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < myIdleVFX.Length; i++)
            {
                myIdleVFX[i].gameObject.SetActive(false);
            }

            if (gameObject.GetComponent<Outline>() != null)
            {
                visualForAnimationTests = gameObject.GetComponent<Outline>();
                visualForAnimationTests.enabled = true;
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

        if (isLeg || isArm || isTail) //this will be expanded to include heads and horns
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
                if (isHorn)
                {
                    if (attackAnimationID == 1)
                    {
                        headCommand = "Forward Attack";
                        torsoCommandOverride = "Forward Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
                    else if (attackAnimationID == 2)
                    {
                        headCommand = "Face Attack";
                        torsoCommandOverride = "Upper Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
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
                if (isHorn)
                {
                    if (attackAnimationID == 1)
                    {
                        headCommand = "Forward Attack";
                        torsoCommandOverride = "Forward Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
                    else if (attackAnimationID == 2)
                    {
                        headCommand = "Face Attack";
                        torsoCommandOverride = "Upper Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
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
            else if (attackAnimationID == 0)
            {
                torsoCommand = "Right Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = true;
                requiresLeftStance = false;
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
            else if (attackAnimationID == 0)
            {
                torsoCommand = "Left Lower Attack";
                requiresBackwardStance = false;
                requiresForwardStance = false;
                requiresRightStance = false;
                requiresLeftStance = true;
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
            //headCommand = "Forward Attack";
            torsoCommandOverride = "Upper Attack";
            requiresBackwardStance = true;
            requiresForwardStance = false;
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
                if (isHorn)
                {
                    if (attackAnimationID == 1)
                    {
                        headCommand = "Forward Attack";
                        torsoCommandOverride = "Forward Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
                    else if (attackAnimationID == 2)
                    {
                        headCommand = "Face Attack";
                        torsoCommandOverride = "Upper Attack";
                        requiresBackwardStance = false;
                        requiresForwardStance = false;
                        requiresRightStance = true;
                        requiresLeftStance = false;
                    }
                }
                else
                {
                    headCommand = "Face Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
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
                    headCommand = "Face Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }
            else
            {
                headCommand = "Face Attack";
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
            if (isHorn)
            {
                if (attackAnimationID == 2)
                {
                    headCommand = "Upward Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
                else
                {
                    headCommand = "Forward Attack";
                    torsoCommandOverride = "Forward Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }
            else
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
                    torsoCommandOverride = "Forward Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }
        }

        if (isLeftEarLimb)
        {
            if (isHorn)
            {
                if (attackAnimationID == 2)
                {
                    headCommand = "Upward Attack";
                    torsoCommandOverride = "Upper Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
                else
                {
                    headCommand = "Forward Attack";
                    torsoCommandOverride = "Forward Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = true;
                    requiresLeftStance = false;
                }
            }
            else
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
                    torsoCommandOverride = "Forward Attack";
                    requiresBackwardStance = false;
                    requiresForwardStance = false;
                    requiresRightStance = false;
                    requiresLeftStance = true;
                }
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

        if (myIdleVFX.Length != 0) //checks for the idle VFX because of what needs to happen for the outline to not go around the VFX planes
        {
            for (int i = 0; i < myIdleVFX.Length; i++)
            {
                myIdleVFX[i].gameObject.SetActive(true);
            }
        }
        else
        {
            myIdleVFX = GetComponentsInChildren<ParticleSystem>();
        }

        for (int i = 0; i < hitboxesAndHurtboxes.Count; i++)
        {
            hitboxesAndHurtboxes[i].gameObject.SetActive(true);
        }
        #endregion

        setUpVFX();
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
        //this whole section needs an overhaul
        //I need a better way to exclude parts from this idle offset thing
        else if ((isRightSidedLimb || isLeftSidedLimb) && isHorn == false && isEye == false) 
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

    #region Collision Set Up

    public void triggerCollisionLogic()
    {
        for (int u = 0; u < referencesToIgnore.Count; u++)
        {
            referencesToIgnore[u].referencesToIgnore = referencesToIgnore;
        }

        monsterPartReference[] internalPartReferences = GetComponentsInChildren<monsterPartReference>();

        for (int i = 0; i < internalPartReferences.Length; i++)
        {
            internalPartReferences[i].partReference = this.GetComponent<monsterPart>();
        }
    }

    #endregion

    #region Attack Animations
    public void triggerAttack(string animationName)
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
            connectedMonsterPart.SetTrigger(headCommand);//current issue with making pieces connected to the torso affect the head with attacks
        }
    }

    #region Attack Bracing Animations
    public void triggerLeftAttackStance()
    {
        if (isGroundedLimb)
        {
            /*
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
            */
            if (grounded)
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
            else
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
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
        else if (isTail)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
    }

    public void triggerRightAttackStance()
    {
        if (isGroundedLimb)
        {
            /*
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
            */
            if (grounded)
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
            else
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
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
        else if (isTail)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
    }

    public void triggerForwardStance()
    {
        if (isGroundedLimb)
        {
            /*
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
            */

            if (grounded)
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
            else
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
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || 
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
        else if (isTail)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
    }

    public void triggerBackwardStance()
    {
        if (isGroundedLimb)
        {
            /*
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Launching Backward Brace");
                    myAnimator.SetBool("Needs Launch", true);
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Launching Backward Brace");
                    myAnimator.SetBool("Needs Launch", true);
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
            */

            if (grounded)
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Launching Backward Brace");
                    myAnimator.SetBool("Needs Launch", true);
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Launching Backward Brace");
                    myAnimator.SetBool("Needs Launch", true);
                }

                myAnimator.SetBool("Walking", false);
                myAnimator.SetBool("Running", false);
            }
            else
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
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
        else if (isTail)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
    }

    public void triggerFlourishStance()
    {
        if (isGroundedLimb)
        {
            /*
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Flourish");
                    myAnimator.SetBool("Needs Launch", true);
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Flourish");
                    myAnimator.SetBool("Needs Launch", true);
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
            */

            if (grounded)
            {
                if (isRightSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Flourish");
                    myAnimator.SetBool("Needs Launch", true);
                }
                else if (isLeftSidedLimb && isAttacking == false)
                {
                    myAnimator.SetTrigger("Flourish");
                    myAnimator.SetBool("Needs Launch", true);
                }

                myAnimator.SetBool("Walking", false);
                myAnimator.SetBool("Running", false);
            }
            else
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
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
                }
            }
        }
        else if (isTail)
        {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Land") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide") ||
                myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
            {
                if (isAttacking == false)
                {
                    myAnimator.ResetTrigger("Unbrace");
                    myAnimator.SetTrigger("Brace");
                    myAnimator.SetBool("Walking", false);
                    myAnimator.SetBool("Running", false);
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
                myAnimator.ResetTrigger("Unbrace");
                myAnimator.SetTrigger("Unbrace");
                myAnimator.ResetTrigger("Backward Brace");
                myAnimator.ResetTrigger("Forward Brace");
            }

            if (isArm || isWing || isTail)
            {
                myAnimator.ResetTrigger("Unbrace");
                myAnimator.SetTrigger("Unbrace");

                if (isWing)
                {
                    myAnimator.SetBool("Glide Activated", false);
                }
            }

            if (isMouth && myAnimator != null)
            {
                myAnimator.ResetTrigger("Unbrace");
                myAnimator.SetTrigger("Unbrace");
            }

            if (isHead)
            {

            }
        }
        else
        {
            if (isGroundedLimb)
            {
                myAnimator.ResetTrigger("Unbrace");
                myAnimator.SetTrigger("Unbrace");
                myAnimator.ResetTrigger("Backward Brace");
                myAnimator.ResetTrigger("Forward Brace");
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
            triggerHeavyAttackPowerUp();//by triggering the heavy, 1 power up is granted
        }
        else
        {
            myAnimator.SetTrigger("Force Neutral Attack");
        }
    }

    public void triggerHeavyLegStance()
    {
        if (isGroundedLimb) // && requiresBackwardStance == false
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

            if (attackMarkedHeavy)
            {
                if (isGroundedLimb && attackAnimationID == 0)
                {
                    //myMainSystem.stompAttack();
                }
                else if (jabHeavyAttack || slashHeavyAttack)
                {
                    triggerJabOrSlashCollisionsOn(); //make sure that the opposite function is called at interrupting points like fall, land, hit, etc.
                }
                else if (reelHeavyAttack)
                {
                    triggerReelCollisionsOn();
                }
            }
            else
            {
                if (isGroundedLimb && attackAnimationID == 0)
                {
                    //myMainSystem.stompAttack();
                }
                else if (jabNeutralAttack || slashNeutralAttack)
                {
                    triggerJabOrSlashCollisionsOn(); //make sure that the opposite function is called at interrupting points like fall, land, hit, etc.
                }
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

            if (hasHeavyBrace || myAnimator.GetBool("Needs Launch"))
            {
                myAnimator.SetTrigger("Switch Stance Quick");
                hasHeavyBrace = false;
                myAnimator.SetBool("Needs Launch", false);
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

            if (reelHeavyAttack)
            {
                myMainSystem.grabbingCanceled();
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

        if (isHead || isTail || isWing)
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

        //if I have a grabbed person, maybe let them go? That would be nice

        
        if (reelHeavyAttack)
        {
            //myMainSystem.grabbingStabilized();
        }
        
    }

    #endregion

    #region Attack Effects

    private void setUpVFX()
    {
        #region Neutral Hit VFX Holder
        if (neutralHitVFXHolder != null)
        {
            if (neutralHitVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralHitVFXManager = neutralHitVFXHolder.GetComponent<vfxHolder>();
            }

            if (projectileNeutralAttack || sprayNeutralAttack)
            {
                if (attackAnimationID == 0 && neutralDownwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralHitVFXHolder.transform.parent;
                    neutralDownwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralDownwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralDownwardMuzzle.localRotation;
                    neutralHitVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralHitVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 1 && neutralForwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralHitVFXHolder.transform.parent;
                    neutralForwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralForwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralForwardMuzzle.localRotation;
                    neutralHitVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralHitVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 2 && neutralUpwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralHitVFXHolder.transform.parent;
                    neutralUpwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralUpwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralUpwardMuzzle.localRotation;
                    neutralHitVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralHitVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == -1 && neutralBackwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralHitVFXHolder.transform.parent;
                    neutralBackwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralBackwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralBackwardMuzzle.localRotation;
                    neutralHitVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralHitVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
            }

            neutralAttackHitVFXArray = new Transform[neutralHitVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackHitVFXArray.Length; i++)
            {
                neutralAttackHitVFXArray[i] = neutralHitVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Neutral Miss VFX Holder
        if (neutralMissVFXHolder != null)
        {
            if (neutralMissVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralMissVFXManager = neutralMissVFXHolder.GetComponent<vfxHolder>();
            }

            neutralAttackMissVFXArray = new Transform[neutralMissVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackMissVFXArray.Length; i++)
            {
                neutralAttackMissVFXArray[i] = neutralMissVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Neutral Default Spray Holder
        if (neutralDefaultSprayVFXHolder != null)
        {
            if (neutralDefaultSprayVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralDefaultSprayVFXManager = neutralDefaultSprayVFXHolder.GetComponent<vfxHolder>();
            }

            if (projectileNeutralAttack || sprayNeutralAttack)
            {
                if (attackAnimationID == 0 && neutralDownwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralDefaultSprayVFXHolder.transform.parent;
                    neutralDownwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralDownwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralDownwardMuzzle.localRotation;
                    neutralDefaultSprayVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralDefaultSprayVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 1 && neutralForwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralDefaultSprayVFXHolder.transform.parent;
                    neutralForwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralForwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralForwardMuzzle.localRotation;
                    neutralDefaultSprayVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralDefaultSprayVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 2 && neutralUpwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralDefaultSprayVFXHolder.transform.parent;
                    neutralUpwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralUpwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralUpwardMuzzle.localRotation;
                    neutralDefaultSprayVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralDefaultSprayVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == -1 && neutralBackwardMuzzle != null)
                {
                    neutralVFXStoredParent = neutralDefaultSprayVFXHolder.transform.parent;
                    neutralBackwardMuzzle.transform.parent = neutralVFXStoredParent;
                    neutralVFXStoredPosition = neutralBackwardMuzzle.localPosition;
                    neutralVFXStoredRotation = neutralBackwardMuzzle.localRotation;
                    neutralDefaultSprayVFXHolder.transform.localPosition = neutralVFXStoredPosition;
                    neutralDefaultSprayVFXHolder.transform.localRotation = neutralVFXStoredRotation;
                    neutralDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
            }

            neutralAttackDefaultVFXArray = new Transform[neutralDefaultSprayVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackDefaultVFXArray.Length; i++)
            {
                neutralAttackDefaultVFXArray[i] = neutralDefaultSprayVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Heavy Hit VFX Holder
        if (heavyHitVFXHolder != null)
        {
            if (heavyHitVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyHitVFXManager = heavyHitVFXHolder.GetComponent<vfxHolder>();
            }

            if (projectileHeavyAttack || sprayHeavyAttack)
            {
                if (attackAnimationID == 0 && heavyDownwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
                    heavyDownwardMuzzle.transform.parent = heavyVFXStoredParent;
                    heavyVFXStoredPosition = heavyDownwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyDownwardMuzzle.localRotation;
                    heavyHitVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyHitVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 1 && heavyForwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
                    heavyForwardMuzzle.transform.parent = heavyVFXStoredParent;
                    heavyVFXStoredPosition = heavyForwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyForwardMuzzle.localRotation;
                    heavyHitVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyHitVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 2 && heavyUpwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
                    heavyUpwardMuzzle.transform.parent = neutralVFXStoredParent;
                    heavyVFXStoredPosition = heavyUpwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyUpwardMuzzle.localRotation;
                    heavyHitVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyHitVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == -1 && heavyBackwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
                    heavyBackwardMuzzle.transform.parent = heavyVFXStoredParent;
                    heavyVFXStoredPosition = heavyBackwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyBackwardMuzzle.localRotation;
                    heavyHitVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyHitVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyHitVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
            }

            heavyAttackHitVFXArray = new Transform[heavyHitVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackHitVFXArray.Length; i++)
            {
                heavyAttackHitVFXArray[i] = heavyHitVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Heavy Miss VFX Holder
        if (heavyMissVFXHolder != null)
        {
            if (heavyMissVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyMissVFXManager = heavyMissVFXHolder.GetComponent<vfxHolder>();
            }

            heavyAttackMissVFXArray = new Transform[heavyMissVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackMissVFXArray.Length; i++)
            {
                heavyAttackMissVFXArray[i] = heavyMissVFXHolder.transform.GetChild(i);
            }
        }

        if (heavyDefaultSprayVFXHolder != null)
        {
            if (heavyDefaultSprayVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyDefaultSprayVFXManager = heavyDefaultSprayVFXHolder.GetComponent<vfxHolder>();
            }

            if (projectileHeavyAttack || sprayHeavyAttack)
            {
                if (attackAnimationID == 0 && heavyDownwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyDefaultSprayVFXHolder.transform.parent;
                    heavyDownwardMuzzle.transform.parent = heavyVFXStoredParent;
                    heavyVFXStoredPosition = heavyDownwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyDownwardMuzzle.localRotation;
                    heavyDefaultSprayVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyDefaultSprayVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 1 && heavyForwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyDefaultSprayVFXHolder.transform.parent;
                    heavyForwardMuzzle.transform.parent = heavyVFXStoredParent;
                    heavyVFXStoredPosition = heavyForwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyForwardMuzzle.localRotation;
                    heavyDefaultSprayVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyDefaultSprayVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == 2 && heavyUpwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyDefaultSprayVFXHolder.transform.parent;
                    heavyUpwardMuzzle.transform.parent = heavyVFXStoredParent;
                    heavyVFXStoredPosition = heavyUpwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyUpwardMuzzle.localRotation;
                    heavyDefaultSprayVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyDefaultSprayVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
                else if (attackAnimationID == -1 && heavyBackwardMuzzle != null)
                {
                    heavyVFXStoredParent = heavyDefaultSprayVFXHolder.transform.parent;
                    heavyBackwardMuzzle.transform.parent = heavyVFXStoredParent;
                    heavyVFXStoredPosition = heavyBackwardMuzzle.localPosition;
                    heavyVFXStoredRotation = heavyBackwardMuzzle.localRotation;
                    heavyDefaultSprayVFXHolder.transform.localPosition = heavyVFXStoredPosition;
                    heavyDefaultSprayVFXHolder.transform.localRotation = heavyVFXStoredRotation;
                    heavyDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
                }
            }

            heavyAttackDefaultVFXArray = new Transform[heavyDefaultSprayVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackDefaultVFXArray.Length; i++)
            {
                heavyAttackDefaultVFXArray[i] = heavyDefaultSprayVFXHolder.transform.GetChild(i);
            }
        }
        #endregion
    }

    //Attack Specific Functions

    #region Jab and Slash Specific Functions
    public void triggerJabOrSlashHitDetect() //marks whether or not the hit VFX is needed
    {
        jabOrSlashLanded = true;
        //play out VFX

        if (attackMarkedHeavy == true)
        {
            if (heavyHitVFXManager != null)
            {
                heavyHitVFXManager.unleashJabOrSlash();
            }
        }
        else
        {
            if (neutralHitVFXManager != null)
            {
                neutralHitVFXManager.unleashJabOrSlash();
            }
        }
    }

    public void triggerJabOrSlashCollisionsOn() //called in attack animation
    {
        //turn on neutral vfx holder
        jabOrSlashLanded = false;
        
        if (attackMarkedHeavy == true && heavyCollider != null)
        {
            heavyCollider.enabled = true;
        }
        else if(attackMarkedHeavy == false && neutralCollider != null)
        {
            neutralCollider.enabled = true;
        }
        
    }

    public void triggerJabOrSlashCollisionsOff() //called in attack animation
    {
        //turn off neutral vfx holder
        jabOrSlashLanded = false;

        if (attackMarkedHeavy == true)
        {
            heavyCollider.enabled = false;
        }
        else
        {
            neutralCollider.enabled = false;
        }
        
    }
    #endregion

    public void triggerStompDetectorOn()
    {
        stompDetection.enabled = true;
        myMainSystem.stompAttack();
    }

    public void triggerStompDetectionOff()
    {
        stompDetection.enabled = false;
    }

    public void triggerNeutralAttackVisuals() //called in attack animation
    {
        if (jabNeutralAttack)
        {
            neutralCollider.enabled = false;

            if (jabOrSlashLanded == false && neutralMissVFXHolder != null)
            {
                //turn on miss visual if neutral vfx holder's script hasn't made contact
                neutralMissVFXManager.unleashJabOrSlash();
            }
        }
        else if (slashNeutralAttack)
        {
            neutralCollider.enabled = false;

            if (jabOrSlashLanded == false && neutralMissVFXHolder != null)
            {
                //turn on miss visual if neutral vfx holder's script hasn't made contact
                neutralMissVFXManager.unleashJabOrSlash();
            }
        }
        else if (sprayNeutralAttack)
        {
            //neutralHitVFXHolder.transform.parent = mainTorso.gameObject.transform;

            if (neutralDefaultSprayVFXHolder != null)
            {
                //neutralDefaultSprayVFXHolder.transform.parent = mainTorso.gameObject.transform;
            }

            neutralHitVFXManager.unleashSpray();
            if (neutralDefaultSprayVFXManager)
            {
                neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
            }
        }
        else if (projectileNeutralAttack && neutralAttackHitVFXArray.Length != 0)
        {
            heavyHitVFXManager.faceRightDirection(facingRight);
            neutralHitVFXManager.unleashSingleProjectile();
        }
        else if (beamNeutralAttack)
        {

        }
    }

    #region Reel Attack Specific Functions
    public void triggerReelHitDetect() //marks whether or not the hit VFX is needed
    {
        //jabOrSlashLanded = true;
        reelAttackLanded = true;
        //play out VFX

        if (attackMarkedHeavy == true)
        {
            //heavyHitVFXManager.unleashJabOrSlash();
            triggerReelCollisionsOff();
            reelAttackBuiltUpPower = 0; //the reason that this always starts at 1 is that just by initiating the heavy, players are given a power up
            reelAttackCurrentThreshold = 0;
            powerUpCheckAllowed = false;
            myAnimator.ResetTrigger("Reel Back");
            myAnimator.SetTrigger("Reel Back");
        }
    }

    public void triggerReelCollisionsOn() //called in attack animation
    {
        //turn on neutral vfx holder
        //jabOrSlashLanded = false;
        reelAttackLanded = false;

        if (attackMarkedHeavy == true && heavyCollider != null)
        {
            heavyCollider.enabled = true;
        }

        //print("reel collider turned back on");

    }

    public void triggerReelCollisionsOff() //called in attack animation
    {
        //turn off neutral vfx holder
        //jabOrSlashLanded = false;
        reelAttackLanded = false;

        if (attackMarkedHeavy == true)
        {
            heavyCollider.enabled = false;
        }

    }
    #endregion

    public void triggerHeavyAttackPowerUp() //built up in wind up animation
    {
        if (reelHeavyAttack)
        {
            reelAttackBuiltUpPower++;
            powerUpCheckAllowed = true;
        }
    }

    public void triggerHeavyAttackPowerCheck() //called at same time intervals as power up but is instead called in the heavy animation 
    {
        if (reelHeavyAttack && powerUpCheckAllowed)
        {
            reelAttackCurrentThreshold++;

            if (reelAttackCurrentThreshold == reelAttackBuiltUpPower)
            {
                reelAttackBuiltUpPower = 0; //the reason that this always starts at 1 is that just by initiating the heavy, players are given a power up
                reelAttackCurrentThreshold = 0;
                powerUpCheckAllowed = false;
                myAnimator.ResetTrigger("Reel Back");
                myAnimator.SetTrigger("Reel Back");
                triggerReelCollisionsOff();
            }
        }
    }

    public void triggerHeavyAttackVisuals()
    {
        if (jabHeavyAttack)
        {
            heavyCollider.enabled = false;

            if (jabOrSlashLanded == false && heavyMissVFXHolder != null)
            {
                //turn on miss visual if neutral vfx holder's script hasn't made contact
                heavyMissVFXManager.unleashJabOrSlash();
            }
        }
        else if (slashHeavyAttack)
        {
            heavyCollider.enabled = false;

            if (jabOrSlashLanded == false && heavyMissVFXHolder != null)
            {
                //turn on miss visual if neutral vfx holder's script hasn't made contact
                heavyMissVFXManager.unleashJabOrSlash();
            }
        }
        else if (sprayHeavyAttack)
        {
            heavyHitVFXManager.unleashSpray();
        }
        else if (projectileHeavyAttack && heavyAttackHitVFXArray.Length != 0)
        {
            heavyHitVFXManager.faceRightDirection(facingRight);
            heavyHitVFXManager.unleashSingleProjectile();
        }
        else if (reelHeavyAttack)
        {
            if (reelAttackLanded == false)
            {
                //miss visual
                triggerReelCollisionsOff();

            }
        }
        else if (beamHeavyAttack)
        {

        }
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
        else if (isHead || isWing || isTail)
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
        else if (isHead || isWing || isTail)
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

        /*
        if (isArm || isTail)
        {
            myAnimator.SetBool("Running", true);
            isRunning = true;
        }
        */

        if (isHead || isWing || isArm || isTail)
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

        if (isArm || isHead || isWing || isTail)
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

        if (isGroundedLimb || isTorso)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }

        if (isWing || isHead || isArm || isTail)
        {
            myAnimator.SetBool("Glide Activated", false);
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }

        grounded = false;
    }

    public void triggerRoll(bool groundedWhenTriggered)
    {
        if (isLeg || isArm ||isTorso || isHead || isMouth || isWing || isTail)
        {
            if (myAnimator != null)
            {
                myAnimator.SetBool("Grounded", groundedWhenTriggered);
                myAnimator.SetTrigger("Roll");
            }

            if (isGroundedLimb || isTorso || isHead || isWing || isTail)
            {
                myAnimator.SetBool("Walking", false);
                myAnimator.SetBool("Running", false);
                isRunning = false;

                if (isWing || isHead)
                {
                    myAnimator.SetBool("Glide Activated", false);
                }
            }

            if (isArm)
            {
                myAnimator.SetBool("Glide Activated", false);
                myAnimator.SetBool("Running", false);
                isRunning = false;
            }

            grounded = groundedWhenTriggered;
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

        if (isLeg || isHead || isTail)
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

        if (isGroundedLimb || isTorso)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }

        if (isWing || isHead || isArm || isTail)
        {
            myAnimator.SetBool("Glide Activated", false);
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
            isRunning = false;
        }

        grounded = false;
    }

    public void triggerLand()
    {
        if (myAnimator != null)
        {
            myAnimator.SetBool("Grounded", true);
            myAnimator.SetTrigger("Land");
        }

        if (isWing || isHead || isArm || isTail)
        {
            myAnimator.SetBool("Glide Activated", false);
        }

        grounded = true;
    }

    public void triggerGlide()
    {
        if (isWing || isHead || isArm || isTail)
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

        if (isGroundedLimb || isTorso || isHead || isTail)
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

    #region Corrections

    public void resetBracing()
    {
        if (isAttacking == false)
        {
            if (isGroundedLimb || isArm || (isMouth && myAnimator != null) || isWing || isTail)
            {
                myAnimator.ResetTrigger("Unbrace");

                if (isGroundedLimb)
                {
                    myAnimator.ResetTrigger("Switch Stance");
                    myAnimator.ResetTrigger("Switch Stance Quick");
                }

                /*
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Brace") && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") == false)
                {
                    myAnimator.SetTrigger("Unbrace");
                }
                */
            }
        }
    }

    public void bounceCorrections(bool bounceAllowed)
    {
        if (isGroundedLimb)
        {
            myAnimator.SetBool("Idle Bounce Allowed", bounceAllowed);
        }
    }

    #endregion

    /*
    #region Judah's BS
    public bool isAttackingCheck()
    {
        return isAttacking;
    }
    #endregion
    */
}
