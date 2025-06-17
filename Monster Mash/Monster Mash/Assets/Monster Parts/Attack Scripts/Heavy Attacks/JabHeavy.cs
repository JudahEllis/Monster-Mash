using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JabHeavy : HeavyAttack
{
    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        Attack = HeavyAttackType.Jab;
    }
    public override void triggerHeavyAttackVisuals()
    {
        if (monsterPartRef.jabOrSlashLanded == false && monsterPartVisualRef.heavyMissVFXHolder != null)
        {
            //turn on miss visual if neutral vfx holder's script hasn't made contact
            monsterPartVisualRef.heavyMissVFXManager.unleashJabOrSlash();
        }
    }

    public override void triggerAttackRelease(NewMonsterPart monsterPartRef)
    {
        monsterPartRef.triggerJabOrSlashCollisionsOn();
    }

    public override void heavyAttackPowerCalculation()
    {
        base.heavyAttackPowerCalculation();

        monsterPartRef.heavyColliderReference.resetAttackHistory();
        monsterPartRef.heavyColliderReference.damage = monsterPartRef.damage;
        monsterPartRef.heavyColliderReference.markedHeavy = true;

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        ApplyStatusEffectsToColliderReference(monsterPartRef.heavyColliderReference);
    }
}
