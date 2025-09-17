using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeavyAttack : BaseAttack
{
    public enum HeavyAttackType
    {
        None,
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
        Spinning,
    }
    // a property drawer relies on this var using string lookup. Please do not change the var name or the property drawer will stop working
    public HeavyAttackType Attack;


    public HeavyAttack GetAttack()
    {
        return Attack switch
        {
            HeavyAttackType.Jab => new JabHeavy { Attack = HeavyAttackType.Jab},
            HeavyAttackType.Slash => new SlashHeavy { Attack = HeavyAttackType.Slash},
            HeavyAttackType.Spray => new SprayHeavy { Attack = HeavyAttackType.Spray},
            HeavyAttackType.Projectile => new ProjectileHeavy { Attack = HeavyAttackType.Projectile},
            HeavyAttackType.Beam => new BeamHeavy { Attack = HeavyAttackType.Beam},
            HeavyAttackType.Reel => new ReelHeavy { Attack = HeavyAttackType.Reel},
            HeavyAttackType.Grapple => new GrappleHeavy { Attack = HeavyAttackType.Grapple},
            HeavyAttackType.Boomerang => new BoomerangHeavy { Attack = HeavyAttackType.Boomerang},
            HeavyAttackType.Spinning => new SpinningHeavy { Attack = HeavyAttackType.Spinning },
            _ => new HeavyAttack { Attack = HeavyAttackType.None}
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

    protected void ApplyStatusEffectsToColliderReference(monsterPartReference colliderRef)
    {
        if (colliderRef == null) { return; }

        colliderRef.burnedStatusEffect = monsterPartRef.burnedStatusEffect;
        colliderRef.electrifiedStatusEffect = monsterPartRef.electrifiedStatusEffect;
        colliderRef.poisonedStatusEffect = monsterPartRef.poisonedStatusEffect;
        colliderRef.stinkyStatusEffect = monsterPartRef.stinkyStatusEffect;
        colliderRef.confusedStatusEffect = monsterPartRef.cursedStatusEffect;
        colliderRef.confusedStatusEffect = monsterPartRef.confusedStatusEffect;
        colliderRef.slimedStatusEffect = monsterPartRef.slimedStatusEffect;
        colliderRef.frozenStatusEffect = monsterPartRef.frozenStatusEffect;
        colliderRef.squashedStatusEffect = monsterPartRef.squashedStatusEffect;
        colliderRef.slowedStatusEffect = monsterPartRef.slowedStatusEffect;
        colliderRef.grabbedStatusEffect = monsterPartRef.grabbedStatusEffect;
    }

    protected void ApplyStatusEffectsToVFXHolder(vfxHolder vfxHolder)
    {
        vfxHolder.burnedStatusEffect = monsterPartRef.burnedStatusEffect;
        vfxHolder.electrifiedStatusEffect = monsterPartRef.electrifiedStatusEffect;
        vfxHolder.poisonedStatusEffect = monsterPartRef.poisonedStatusEffect;
        vfxHolder.stinkyStatusEffect = monsterPartRef.stinkyStatusEffect;
        vfxHolder.hauntedStatusEffect = monsterPartRef.cursedStatusEffect;
        vfxHolder.confusedStatusEffect = monsterPartRef.confusedStatusEffect;
        vfxHolder.slimedStatusEffect = monsterPartRef.slimedStatusEffect;
        vfxHolder.frozenStatusEffect = monsterPartRef.frozenStatusEffect;
        vfxHolder.squashedStatusEffect = monsterPartRef.squashedStatusEffect;
        vfxHolder.slowedStatusEffect = monsterPartRef.slowedStatusEffect;
        vfxHolder.grabbedStatusEffect = monsterPartRef.grabbedStatusEffect;
    }

    protected void StoredParentSetup()
    {
        monsterPartVisualRef.heavyVFXStoredParent = monsterPartVisualRef.heavyHitVFXHolder.transform.parent;
        monsterPartVisualRef.heavyVFXStoredPosition = monsterPartRef.transform.localPosition;
        monsterPartVisualRef.heavyVFXStoredRotation = monsterPartRef.transform.localRotation;
    }

    public virtual void triggerHeavyAttackPowerCheck()
    {

    }

    public virtual void heavyAttackPowerCalculation()
    {
        monsterPartRef.damage = monsterPartRef.baseHeavyAttackDamage + (monsterPartRef.builtUpAttackPower * monsterPartRef.builtUpAddedDamage);
        monsterPartRef.builtUpAttackPower = 0;
    }

    public virtual void triggerHeavyAttackPowerUp()
    {

    }



}
