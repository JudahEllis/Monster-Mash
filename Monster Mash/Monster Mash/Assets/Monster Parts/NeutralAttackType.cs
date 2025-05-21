using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeutralAttackType
{
    public enum NeutralAttackTypeEnum 
    {
        Jab,
        Slash,
        Spray,
        Projectile,
        HomingMissile,
        Anvil,
        BowlingBall,
        Gaseous,
        Reflecting,
        Suction,
        AOE,
        Boomerang
    }

    public NeutralAttackTypeEnum attackType;
}
