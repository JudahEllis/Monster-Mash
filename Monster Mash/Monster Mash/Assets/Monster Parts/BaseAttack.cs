using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack
{
    // TODO: Remember to add the functions that assign these varables
    protected int damage = 0;
    protected monsterPartReference heavyColliderReference;
    [SerializeField] protected int baseNeutralAttackDamage = 0;

    protected void damageClearance()
    {
        damage = 0;
    }
}
