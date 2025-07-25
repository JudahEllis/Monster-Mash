using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackReleaseEventArgs : EventArgs
{
    public Vector2 MovementModifier { get; set; }
    public float ClipLength { get; set; }

    public TriggerAttackReleaseEventArgs(Vector2 movementModifier, float clipLength)
    {
        MovementModifier = movementModifier;
        ClipLength = clipLength;
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
