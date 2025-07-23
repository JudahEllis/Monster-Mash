using System;

[Serializable]
public class SprayHeavy : HeavyAttack
{
    public override void SetupVFX()
    {
        if (monsterPartVisualRef.heavyHitVFXHolder != null || monsterPartVisualRef.heavyDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }
    }

    public override void triggerHeavyAttackVisuals()
    {
        monsterPartVisualRef.heavyHitVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.heavyMuzzle.transform.position, monsterPartVisualRef.heavyMuzzle.transform.rotation);

        if (monsterPartVisualRef.heavyDefaultSprayVFXHolder != null)
        {
            monsterPartVisualRef.heavyDefaultSprayVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.heavyMuzzle.transform.position, monsterPartVisualRef.heavyMuzzle.transform.rotation);
        }

        monsterPartVisualRef.heavyHitVFXManager.unleashSpray();
        if (monsterPartVisualRef.heavyDefaultSprayVFXManager)
        {
            monsterPartVisualRef.heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
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
        monsterPartVisualRef.heavyHitVFXManager.updateDamageOnSpray();
        ApplyStatusEffectsToVFXHolder(monsterPartVisualRef.heavyHitVFXManager);
        monsterPartVisualRef.heavyHitVFXManager.updateStatusEffectsOnSpray();
    }
}
