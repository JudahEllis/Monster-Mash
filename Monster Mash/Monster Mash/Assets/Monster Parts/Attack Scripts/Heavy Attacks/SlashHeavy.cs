using System;

[Serializable]
public class SlashHeavy : HeavyAttack
{
    public override void triggerHeavyAttackVisuals()
    {
        if (monsterPartRef.jabOrSlashLanded == false && monsterPartVisualRef.heavyMissVFXHolder != null)
        {
            //turn on miss visual if neutral vfx holder's script hasn't made contact
            monsterPartVisualRef.heavyMissVFXManager.unleashJabOrSlash();
        }
    }
    public override void heavyAttackPowerCalculation()
    {
        base.heavyAttackPowerCalculation();

        monsterPartRef.heavyColliderReference.resetAttackHistory();
        monsterPartRef.heavyColliderReference.damage = monsterPartRef.damage;
        monsterPartRef.heavyColliderReference.markedHeavy = true;

        damageClearance();
    }
    public override void triggerAttackRelease(NewMonsterPart monsterPartRef)
    {
        base.triggerAttackRelease(monsterPartRef);
        monsterPartRef.triggerJabOrSlashCollisionsOn();
    }

    public override void statusEffectAndDamageCalculations()
    {
        ApplyStatusEffectsToColliderReference(monsterPartRef.heavyColliderReference);
    }
}
