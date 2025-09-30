using System;

[Serializable]
public class GrappleHeavy : HeavyAttack
{
    public GrappleHeavy()
    {
        Attack = HeavyAttackType.Grapple;
        DamageRange = DamageRange.Range2;
    }
    public override void heavyAttackPowerCalculation()
    {
        base.heavyAttackPowerCalculation();

        monsterPartRef.heavyColliderReference.resetAttackHistory();
        monsterPartRef.heavyColliderReference.damage = Damage;
        monsterPartRef.heavyColliderReference.markedHeavy = true;

        damageClearance();
    }
}
