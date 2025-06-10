using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack
{
    protected int damage = 0;
    protected monsterPartReference heavyColliderReference;
    protected bool jabOrSlashLanded;
    protected bool facingRight;
    protected Transform partTransform;
    [SerializeField] protected int baseNeutralAttackDamage = 0;

    /// <summary>
    /// Used to grab public varables from the monster part script
    /// </summary>
    /// <param name="monsterPartRef">Instance of the monster part script</param>
    public virtual void Init(NewMonsterPart monsterPartRef)
    {
        jabOrSlashLanded = monsterPartRef.jabOrSlashLanded;
        facingRight = monsterPartRef.facingRight;
        partTransform = monsterPartRef.transform;
        heavyColliderReference = monsterPartRef.heavyCollider.GetComponent<monsterPartReference>();
    }

    /// <summary>
    /// Used to grab public varables from MonsterPartVisual
    /// </summary>
    /// <param name="monsterPartVisual">Instance of MonsterPartVisual</param>
    public virtual void Init(MonsterPartVisual monsterPartVisual)
    {

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
