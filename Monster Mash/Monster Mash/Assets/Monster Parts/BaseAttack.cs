using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack
{
    protected int damage = 0;
    protected monsterPartReference heavyColliderReference;
    [SerializeField] protected int baseNeutralAttackDamage = 0;

    public virtual void Init(NewMonsterPart monsterPartRef)
    {
        heavyColliderReference = monsterPartRef.heavyCollider.GetComponent<monsterPartReference>();
    }

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
