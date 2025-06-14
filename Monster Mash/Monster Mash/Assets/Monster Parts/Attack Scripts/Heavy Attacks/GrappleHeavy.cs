using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHeavy : HeavyAttack
{
    public override void heavyAttackPowerCalculation()
    {
        base.heavyAttackPowerCalculation();

        monsterPartRef.heavyColliderReference.resetAttackHistory();
        monsterPartRef.heavyColliderReference.damage = monsterPartRef.damage;
        monsterPartRef.heavyColliderReference.markedHeavy = true;

        damageClearance();
    }
}
