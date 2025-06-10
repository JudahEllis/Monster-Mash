using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayNeutral : NeutralAttack
{
    private vfxHolder neutralDefaultSprayVFXManager;

    public override void Init(MonsterPartVisual monsterPartVisual)
    {
        base.Init(monsterPartVisual);
        neutralDefaultSprayVFXManager = monsterPartVisual.neutralDefaultSprayVFXManager;
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
        neutralHitVFXHolder.transform.position = neutralMuzzle.transform.position;
        neutralHitVFXHolder.transform.rotation = neutralMuzzle.transform.rotation;

        if (neutralDefaultSprayVFXHolder != null)
        {
            neutralDefaultSprayVFXHolder.transform.position = neutralMuzzle.transform.position;
            neutralDefaultSprayVFXHolder.transform.rotation = neutralMuzzle.transform.rotation;
        }

        neutralHitVFXManager.unleashSpray();

        if (neutralDefaultSprayVFXManager != null)
        {
            neutralDefaultSprayVFXManager.unleashAdditionalSprayVisual();
        }
    }

    public override void SetupVFX()
    {
        if (neutralHitVFXHolder != null || neutralDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }
    }
}
