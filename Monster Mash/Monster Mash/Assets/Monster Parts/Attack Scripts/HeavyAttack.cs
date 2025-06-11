using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeavyAttack : BaseAttack
{
    public enum HeavyAttackType
    {
        Jab,
        Slash,
        Spray,
        Projectile,
        Beam,
        Reel,
        Grapple,
        Boomerang,
        HomingMissile,
        Anvil,
        BowlingBall,
        Gaseous,
        PowerBoosting,
        Shield,
        Reflecting,
        Eating,
        Suction,
        AOE,
        Charging,
        SelfExploding,
    }

    public HeavyAttackType Attack;


    public HeavyAttack GetAttack()
    {
        return Attack switch
        {
            HeavyAttackType.Jab => new JabHeavy(),
            HeavyAttackType.Slash => new SlashHeavy(),
            HeavyAttackType.Spray => new SprayHeavy(),
            HeavyAttackType.Projectile => new ProjectileHeavy(),
            HeavyAttackType.Beam => new BeamHeavy(),
            HeavyAttackType.Reel => new ReelHeavy(),
            HeavyAttackType.Grapple => new GrappleHeavy(),
            HeavyAttackType.Boomerang => new BoomerangHeavy(),
            _ => null
        };
    }

    public virtual void endRemainingVFX()
    {

    }

    public virtual void SetupVFX()
    {

    }

    public virtual void triggerHeavyAttackVisuals()
    {

    }

    protected void StoredParentSetup()
    {
        monsterPartVisualRef.heavyVFXStoredParent = monsterPartVisualRef.heavyHitVFXHolder.transform.parent;
        monsterPartVisualRef.heavyVFXStoredPosition = monsterPartRef.transform.localPosition;
        monsterPartVisualRef.heavyVFXStoredRotation = monsterPartRef.transform.localRotation;
    }
}
