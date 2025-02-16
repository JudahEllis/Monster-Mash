using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPartReference : MonoBehaviour
{
    public monsterAttackSystem mainSystem;
    public monsterPart partReference;
    public List<monsterPartReference> referencesToIgnore = new List<monsterPartReference>();
    private List<monsterAttackSystem> attackHistory = new List<monsterAttackSystem>();
    public bool isHitbox;
    public bool isHurtbox;
    public bool isStomp;
    public bool isProjectile;
    public bool isJabOrSlash;
    public bool isReel;
    public bool isBoomerang;

    [Header("Damage, Direction, and Status Effects")]
    public int damage = 0;
    public bool hasTickDamage;
    public int tickDamage = 0;
    public float tickTiming = 0;
    public float tickDamageDuration = 0;
    public bool markedHeavy;
    public int directionOfAttack = 1;
    private Vector3 pointOfContact;
    public bool hasStatusEffect;
    public bool burnedStatusEffect;
    public bool electrifiedStatusEffect;
    public bool poisonedStatusEffect;
    public bool stinkyStatusEffect;
    public bool cursedStatusEffect;
    public bool confusedStatusEffect;
    public bool slimedStatusEffect;
    public bool frozenStatusEffect;
    public bool squashedStatusEffect;
    public bool slowedStatusEffect;
    public bool grabbedStatusEffect;

    public void resetAttackHistory()
    {
        if (isHitbox && attackHistory.Count > 0)
        {
            attackHistory.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHitbox)
        {
            if (other.GetComponent<monsterPartReference>() != null)
            {
                monsterAttackSystem damagedMonster = other.GetComponent<monsterPartReference>().mainSystem;

                if (attackHistory.Contains(damagedMonster) == false && damagedMonster != mainSystem)
                {
                    //attack
                    pointOfContact = other.ClosestPointOnBounds(transform.position);
                    damagedMonster.myPlayer.damaged(damage, markedHeavy, directionOfAttack, pointOfContact);

                    if (hasStatusEffect && markedHeavy)
                    {
                        damagedMonster.myPlayer.statusGiven(burnedStatusEffect, electrifiedStatusEffect, confusedStatusEffect, stinkyStatusEffect, cursedStatusEffect, slowedStatusEffect, 
                                                            poisonedStatusEffect, frozenStatusEffect, slimedStatusEffect, tickDamage, tickTiming, tickDamageDuration);
                    }

                    #region VFX & Misc Response to Successful Hit
                    if (isProjectile || isBoomerang)
                    {
                        GetComponent<projectile>().impact();
                    }

                    if (isJabOrSlash)
                    {
                        partReference.triggerJabOrSlashHitDetect();
                        mainSystem.myPlayer.forceStopLeap();
                    }
                    #endregion

                    attackHistory.Add(damagedMonster);
                }
                else
                {
                    return;
                }

                /*
                if (referencesToIgnore.Contains(other.GetComponent<monsterPartReference>()) == false)
                {
                    if ((isProjectile || isBoomerang) && isHitbox)
                    {
                        GetComponent<projectile>().impact();
                        damagedMonster.neutralDamage();
                        //print(damage);
                    }

                    if (isJabOrSlash && isHitbox)
                    {
                        partReference.triggerJabOrSlashHitDetect();
                        damagedMonster.neutralDamage();
                        //print(damage);
                    }

                    if (isReel && isHitbox)
                    {
                        partReference.triggerReelHitDetect(); //probably pass along monster part reference script of whatever this hits for "grabbing"
                        Vector3 storedCollisionPoint = other.ClosestPoint(transform.position);
                        mainSystem.grabbingActivated(other.GetComponent<monsterPartReference>().mainSystem, this.transform, storedCollisionPoint);
                    }
                }

                */
            }
        }
       

        if(other.GetComponent<collisionMaterial>() != null)
        {
            if (isStomp)
            {
                Vector3 storedCollisionPoint = other.ClosestPoint(transform.position);
                other.GetComponent<collisionMaterial>().spawnVFX(storedCollisionPoint);
                partReference.triggerStompDetectionOff();
            }
        }
    }

}
