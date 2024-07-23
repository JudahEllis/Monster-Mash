using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxHolder : MonoBehaviour
{
    public monsterPartReference[] damageGivingVFX;
    private GameObject[] damageGivingObjects; 
    public List<monsterPartReference> referencesToIgnore = new List<monsterPartReference>();
    //public bool usesContainers;
    //
    [Header("VFX Questionaire")]
    public bool isJabOrSlashHolder;
    private GameObject[] hitVFX;
    private int hitVFXCount;
    private Transform hitVFXParent;
    private Vector3 VFXPosition;
    //
    public bool isProjectileHolder;
    public float projectileSpeed;
    private GameObject[] projectileVFXArray;
    private int projectileVFXCount;
    //
    public bool isSprayHolder;
    public bool isSubSprayHolder;
    public float spreadOfSpray;
    private vfxHolder[] subSprayVFXArray;
    private int subSprayVFXCount = 0;
    public bool isDefaultSprayHolder;
    private ParticleSystem[] defaultSprayVFXArray;
    private int defaultSprayVFXCount;
    public Transform defaultSprayHolder;
    private Vector3 startingPoint = new Vector3(0, 0, 0);
    private Quaternion startingRotation;
    //
    public bool isBeamHolder;
    public GameObject beam;
    //
    public bool isReelInHolder;
    //
    public bool isMonsterSystemVFXHolder;
    private ParticleSystem[] MonsterSystemVFXArray;
    private int MonsterSystemVFXCount;
    public Transform MonsterSystemVFXHolder;
    //

    [Header("Damage and Status Effects")]
    public int damage = 0;
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
    public bool grabbedStatusEffect;


    public void grabReferences()
    {
        if (isProjectileHolder || isSubSprayHolder)
        {
            List<monsterPartReference> tempVFXList = new List<monsterPartReference>();

            for (int i = 0; i < transform.childCount; i++)
            {
                tempVFXList.Add(transform.GetChild(i).GetComponent<monsterPartReference>());
            }

            if (tempVFXList.Count > 0)
            {
                damageGivingVFX = new monsterPartReference[tempVFXList.Count];
                damageGivingObjects = new GameObject[tempVFXList.Count];

                for (int i = 0; i < damageGivingVFX.Length; i++)
                {
                    damageGivingVFX[i] = tempVFXList[i];
                    damageGivingObjects[i] = tempVFXList[i].gameObject;
                }
            }
        }
    }
    public void collisionOcclusion()
    {
        if (damageGivingVFX.Length > 0)
        {
            for (int i = 0; i < damageGivingVFX.Length; i++)
            {
                damageGivingVFX[i].referencesToIgnore = referencesToIgnore;
            }
        }

        additionalSetUp();
    }

    public void updateStatusEffectsOnProjectiles()
    {
        if (damageGivingVFX.Length > 0)
        {
            for (int i = 0; i < damageGivingVFX.Length; i++)
            {
                //damageGivingVFX[i].referencesToIgnore = referencesToIgnore;
                damageGivingVFX[i].burnedStatusEffect = burnedStatusEffect;
                damageGivingVFX[i].electrifiedStatusEffect = electrifiedStatusEffect;
                damageGivingVFX[i].poisonedStatusEffect = poisonedStatusEffect;
                damageGivingVFX[i].stinkyStatusEffect = stinkyStatusEffect;
                damageGivingVFX[i].hauntedStatusEffect = hauntedStatusEffect;
                damageGivingVFX[i].confusedStatusEffect = confusedStatusEffect;
                damageGivingVFX[i].slimedStatusEffect = slimedStatusEffect;
                damageGivingVFX[i].stunnedStatusEffect = stunnedStatusEffect;
                damageGivingVFX[i].frozenStatusEffect = frozenStatusEffect;
                damageGivingVFX[i].squashedStatusEffect = squashedStatusEffect;
                damageGivingVFX[i].slowedStatusEffect = slowedStatusEffect;
                damageGivingVFX[i].grabbedStatusEffect = grabbedStatusEffect;
            }
        }
    }

    public void updateStatusEffectsOnSpray()
    {
        if (subSprayVFXArray.Length > 0)
        {
            for (int i = 0; i < subSprayVFXArray.Length; i++)
            {
                subSprayVFXArray[i].burnedStatusEffect = burnedStatusEffect;
                subSprayVFXArray[i].electrifiedStatusEffect = electrifiedStatusEffect;
                subSprayVFXArray[i].poisonedStatusEffect = poisonedStatusEffect;
                subSprayVFXArray[i].stinkyStatusEffect = stinkyStatusEffect;
                subSprayVFXArray[i].hauntedStatusEffect = hauntedStatusEffect;
                subSprayVFXArray[i].confusedStatusEffect = confusedStatusEffect;
                subSprayVFXArray[i].slimedStatusEffect = slimedStatusEffect;
                subSprayVFXArray[i].stunnedStatusEffect = stunnedStatusEffect;
                subSprayVFXArray[i].frozenStatusEffect = frozenStatusEffect;
                subSprayVFXArray[i].squashedStatusEffect = squashedStatusEffect;
                subSprayVFXArray[i].slowedStatusEffect = slowedStatusEffect;
                subSprayVFXArray[i].grabbedStatusEffect = grabbedStatusEffect;
                subSprayVFXArray[i].updateDamageOnProjectiles();
            }
        }
    }

    public void updateDamageOnProjectiles()
    {
        if (damageGivingVFX.Length > 0)
        {
            for (int i = 0; i < damageGivingVFX.Length; i++)
            {
                damageGivingVFX[i].damage = damage;
            }
        }
    }

    public void updateDamageOnSpray()
    {
        if (subSprayVFXArray.Length > 0)
        {
            for (int i = 0; i < subSprayVFXArray.Length; i++)
            {
                subSprayVFXArray[i].damage = damage;
                subSprayVFXArray[i].updateDamageOnProjectiles();
            }
        }
    }

    #region Additional Set Up

    private void additionalSetUp()
    {
        if (isJabOrSlashHolder || isReelInHolder)
        {
            prepJabOrSlashVFX();
        }
        else if (isProjectileHolder)
        {
            prepProjectileVFX();
        }
        else if (isSprayHolder)
        {
            prepSprayVFX();
        }
        else if (isDefaultSprayHolder)
        {
            prepDefaultSprayVFX();
        }
        else if (isBeamHolder)
        {
            prepBeamVFX();
        }
        else if (isMonsterSystemVFXHolder)
        {
            prepMonsterSystemVFX();
        }
    }

    public void prepJabOrSlashVFX()
    {
        if (isJabOrSlashHolder || isReelInHolder)
        {
            List<GameObject> tempJabOrSlashVFX = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                tempJabOrSlashVFX.Add(transform.GetChild(i).gameObject);
            }
            hitVFX = new GameObject[tempJabOrSlashVFX.Count];
            for (int i = 0; i < hitVFX.Length; i++)
            {
                hitVFX[i] = tempJabOrSlashVFX[i];
            }

            VFXPosition = hitVFX[0].transform.localPosition;
            hitVFXParent = hitVFX[0].transform.parent;
        }
    }

    public void prepProjectileVFX()
    {
        if (isProjectileHolder || isSubSprayHolder)
        {
            List<GameObject> tempProjectileVFX = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                tempProjectileVFX.Add(transform.GetChild(i).gameObject);
            }
            projectileVFXArray = new GameObject[tempProjectileVFX.Count];
            for (int i = 0; i < projectileVFXArray.Length; i++)
            {
                projectileVFXArray[i] = tempProjectileVFX[i];
                //projectileVFXArray[i].GetComponent<projectile>().speed = projectileSpeed;
            }
        }
    }

    private void prepSprayVFX()
    {
        subSprayVFXArray = transform.GetComponentsInChildren<vfxHolder>();

        for (int i = 0; i < subSprayVFXArray.Length; i++)
        {
            subSprayVFXArray[i].prepSubSpray(spreadOfSpray);
        }
    }

    public void prepSubSpray(float degreeOfSpread)
    {
        if (isSubSprayHolder)
        {
            prepProjectileVFX();

            spreadOfSpray = degreeOfSpread;
            float degreeDifference = degreeOfSpread / (projectileVFXArray.Length - 1);
            float degreesLeft = degreeOfSpread / 2;
            //thought process: if we have 10 fireballs on each subspray and we want a spread of 40 degrees then the degree of difference is 4 degrees
            //it should be split between 20 degrees positive and 20 degrees negative, so starting with 20 degrees on the first projectile, we subtract 4 degrees -
            // - for each subsequent projectile
            //make sure to skip the projectile that is a perfect 0 degrees because this is the center projectile responsible for the true knockback of the attack
            //it should be marked as the center projectile
            for (int i = 0; i < projectileVFXArray.Length; i++)
            {
                if (projectileVFXArray[i].GetComponent<projectile>() != null)
                {
                    if (projectileVFXArray[i].GetComponent<projectile>().isCenterOfSpray == false)
                    {
                        if (degreesLeft != 0)
                        {
                            projectileVFXArray[i].transform.localRotation = Quaternion.Euler(degreesLeft, 0, 0);
                            degreesLeft = degreesLeft - degreeDifference;
                        }
                        else
                        {
                            degreesLeft = degreesLeft - degreeDifference;
                            projectileVFXArray[i].transform.localRotation = Quaternion.Euler(degreesLeft, 0, 0);
                        }
                    }
                }
            }
        }
    }

    public void prepDefaultSprayVFX()
    {
        if (isDefaultSprayHolder)
        {
            List<ParticleSystem> tempSprayVFX = new List<ParticleSystem>();
            for (int i = 0; i < transform.childCount; i++)
            {
                tempSprayVFX.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
            }
            defaultSprayVFXArray = new ParticleSystem[tempSprayVFX.Count];
            for (int i = 0; i < defaultSprayVFXArray.Length; i++)
            {
                defaultSprayVFXArray[i] = tempSprayVFX[i];
                //projectileVFXArray[i].GetComponent<projectile>().speed = projectileSpeed;
            }

            if (defaultSprayVFXArray[0] != null)
            {
                startingRotation = defaultSprayVFXArray[0].transform.localRotation;
            }
        }
    }

    public void prepBeamVFX()
    {
        beam = this.transform.GetChild(0).gameObject;
    }

    public void prepMonsterSystemVFX()
    {
        if (isMonsterSystemVFXHolder)
        {
            List<ParticleSystem> tempSprayVFX = new List<ParticleSystem>();
            for (int i = 0; i < transform.childCount; i++)
            {
                tempSprayVFX.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
            }
            MonsterSystemVFXArray = new ParticleSystem[tempSprayVFX.Count];
            for (int i = 0; i < MonsterSystemVFXArray.Length; i++)
            {
                MonsterSystemVFXArray[i] = tempSprayVFX[i];
            }

            if (MonsterSystemVFXArray[0] != null)
            {
                startingRotation = MonsterSystemVFXArray[0].transform.localRotation;
            }
        }
    }

    #endregion

    #region Specialized Attacks

    public void unleashJabOrSlash()
    {
        if (hitVFXCount == hitVFX.Length - 1)
        {
            hitVFXCount = 0;
        }
        else
        {
            hitVFXCount++;
        }

        hitVFX[hitVFXCount].SetActive(false);
        hitVFX[hitVFXCount].transform.parent = hitVFXParent;
        hitVFX[hitVFXCount].transform.localPosition = VFXPosition;
        hitVFX[hitVFXCount].transform.parent = null;
        hitVFX[hitVFXCount].SetActive(true);
    }

    public void unleashSingleProjectile()
    {
        if (projectileVFXCount == projectileVFXArray.Length - 1)
        {
            projectileVFXCount = 0;
        }
        else
        {
            projectileVFXCount++;
        }

        projectileVFXArray[projectileVFXCount].SetActive(true);
    }

    public void unleashAllProjectiles()
    {
        for (int i = 0; i < projectileVFXArray.Length; i++)
        {
            projectileVFXArray[i].SetActive(true);
        }
    }

    public void unleashSpray()
    {
        if (isSprayHolder)
        {
            if (subSprayVFXCount == subSprayVFXArray.Length - 1)
            {
                subSprayVFXCount = 1;
            }
            else
            {
                subSprayVFXCount++;
            }

            subSprayVFXArray[subSprayVFXCount].unleashAllProjectiles();
        }
    }

    public void unleashAdditionalSprayVisual()
    {
        if (defaultSprayVFXCount == defaultSprayVFXArray.Length - 1)
        {
            defaultSprayVFXCount = 0;
        }
        else
        {
            defaultSprayVFXCount++;
        }

        defaultSprayVFXArray[defaultSprayVFXCount].transform.parent = defaultSprayHolder;
        defaultSprayVFXArray[defaultSprayVFXCount].transform.localPosition = startingPoint;
        defaultSprayVFXArray[defaultSprayVFXCount].transform.localRotation = startingRotation;
        defaultSprayVFXArray[defaultSprayVFXCount].Stop();
        defaultSprayVFXArray[defaultSprayVFXCount].transform.parent = null;
        defaultSprayVFXArray[defaultSprayVFXCount].Play();
    }

    public void unleashBeamVisual()
    {
        beam.SetActive(true);
    }

    public void endBeamVisual()
    {
        beam.SetActive(false);
    }

    public void unleashReelInVisual()
    {
        if (hitVFXCount == hitVFX.Length - 1)
        {
            hitVFXCount = 0;
        }
        else
        {
            hitVFXCount++;
        }

        hitVFX[hitVFXCount].SetActive(false);
        hitVFX[hitVFXCount].transform.parent = hitVFXParent;
        hitVFX[hitVFXCount].transform.localPosition = VFXPosition;
        hitVFX[hitVFXCount].transform.parent = null;
        hitVFX[hitVFXCount].transform.localScale = new Vector3(3f, 3f, 3f);
        hitVFX[hitVFXCount].SetActive(true);
    }

    public void releaseMonsterSystemVisual()
    {
        if (MonsterSystemVFXCount == MonsterSystemVFXArray.Length - 1)
        {
            MonsterSystemVFXCount = 0;
        }
        else
        {
            MonsterSystemVFXCount++;
        }

        //defaultSprayVFXArray[MonsterSystemVFXCount].transform.parent = defaultSprayHolder;
        //defaultSprayVFXArray[MonsterSystemVFXCount].transform.localPosition = startingPoint;
        //defaultSprayVFXArray[MonsterSystemVFXCount].transform.localRotation = startingRotation;
        MonsterSystemVFXArray[MonsterSystemVFXCount].Stop();
        //defaultSprayVFXArray[MonsterSystemVFXCount].transform.parent = null;
        MonsterSystemVFXArray[MonsterSystemVFXCount].Play();
    }

    #endregion

    public void faceRightDirection(bool isFacingRight)
    {
        for (int i = 0; i < damageGivingVFX.Length; i++)
        {
            if (damageGivingObjects[i].GetComponent<projectile>())
            {
                damageGivingObjects[i].GetComponent<projectile>().facingRight = isFacingRight;
            }

        }
    }
}
