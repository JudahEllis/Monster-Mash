using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashNeutral : NeutralAttack
{
    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        Attack = AttackType.Slash;
    }

    public override void neutralAttackPowerCalculation()
    {
        base.neutralAttackPowerCalculation();
        monsterPartRef.neutralColliderReference.resetAttackHistory();
        monsterPartRef.neutralColliderReference.damage = monsterPartRef.damage;
        monsterPartRef.heavyColliderReference.markedHeavy = false;

        damageClearance();
    }

    public override void statusEffectAndDamageCalculations()
    {
        if (monsterPartRef.neutralColliderReference == null) { return; }
        monsterPartRef.neutralColliderReference.damage = monsterPartRef.baseNeutralAttackDamage;
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
