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

    // Monster part visual vars
    protected vfxHolder heavyHitVFXManager;
    protected GameObject heavyHitVFXHolder;
    protected GameObject heavyDefaultSprayVFXHolder;
    protected GameObject heavyMissVFXHolder;
    protected vfxHolder heavyMissVFXManager;
    protected vfxHolder heavyDefaultSprayVFXManager;
    protected Transform heavyMuzzle;
    protected Transform[] heavyAttackHitVFXArray;
    private Transform heavyVFXStoredParent;
    private Vector3 heavyVFXStoredPosition;
    private Quaternion heavyVFXStoredRotation;


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

    public override void Init(MonsterPartVisual monsterPartVisual)
    {
        heavyHitVFXManager = monsterPartVisual.heavyHitVFXManager;
        heavyVFXStoredParent = monsterPartVisual.heavyVFXStoredParent;
        heavyVFXStoredPosition = monsterPartVisual.heavyVFXStoredPosition;
        heavyVFXStoredRotation = monsterPartVisual.heavyVFXStoredRotation;
        heavyHitVFXHolder = monsterPartVisual.heavyHitVFXHolder;
        heavyDefaultSprayVFXHolder = monsterPartVisual.heavyDefaultSprayVFXHolder;
        heavyMissVFXHolder = monsterPartVisual.heavyMissVFXHolder;
        heavyMissVFXManager = monsterPartVisual.heavyMissVFXManager;
        heavyMuzzle = monsterPartVisual.heavyMuzzle;
        heavyDefaultSprayVFXManager = monsterPartVisual.heavyDefaultSprayVFXManager;
        heavyAttackHitVFXArray = monsterPartVisual.heavyAttackHitVFXArray;
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
        heavyVFXStoredParent = heavyHitVFXHolder.transform.parent;
        heavyVFXStoredPosition = partTransform.localPosition;
        heavyVFXStoredRotation = partTransform.localRotation;
    }
}
