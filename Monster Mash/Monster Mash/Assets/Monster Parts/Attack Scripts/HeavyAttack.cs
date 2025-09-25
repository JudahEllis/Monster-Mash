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

    // factory pattern to assign the subclass
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
            HeavyAttackType.Spinning => new SpinningHeavy(),
            _ => new HeavyAttack ()
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
