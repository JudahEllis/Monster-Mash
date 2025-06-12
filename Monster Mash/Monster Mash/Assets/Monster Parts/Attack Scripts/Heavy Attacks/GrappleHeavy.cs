using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHeavy : HeavyAttack
{
    public override void heavyAttackPowerCalculation()
    {
        base.heavyAttackPowerCalculation();

        monsterPartRef.heavyColliderReference.resetAttackHistory();
        monsterPartRef.heavyColliderReference.damage = damage;
        monsterPartRef.heavyColliderReference.markedHeavy = true;

        damageClearance();
    }
}
