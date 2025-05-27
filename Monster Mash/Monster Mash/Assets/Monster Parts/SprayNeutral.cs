using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayNeutral : NeutralAttack
{
    public override void neutralAttackPowerCalculation()
    {
        base.neutralAttackPowerCalculation();
        neutralHitVFXManager.damage = damage;
        neutralHitVFXManager.updateDamageOnProjectiles();

        damageClearance();
    }
}
