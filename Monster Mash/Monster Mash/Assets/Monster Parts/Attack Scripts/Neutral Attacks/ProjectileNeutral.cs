using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNeutral : NeutralAttack
{
    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        Attack = AttackType.Projectile;
    }
    public override void neutralAttackPowerCalculation()
    {
        base.neutralAttackPowerCalculation();
        monsterPartVisualRef.neutralHitVFXManager.damage = monsterPartRef.damage;
        monsterPartVisualRef.neutralHitVFXManager.updateDamageOnProjectiles();

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        monsterPartVisualRef.neutralHitVFXManager.damage = monsterPartRef.baseNeutralAttackDamage;
        monsterPartVisualRef.neutralHitVFXManager.updateDamageOnSpray();
    }

    public override void triggerNeutralAttackVisuals()
    {
        if (monsterPartVisualRef.neutralAttackHitVFXArray.Length != 0)
        {
            monsterPartVisualRef.neutralHitVFXManager.faceRightDirection(monsterPartRef.facingRight);
            monsterPartVisualRef.neutralHitVFXManager.unleashSingleProjectile();

            if (monsterPartVisualRef.neutralDefaultSprayVFXManager != null)
            {
                monsterPartVisualRef.neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
            }
        }
    }

    public override void SetupVFX()
    {
        if (monsterPartVisualRef.neutralHitVFXHolder != null || monsterPartVisualRef.neutralDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }
    }
}
