using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeutralAttack: BaseAttack
{
    public enum AttackType 
    {
        None,
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
    // a property drawer relies on this var using string lookup. Please do not change the var name or the property drawer will stop working
    public AttackType Attack;

    // factory pattern to assign the subclass
    public NeutralAttack GetAttack()
    {
        return Attack switch
        {
            AttackType.Jab => new JabNeutral { Attack = AttackType.Jab},
            AttackType.Slash => new SlashNeutral { Attack = AttackType.Slash},
            AttackType.Spray => new SprayNeutral { Attack = AttackType.Spray},
            AttackType.Projectile => new ProjectileNeutral { Attack = AttackType.Projectile},
            AttackType.Boomerang => new BoomerangNeutral { Attack = AttackType.Boomerang},
            _ => new NeutralAttack { Attack = AttackType.None},
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
