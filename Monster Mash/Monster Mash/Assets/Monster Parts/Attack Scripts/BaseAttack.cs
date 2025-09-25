using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementModifier
{
    [field:SerializeField] public Vector2 Left { get; set; }
    [field:SerializeField] public Vector2 Right { get; set; }
    [field:SerializeField] public Vector2 Up { get; set; }
    [field: SerializeField] public Vector2 Down { get; set; }
}

public class TriggerAttackReleaseEventArgs : EventArgs
{
    public MovementModifier MovementModifier { get; set; }
    public float ClipLength { get; set; }

    public TriggerAttackReleaseEventArgs(MovementModifier movementModifier, float clipLength)
    {
        MovementModifier = movementModifier;
        ClipLength = clipLength;
    }
}


public abstract class BaseAttack
{
    public DamageRange DamageRange { get; protected set; } = DamageRange.Range0;
    /// <summary>
    /// The amount of damge the attack deals. The value is automaticaly clamped by the damage range.
    /// </summary>
    [field: SerializeField, DamageRange] public int Damage { get; private set; }
    protected NewMonsterPart monsterPartRef;
    protected MonsterPartVisual monsterPartVisualRef;
    [SerializeField] protected MovementModifier movementModifier;
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
        // Get the attack animation length so that the movement is timed to the animation
        float clipLength = monsterPartRef.myAnimator.GetCurrentAnimatorStateInfo(0).length;

        OnAttackRelease?.Invoke(this, new TriggerAttackReleaseEventArgs(movementModifier, clipLength));
    }

    public virtual void CancelAttack()
    {

    }

    public virtual void TriggerAttack()
    {

    }
}
