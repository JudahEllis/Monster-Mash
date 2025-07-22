using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackReleaseEventArgs : EventArgs
{
    public Vector2 movementModifier { get; set; }

    public TriggerAttackReleaseEventArgs(Vector2 movementModifier)
    {
        this.movementModifier = movementModifier;
    }
}


public class BaseAttack
{
    protected NewMonsterPart monsterPartRef;
    protected MonsterPartVisual monsterPartVisualRef;
    [SerializeField] protected Vector2 movementModifier;
    public static event EventHandler<TriggerAttackReleaseEventArgs> OnAttackRelease;

    /// <summary>
    /// Used to grab public varables from the monster part script
    /// </summary>
    /// <param name="monsterPartRef">Instance of the monster part script</param>
    public virtual void Init(NewMonsterPart monsterPartRef)
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
        monsterPartRef.damage = 0;
    }

    public virtual void statusEffectAndDamageCalculations()
    {

    }

    public virtual void triggerAttackRelease(NewMonsterPart monsterPartRef)
    {
        OnAttackRelease?.Invoke(this, new TriggerAttackReleaseEventArgs(movementModifier));
    }

    public virtual void CancelAttack()
    {

    }

    public virtual void TriggerAttack()
    {

    }
}
