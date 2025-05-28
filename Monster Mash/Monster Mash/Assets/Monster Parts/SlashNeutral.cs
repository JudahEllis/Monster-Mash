using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashNeutral : NeutralAttack
{
    public override void neutralAttackPowerCalculation()
    {
        base.neutralAttackPowerCalculation();
        neutralColliderReference.resetAttackHistory();
        neutralColliderReference.damage = damage;
        heavyColliderReference.markedHeavy = false;

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        base.statusEffectAndDamageCalculations();
        neutralColliderReference.damage = baseNeutralAttackDamage;
    }

    public override void triggerAttackRelease(NewMonsterPart monsterPartRef)
    {
        monsterPartRef.triggerJabOrSlashCollisionsOn();
    }
}
