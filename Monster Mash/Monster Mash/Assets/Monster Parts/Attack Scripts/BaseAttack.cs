using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack
{
    protected int damage = 0;
    [SerializeField] protected int baseNeutralAttackDamage = 0;
    protected NewMonsterPart monsterPartRef;
    protected MonsterPartVisual monsterPartVisualRef;

    /// <summary>
    /// Used to grab public varables from the monster part script
    /// </summary>
    /// <param name="monsterPartRef">Instance of the monster part script</param>
    public void Init(NewMonsterPart monsterPartRef)
    {
        this.monsterPartRef = monsterPartRef;
    }

    /// <summary>
    /// Used to grab public varables from MonsterPartVisual
    /// </summary>
    /// <param name="monsterPartVisualRef">Instance of MonsterPartVisual</param>
    public void Init(MonsterPartVisual monsterPartVisualRef)
    {
        this.monsterPartVisualRef = monsterPartVisualRef;
    }

    protected void damageClearance()
    {
        damage = 0;
    }

    public virtual void statusEffectAndDamageCalculations()
    {

    }

    public virtual void triggerAttackRelease(NewMonsterPart monsterPartRef)
    {

    }
}
