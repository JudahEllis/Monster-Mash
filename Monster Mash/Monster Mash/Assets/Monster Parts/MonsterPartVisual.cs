using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPartVisual : MonoBehaviour
{
    //This script is a mess a lot of the vars are not assigned. I just chucked all the VFX in here so I could deal with it seperate from the rest of the monster part script

    [HideInInspector] public bool attackMarkedHeavy = false;
    public Transform[] neutralAttackHitVFXArray;
    public Transform[] neutralAttackForwardSwingVFXArray;
    public Transform[] neutralAttackBackwardSwingVFXArray;
    public Transform[] neutralAttackDownwardSwingVFXArray;
    public Transform[] neutralAttackMissVFXArray;
    public Transform[] neutralAttackDefaultVFXArray;
    public Transform[] neutralStompVFXArray;

    public Transform[] heavyAttackHitVFXArray;
    public Transform[] heavyAttackForwardSwingVFXArray;
    public Transform[] heavyAttackBackwardSwingVFXArray;
    public Transform[] heavyAttackDownwardSwingVFXArray;
    public Transform[] heavyAttackMissVFXArray;
    public Transform[] heavyAttackDefaultVFXArray;
    public Transform[] heavyStompVFXArray;


    public GameObject neutralHitVFXHolder;
    public GameObject neutralForwardSwingVFXHolder;
    public GameObject neutralBackwardSwingVFXHolder;
    public GameObject neutralDownwardSwingVFXHolder;
    public GameObject neutralMissVFXHolder;
    public GameObject neutralDefaultSprayVFXHolder;
    public GameObject neutralStompVFXHolder;

    public GameObject heavyHitVFXHolder;
    public GameObject heavyForwardSwingVFXHolder;
    public GameObject heavyBackwardSwingVFXHolder;
    public GameObject heavyDownwardSwingVFXHolder;
    public GameObject heavyMissVFXHolder;
    public GameObject heavyDefaultSprayVFXHolder;
    public GameObject heavyStompVFXHolder;

    public ParticleSystem[] myIdleVFX;

    private vfxHolder neutralHitVFXManager;
    private vfxHolder neutralForwardSwingVFXManager;
    private vfxHolder neutralBackwardSwingVFXManager;
    private vfxHolder neutralDownwardSwingVFXManager;
    private vfxHolder neutralMissVFXManager;
    private vfxHolder neutralDefaultSprayVFXManager;
    private vfxHolder neutralStompVFXManager;

    private vfxHolder heavyHitVFXManager;
    private vfxHolder heavyForwardSwingVFXManager;
    private vfxHolder heavyBackwardSwingVFXManager;
    private vfxHolder heavyDownwardSwingVFXManager;
    private vfxHolder heavyMissVFXManager;
    private vfxHolder heavyDefaultSprayVFXManager;
    private vfxHolder heavyStompVFXManager;

    private Transform neutralHitVFXParent;
    private Transform neutralMissVFXParent;
    private Transform neutralDefaultSprayVFXParent;
    private Vector3 neutralDefaultSprayVFXStoredPosition;
    private Quaternion neutralDefaultSprayVFXStoredRotation;
    private Transform neutralVFXStoredParent;
    private Vector3 neutralVFXStoredPosition;
    private Quaternion neutralVFXStoredRotation;
    private int neutralVFXCount;

    private Transform heavyVFXStoredParent;
    private Vector3 heavyVFXStoredPosition;
    private Quaternion heavyVFXStoredRotation;
    private int heavyVFXCount;

    private NewMonsterPart monsterPartRef;


    public ParticleSystem chargeVisual;
    public ParticleSystem heavyChargeVisual;
    public GameObject specialRunVisual;
    private bool jabOrSlashLanded = false;
    public Transform neutralMuzzle;
    public int attackAnimationID = 1;
    public Transform heavyMuzzle;
    private bool reelAttackLanded = false;

    private void Awake()
    {
        monsterPartRef = GetComponent<NewMonsterPart>();
    }


    public void setUpVFX()//new attack projectile-like types must be added here
    {

        #region Neutral Hit VFX Holder
        if (neutralHitVFXHolder != null)
        {
            if (neutralHitVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralHitVFXManager = neutralHitVFXHolder.GetComponent<vfxHolder>();
            }

            switch (monsterPartRef.neutralAttack.Attack)
            {
                case NeutralAttack.AttackType.Boomerang:
                    neutralHitVFXManager.isBoomerangHolder = true;
                    goto case NeutralAttack.AttackType.Projectile;
                case NeutralAttack.AttackType.Projectile:
                case NeutralAttack.AttackType.Spray:
                    neutralVFXStoredParent = neutralHitVFXHolder.transform.parent;
                    neutralVFXStoredPosition = transform.localPosition;
                    neutralVFXStoredRotation = transform.localRotation;
                    break;
            }

            neutralAttackHitVFXArray = new Transform[neutralHitVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackHitVFXArray.Length; i++)
            {
                neutralAttackHitVFXArray[i] = neutralHitVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Neutral Forward Swing VFX Holder
        if (neutralForwardSwingVFXHolder != null)
        {
            if (neutralForwardSwingVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralForwardSwingVFXManager = neutralForwardSwingVFXHolder.GetComponent<vfxHolder>();
            }

            neutralAttackForwardSwingVFXArray = new Transform[neutralForwardSwingVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackForwardSwingVFXArray.Length; i++)
            {
                neutralAttackForwardSwingVFXArray[i] = neutralForwardSwingVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Neutral Backward Swing VFX Holder
        if (neutralBackwardSwingVFXHolder != null)
        {
            if (neutralBackwardSwingVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralBackwardSwingVFXManager = neutralBackwardSwingVFXHolder.GetComponent<vfxHolder>();
            }

            neutralAttackBackwardSwingVFXArray = new Transform[neutralBackwardSwingVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackBackwardSwingVFXArray.Length; i++)
            {
                neutralAttackBackwardSwingVFXArray[i] = neutralBackwardSwingVFXHolder.transform.GetChild(i);
            }
        }

        #endregion

        #region Neutral Downward Swing VFX Holder
        if (neutralDownwardSwingVFXHolder != null)
        {
            if (neutralDownwardSwingVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralDownwardSwingVFXManager = neutralDownwardSwingVFXHolder.GetComponent<vfxHolder>();
            }

            neutralAttackDownwardSwingVFXArray = new Transform[neutralDownwardSwingVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackDownwardSwingVFXArray.Length; i++)
            {
                neutralAttackDownwardSwingVFXArray[i] = neutralDownwardSwingVFXHolder.transform.GetChild(i);
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
        //new sprayable attack types must be added here
        if (neutralDefaultSprayVFXHolder != null)
        {
            if (neutralDefaultSprayVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralDefaultSprayVFXManager = neutralDefaultSprayVFXHolder.GetComponent<vfxHolder>();
            }

            switch (monsterPartRef.neutralAttack.Attack)
            {
                case NeutralAttack.AttackType.Projectile:
                case NeutralAttack.AttackType.Spray:
                case NeutralAttack.AttackType.Boomerang:
                    neutralVFXStoredParent = neutralHitVFXHolder.transform.parent;
                    neutralVFXStoredPosition = transform.localPosition;
                    neutralVFXStoredRotation = transform.localRotation;
                    break;
            }

            neutralAttackDefaultVFXArray = new Transform[neutralDefaultSprayVFXHolder.transform.childCount];
            for (int i = 0; i < neutralAttackDefaultVFXArray.Length; i++)
            {
                neutralAttackDefaultVFXArray[i] = neutralDefaultSprayVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Neutral Stomp VFX Holder
        if (neutralStompVFXHolder != null)
        {
            if (neutralStompVFXHolder.GetComponent<vfxHolder>() != null)
            {
                neutralStompVFXManager = neutralStompVFXHolder.GetComponent<vfxHolder>();
            }

            neutralStompVFXArray = new Transform[neutralStompVFXHolder.transform.childCount];
            for (int i = 0; i < neutralStompVFXArray.Length; i++)
            {
                neutralStompVFXArray[i] = neutralStompVFXHolder.transform.GetChild(i);
            }
        }
        #endregion


        #region Heavy Hit VFX Holder
        //new projectile-like attack types must be added here
        if (heavyHitVFXHolder != null)
        {
            if (heavyHitVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyHitVFXManager = heavyHitVFXHolder.GetComponent<vfxHolder>();
            }

            switch (monsterPartRef.heavyAttack.Attack)
            {
                case HeavyAttack.HeavyAttackType.Boomerang:
                    heavyHitVFXManager.isBoomerangHolder = true;
                    goto case HeavyAttack.HeavyAttackType.Projectile;
                case HeavyAttack.HeavyAttackType.Projectile:
                case HeavyAttack.HeavyAttackType.Spray:
                    heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
                    heavyVFXStoredPosition = transform.localPosition;
                    heavyVFXStoredRotation = transform.localRotation;
                    break;
            }

            heavyAttackHitVFXArray = new Transform[heavyHitVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackHitVFXArray.Length; i++)
            {
                heavyAttackHitVFXArray[i] = heavyHitVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Heavy Forward Swing VFX Holder
        if (heavyForwardSwingVFXHolder != null)
        {
            if (heavyForwardSwingVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyForwardSwingVFXManager = heavyForwardSwingVFXHolder.GetComponent<vfxHolder>();
            }

            heavyAttackForwardSwingVFXArray = new Transform[heavyForwardSwingVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackForwardSwingVFXArray.Length; i++)
            {
                heavyAttackForwardSwingVFXArray[i] = heavyForwardSwingVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Heavy Backward Swing VFX Holder
        if (heavyBackwardSwingVFXHolder != null)
        {
            if (heavyBackwardSwingVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyBackwardSwingVFXManager = heavyBackwardSwingVFXHolder.GetComponent<vfxHolder>();
            }

            heavyAttackBackwardSwingVFXArray = new Transform[heavyBackwardSwingVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackBackwardSwingVFXArray.Length; i++)
            {
                heavyAttackBackwardSwingVFXArray[i] = heavyBackwardSwingVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Heavy Downward Swing VFX Holder
        if (heavyDownwardSwingVFXHolder != null)
        {
            if (heavyDownwardSwingVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyDownwardSwingVFXManager = heavyDownwardSwingVFXHolder.GetComponent<vfxHolder>();
            }

            heavyAttackDownwardSwingVFXArray = new Transform[heavyDownwardSwingVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackDownwardSwingVFXArray.Length; i++)
            {
                heavyAttackDownwardSwingVFXArray[i] = heavyDownwardSwingVFXHolder.transform.GetChild(i);
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

            switch (monsterPartRef.heavyAttack.Attack)
            {
                case HeavyAttack.HeavyAttackType.Projectile:
                case HeavyAttack.HeavyAttackType.Spray:
                case HeavyAttack.HeavyAttackType.Boomerang:
                    heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
                    heavyVFXStoredPosition = transform.localPosition;
                    heavyVFXStoredRotation = transform.localRotation;
                    break;
            }

            heavyAttackDefaultVFXArray = new Transform[heavyDefaultSprayVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackDefaultVFXArray.Length; i++)
            {
                heavyAttackDefaultVFXArray[i] = heavyDefaultSprayVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Heavy Default Spray Holder
        //new sprayable attack types must be added here
        if (heavyDefaultSprayVFXHolder != null)
        {
            if (heavyDefaultSprayVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyDefaultSprayVFXManager = heavyDefaultSprayVFXHolder.GetComponent<vfxHolder>();
            }

            switch (monsterPartRef.heavyAttack.Attack)
            {
                case HeavyAttack.HeavyAttackType.Projectile:
                case HeavyAttack.HeavyAttackType.Spray:
                case HeavyAttack.HeavyAttackType.Boomerang:
                    heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
                    heavyVFXStoredPosition = transform.localPosition;
                    heavyVFXStoredRotation = transform.localRotation;
                    break;
            }

            heavyAttackDefaultVFXArray = new Transform[heavyDefaultSprayVFXHolder.transform.childCount];
            for (int i = 0; i < heavyAttackDefaultVFXArray.Length; i++)
            {
                heavyAttackDefaultVFXArray[i] = heavyDefaultSprayVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

        #region Heavy Stomp VFX Holder
        if (heavyStompVFXHolder != null)
        {
            if (heavyStompVFXHolder.GetComponent<vfxHolder>() != null)
            {
                heavyStompVFXManager = heavyStompVFXHolder.GetComponent<vfxHolder>();
            }

            heavyStompVFXArray = new Transform[heavyStompVFXHolder.transform.childCount];
            for (int i = 0; i < heavyStompVFXArray.Length; i++)
            {
                heavyStompVFXArray[i] = heavyStompVFXHolder.transform.GetChild(i);
            }
        }
        #endregion

    }

    public void idleVFXSeparation()
    {
        ParticleSystem[] tempVFXGrab = GetComponentsInChildren<ParticleSystem>();
        List<GameObject> tempDefaultSprayVFX = new List<GameObject>(); //this is to catch any VFX from default spray holders which, unlike other attack VFX, are active at this time
        for (int i = 0; i < tempVFXGrab.Length; i++)
        {
            if (tempVFXGrab[i].transform.parent.GetComponent<vfxHolder>() != null)
            {
                tempVFXGrab[i].gameObject.SetActive(false);
                tempDefaultSprayVFX.Add(tempVFXGrab[i].gameObject);
            }
        }

        myIdleVFX = GetComponentsInChildren<ParticleSystem>();


        for (int i = 0; i < tempDefaultSprayVFX.Count; i++)
        {
            tempDefaultSprayVFX[i].SetActive(true);
        }
    }

    public void triggerChargeVisual()
    {
        if (chargeVisual != null)
        {
            chargeVisual.Stop();
            chargeVisual.Play();
        }
    }

    public void triggerEndChargeVisual()
    {
        if (chargeVisual != null)
        {
            chargeVisual.Stop();
        }
    }

    public void triggerHeavyChargeVisual()
    {
        if (heavyChargeVisual != null)
        {
            heavyChargeVisual.Stop();
            heavyChargeVisual.Play();
        }
    }

    public void triggerRunVisual()
    {
        //if we decide that multiple pieces other than grounded legs should have a trail visual, we will move this into a full network message
        if (specialRunVisual != null)
        {
            specialRunVisual.SetActive(true);
        }
    }

    public void endRunVisual()
    {
        if (specialRunVisual != null)
        {
            specialRunVisual.SetActive(false);
        }
    }

    public void triggerStompVisual()
    {
        if (neutralStompVFXManager != null && attackMarkedHeavy == false)
        {
            neutralStompVFXManager.unleashAdditionalSprayVisual();
        }
        else if (heavyStompVFXManager != null && attackMarkedHeavy == true)
        {
            heavyStompVFXManager.unleashAdditionalSprayVisual();
        }
    }

    public void triggerNeutralAttackVisuals() //called in attack animation //new attack types must be added here
    {
        switch (monsterPartRef.neutralAttack.Attack)
        {
            case NeutralAttack.AttackType.Jab:
                if (!jabOrSlashLanded && neutralMissVFXHolder != null)
                {
                    neutralMissVFXManager.unleashJabOrSlash();
                }
                break;

            case NeutralAttack.AttackType.Slash:
                if (!jabOrSlashLanded && neutralMissVFXHolder != null)
                {
                    neutralMissVFXManager.unleashJabOrSlash();
                }
                break;

            case NeutralAttack.AttackType.Spray:
                neutralHitVFXHolder.transform.position = neutralMuzzle.transform.position;
                neutralHitVFXHolder.transform.rotation = neutralMuzzle.transform.rotation;

                if (neutralDefaultSprayVFXHolder != null)
                {
                    neutralDefaultSprayVFXHolder.transform.position = neutralMuzzle.transform.position;
                    neutralDefaultSprayVFXHolder.transform.rotation = neutralMuzzle.transform.rotation;
                }

                neutralHitVFXManager.unleashSpray();

                if (neutralDefaultSprayVFXManager != null)
                {
                    neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
                }
                break;

            case NeutralAttack.AttackType.Projectile:
                if (neutralAttackHitVFXArray.Length != 0)
                {
                    neutralHitVFXManager.faceRightDirection(monsterPartRef.facingRight);
                    neutralHitVFXManager.unleashSingleProjectile();

                    if (neutralDefaultSprayVFXManager != null)
                    {
                        neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
                    }
                }
                break;

            case NeutralAttack.AttackType.Boomerang:
                if (neutralAttackHitVFXArray.Length != 0)
                {
                    neutralHitVFXManager.faceRightDirection(monsterPartRef.facingRight);
                    neutralHitVFXManager.unleashSingleProjectile();

                    if (neutralDefaultSprayVFXManager != null)
                    {
                        neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
                    }
                }
                break;
        }

    }

    public void triggerNeutralSwingVisual()
    {
        if (neutralForwardSwingVFXManager && attackAnimationID == 1)
        {
            neutralForwardSwingVFXManager.unleashAdditionalSprayVisual();
        }
        else if (neutralBackwardSwingVFXManager && attackAnimationID == -1)
        {
            neutralBackwardSwingVFXManager.unleashAdditionalSprayVisual();
        }
        else if (neutralDownwardSwingVFXManager && attackAnimationID == 0)
        {
            neutralDownwardSwingVFXManager.unleashAdditionalSprayVisual();
        }
    }

    public void triggerHeavyAttackVisuals() //new attack types must be added here
    {
        switch (monsterPartRef.heavyAttack.Attack)
        {
            case HeavyAttack.HeavyAttackType.Jab:
            case HeavyAttack.HeavyAttackType.Slash:
                if (jabOrSlashLanded == false && heavyMissVFXHolder != null)
                {
                    //turn on miss visual if neutral vfx holder's script hasn't made contact
                    heavyMissVFXManager.unleashJabOrSlash();
                }

                break;
            case HeavyAttack.HeavyAttackType.Spray:
                heavyHitVFXHolder.transform.position = heavyMuzzle.transform.position;
                heavyHitVFXHolder.transform.rotation = heavyMuzzle.transform.rotation;

                if (heavyDefaultSprayVFXHolder != null)
                {
                    heavyDefaultSprayVFXHolder.transform.position = heavyMuzzle.transform.position;
                    heavyDefaultSprayVFXHolder.transform.rotation = heavyMuzzle.transform.rotation;

                }

                heavyHitVFXManager.unleashSpray();
                if (heavyDefaultSprayVFXManager)
                {
                    heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
                }

                break;
            case HeavyAttack.HeavyAttackType.Projectile:
            case HeavyAttack.HeavyAttackType.Boomerang:
                if (heavyAttackHitVFXArray.Length != 0)
                {
                    heavyHitVFXHolder.transform.position = heavyMuzzle.transform.position;
                    heavyHitVFXHolder.transform.rotation = heavyMuzzle.transform.rotation;

                    heavyHitVFXManager.faceRightDirection(monsterPartRef.facingRight);
                    heavyHitVFXManager.unleashSingleProjectile();

                    if (heavyDefaultSprayVFXManager)
                    {
                        heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
                    }
                }

                break;
            case HeavyAttack.HeavyAttackType.Reel:
                if (!reelAttackLanded)
                {
                    //miss visual
                    monsterPartRef.triggerReelCollisionsOff();
                }

                monsterPartRef.reelAttackBuiltUpPower = 0;
                monsterPartRef.reelAttackCurrentThreshold = 0;
                monsterPartRef.powerUpCheckAllowed = false;
                break;
            case HeavyAttack.HeavyAttackType.Beam:
                heavyHitVFXManager.unleashBeamVisual();
                break;

        }
    }

    public void triggerHeavySwingVisual()
    {
        if (heavyForwardSwingVFXManager && attackAnimationID == 1)
        {
            heavyForwardSwingVFXManager.unleashAdditionalSprayVisual();
        }
        else if (heavyBackwardSwingVFXManager && attackAnimationID == -1)
        {
            heavyBackwardSwingVFXManager.unleashAdditionalSprayVisual();
        }
        else if (heavyDownwardSwingVFXManager && attackAnimationID == 0)
        {
            heavyDownwardSwingVFXManager.unleashAdditionalSprayVisual();
        }
    }

    public void endRemainingVFX()
    {
        if (monsterPartRef.heavyAttack.Attack == HeavyAttack.HeavyAttackType.Beam)
        {
            heavyHitVFXManager.endBeamVisual();
        }

        endRunVisual();
    }
}
