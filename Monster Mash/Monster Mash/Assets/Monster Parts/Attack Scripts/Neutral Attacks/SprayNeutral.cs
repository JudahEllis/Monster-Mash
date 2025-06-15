using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayNeutral : NeutralAttack
{
    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        Attack = AttackType.Spray;
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
        base.statusEffectAndDamageCalculations();
        monsterPartVisualRef.neutralHitVFXManager.damage = monsterPartRef.baseNeutralAttackDamage;
        monsterPartVisualRef.neutralHitVFXManager.updateDamageOnSpray();
    }

    public override void triggerNeutralAttackVisuals()
    {
        monsterPartVisualRef.neutralHitVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.neutralMuzzle.transform.position, 
            monsterPartVisualRef.neutralMuzzle.transform.rotation);

        if (monsterPartVisualRef.neutralDefaultSprayVFXHolder != null)
        {
            monsterPartVisualRef.neutralDefaultSprayVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.neutralMuzzle.transform.position, 
                monsterPartVisualRef.neutralMuzzle.transform.rotation);
        }

        monsterPartVisualRef.neutralHitVFXManager.unleashSpray();

        if (monsterPartVisualRef.neutralDefaultSprayVFXManager != null)
        {
            monsterPartVisualRef.neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
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
