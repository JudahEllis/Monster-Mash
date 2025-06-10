using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeutralAttack: BaseAttack
{
    public enum AttackType 
    {
        Jab,
        Slash,
        Spray,
        Projectile,
        HomingMissile,
        Anvil,
        BowlingBall,
        Gaseous,
        Reflecting,
        Suction,
        AOE,
        Boomerang
    }

    public AttackType Attack;

    protected monsterPartReference neutralColliderReference;

    // Monster visual vars
    protected GameObject neutralMissVFXHolder;
    protected vfxHolder neutralMissVFXManager;
    protected vfxHolder neutralHitVFXManager;
    protected GameObject neutralDefaultSprayVFXHolder;
    protected Transform neutralMuzzle;
    protected Transform[] neutralAttackHitVFXArray;
    protected GameObject neutralHitVFXHolder;
    protected Transform neutralVFXStoredParent;
    protected Vector3 neutralVFXStoredPosition;
    protected Quaternion neutralVFXStoredRotation;

    public NeutralAttack GetAttack()
    {
        return Attack switch
        {
            AttackType.Jab => new JabNeutral(),
            AttackType.Slash => new SlashNeutral(),
            AttackType.Spray => new SprayNeutral(),
            AttackType.Projectile => new ProjectileNeutral(),
            AttackType.Boomerang => new BoomerangNeutral(),
            _ => null,
        };
    }

    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        neutralColliderReference = monsterPartRef.neutralCollider.gameObject.GetComponent<monsterPartReference>();
    }

    public override void Init(MonsterPartVisual monsterPartVisual)
    {
        neutralMissVFXHolder = monsterPartVisual.neutralMissVFXHolder;
        neutralMissVFXManager = monsterPartVisual.neutralMissVFXManager;
        neutralHitVFXManager = monsterPartVisual.neutralHitVFXManager;
        neutralMuzzle = monsterPartVisual.neutralMuzzle;
        neutralAttackHitVFXArray = monsterPartVisual.neutralAttackHitVFXArray;
        neutralHitVFXHolder = monsterPartVisual.neutralHitVFXHolder;
        neutralVFXStoredParent = monsterPartVisual.neutralVFXStoredParent;
        neutralVFXStoredPosition = monsterPartVisual.neutralVFXStoredPosition;
        neutralVFXStoredRotation = monsterPartVisual.neutralVFXStoredRotation;
        neutralDefaultSprayVFXHolder = monsterPartVisual.neutralDefaultSprayVFXHolder;

    }


    public virtual void neutralAttackPowerCalculation()
    {
        damage = baseNeutralAttackDamage;
    }

    public virtual void triggerNeutralAttackVisuals()
    {
        
    }

    public virtual void SetupVFX()
    {

    }

    protected void StoredParentSetup()
    {
        neutralVFXStoredParent = neutralHitVFXHolder.transform.parent;
        neutralVFXStoredPosition = partTransform.localPosition;
        neutralVFXStoredRotation = partTransform.localRotation;
    }
}
