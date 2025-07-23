using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackReleaseEventArgs : EventArgs
{
    public Vector2 MovementModifier { get; set; }

    public TriggerAttackReleaseEventArgs(Vector2 movementModifier)
    {
        MovementModifier = movementModifier;
    }
}


public abstract class BaseAttack
{
    protected NewMonsterPart monsterPartRef;
    protected MonsterPartVisual monsterPartVisualRef;
    [SerializeField] protected Vector2 movementModifier;
    public event EventHandler<TriggerAttackReleaseEventArgs> OnAttackRelease;

    /// <summary>
    /// Used to grab public varables from the monster part script
    /// </summary>
    /// <param name="monsterPartRef">Instance of the monster part script</param>
    public virtual void Init(NewMonsterPart monsterPartRef, MonsterPartVisual monsterPartVisualRef)
    {
        this.monsterPartRef = monsterPartRef;
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
