using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHeavy : HeavyAttack
{
    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        Attack = HeavyAttackType.Projectile;
    }
    public override void SetupVFX()
    {
        if (monsterPartVisualRef.heavyHitVFXHolder != null || monsterPartVisualRef.heavyDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }
    }

    public override void triggerHeavyAttackVisuals()
    {
        if (monsterPartVisualRef.heavyAttackHitVFXArray.Length != 0)
        {
            monsterPartVisualRef.heavyHitVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.heavyMuzzle.transform.position, monsterPartVisualRef.heavyMuzzle.transform.rotation);

            monsterPartVisualRef.heavyHitVFXManager.faceRightDirection(monsterPartRef.facingRight);
            monsterPartVisualRef.heavyHitVFXManager.unleashSingleProjectile();

            if (monsterPartVisualRef.heavyDefaultSprayVFXManager)
            {
                monsterPartVisualRef.heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
            }
        }
    }

    public override void heavyAttackPowerCalculation()
    {
        base.heavyAttackPowerCalculation();

        monsterPartVisualRef.heavyHitVFXManager.damage = monsterPartRef.damage;
        monsterPartVisualRef.heavyHitVFXManager.updateDamageOnProjectiles();

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        monsterPartVisualRef.heavyHitVFXManager.damage = monsterPartRef.baseHeavyAttackDamage;
        monsterPartVisualRef.heavyHitVFXManager.updateDamageOnProjectiles();
        ApplyStatusEffectsToVFXHolder(monsterPartVisualRef.heavyHitVFXManager);
        monsterPartVisualRef.heavyHitVFXManager.updateStatusEffectsOnProjectiles();
    }
}
