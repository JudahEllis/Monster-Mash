using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JabNeutral : NeutralAttack
{
    
    public override void neutralAttackPowerCalculation()
    {
        base.neutralAttackPowerCalculation();
        monsterPartRef.neutralColliderReference.resetAttackHistory();
        monsterPartRef.neutralColliderReference.damage = damage;
        monsterPartRef.heavyColliderReference.markedHeavy = false;

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        base.statusEffectAndDamageCalculations();
        monsterPartRef.neutralColliderReference.damage = baseNeutralAttackDamage;
    }

    public override void triggerAttackRelease(NewMonsterPart monsterPartRef)
    {
        monsterPartRef.triggerJabOrSlashCollisionsOn();
    }

    public override void triggerNeutralAttackVisuals()
    {
        if (!monsterPartRef.jabOrSlashLanded && monsterPartVisualRef.neutralMissVFXHolder != null)
        {
            monsterPartVisualRef.neutralMissVFXManager.unleashJabOrSlash();
        }
    }
}
