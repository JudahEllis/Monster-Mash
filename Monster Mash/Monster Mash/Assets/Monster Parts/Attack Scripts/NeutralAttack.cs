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

    public virtual void neutralAttackPowerCalculation()
    {
        monsterPartRef.damage = monsterPartRef.baseNeutralAttackDamage;
    }

    public virtual void triggerNeutralAttackVisuals()
    {
        
    }

    public virtual void SetupVFX()
    {

    }

    protected void StoredParentSetup()
    {
        monsterPartVisualRef.neutralVFXStoredParent = monsterPartVisualRef.neutralHitVFXHolder.transform.parent;
        monsterPartVisualRef.neutralVFXStoredPosition = monsterPartRef.transform.localPosition;
        monsterPartVisualRef.neutralVFXStoredRotation = monsterPartRef.transform.localRotation;
    }
}
