using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangNeutral : NeutralAttack
{
    private vfxHolder neutralDefaultSprayVFXManager;
    private bool facingRight;

    public override void Init(MonsterPartVisual monsterPartVisual)
    {
        base.Init(monsterPartVisual);
        neutralDefaultSprayVFXManager = monsterPartVisual.neutralDefaultSprayVFXManager;
    }

    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        facingRight = monsterPartRef.facingRight;
    }
    public override void neutralAttackPowerCalculation()
    {
        base.neutralAttackPowerCalculation();
        neutralHitVFXManager.damage = damage;
        neutralHitVFXManager.updateDamageOnProjectiles();

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        base.statusEffectAndDamageCalculations();
        neutralHitVFXManager.damage = baseNeutralAttackDamage;
        neutralHitVFXManager.updateDamageOnSpray();
    }

    public override void triggerNeutralAttackVisuals()
    {
        if (neutralAttackHitVFXArray.Length != 0)
        {
            neutralHitVFXManager.faceRightDirection(facingRight);
            neutralHitVFXManager.unleashSingleProjectile();

            if (neutralDefaultSprayVFXManager != null)
            {
                neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
            }
        }
    }
}
