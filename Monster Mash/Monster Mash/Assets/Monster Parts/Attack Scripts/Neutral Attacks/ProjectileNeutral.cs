using UnityEngine;
using UnityEngine.Pool;

public class ProjectileNeutral : NeutralAttack
{
    [SerializeField] protected ProjectileConfigSO projectileConfig;
    [SerializeField] protected Transform projectileMuzzle;

    public ProjectileNeutral()
    {
        DamageRange = DamageRange.Range2;
    }

    public override void Init(NewMonsterPart monsterPartRef, MonsterPartVisual monsterPartVisualRef)
    {
        base.Init(monsterPartRef, monsterPartVisualRef);
        projectileConfig.SetupPool(Damage, monsterPartRef.myMainSystem.myPlayer, projectileMuzzle);
    }

    public override void TriggerAttackRelease()
    {
        base.TriggerAttackRelease();
        NewProjectile projectile = projectileConfig.ObjectPool.Get();
        projectile.Fire();
    }
}
