using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayNeutral : NeutralAttack
{
    public override void neutralAttackPowerCalculation()
    {
        base.neutralAttackPowerCalculation();
        monsterPartVisualRef.neutralHitVFXManager.damage = damage;
        monsterPartVisualRef.neutralHitVFXManager.updateDamageOnProjectiles();

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        base.statusEffectAndDamageCalculations();
        monsterPartVisualRef.neutralHitVFXManager.damage = baseNeutralAttackDamage;
        monsterPartVisualRef.neutralHitVFXManager.updateDamageOnSpray();
    }

    public override void triggerNeutralAttackVisuals()
    {
        monsterPartVisualRef.neutralHitVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.neutralMuzzle.transform.position, 
            monsterPartVisualRef.neutralMuzzle.transform.rotation);

        if (monsterPartVisualRef.neutralDefaultSprayVFXHolder != null)
        {
            monsterPartVisualRef.neutralDefaultSprayVFXHolder.transform.position = monsterPartVisualRef.neutralMuzzle.transform.position;
            monsterPartVisualRef.neutralDefaultSprayVFXHolder.transform.rotation = monsterPartVisualRef.neutralMuzzle.transform.rotation;
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
