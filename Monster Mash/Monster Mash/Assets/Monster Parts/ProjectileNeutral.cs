using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNeutral : NeutralAttack
{
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
}
