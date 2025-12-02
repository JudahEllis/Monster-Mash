using UnityEngine;
using UnityEngine.Pool;

public class ProjectileNeutral : NeutralAttack
{

    [SerializeField] private NewProjectile projectilePrefab;
    [SerializeField] private Transform projectileMuzzle;
    [SerializeField] private float projectileSpeed;

    private IObjectPool<NewProjectile> objectPool;

    public ProjectileNeutral()
    {
        DamageRange = DamageRange.Range2;
    }

    public override void Init(NewMonsterPart monsterPartRef, MonsterPartVisual monsterPartVisualRef)
    {
        base.Init(monsterPartRef, monsterPartVisualRef);
        SetupPool();
    }

    private void SetupPool()
    {
        // ?? is compound assignment syntax which checks if the refrence is null before assigning it.
        // OnAwakenTheBeast might be invoked multiple times and we dont want to waste resources overwriting the pool each time
        objectPool ??= new ObjectPool<NewProjectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    private NewProjectile CreateProjectile()
    {
        NewProjectile projectileInstance = Instantiate(projectilePrefab, projectileMuzzle.transform.position, Quaternion.identity, projectileMuzzle);
        projectileInstance.ObjectPool = objectPool;
        projectileInstance.Speed = projectileSpeed;
        return projectileInstance;
    }

    private void OnGetFromPool(NewProjectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
        pooledObject.transform.SetParent(null, true);
    }

    private void OnReleaseToPool(NewProjectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.SetParent(projectileMuzzle);
    }

    private void OnDestroyPooledObject(NewProjectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    public override void triggerAttackRelease()
    {
        base.triggerAttackRelease();
        NewProjectile projectile = objectPool.Get();
        projectile.Fire();
    }

    public override void PassDamage()
    {
        if (monsterPartVisualRef.neutralHitVFXManager == null) { return; }
        monsterPartVisualRef.neutralHitVFXManager.damage = Damage;
    }

    public override void statusEffectAndDamageCalculations()
    {
        if (monsterPartVisualRef.neutralHitVFXManager == null) { return; }
        monsterPartVisualRef.neutralHitVFXManager.damage = Damage;
        monsterPartVisualRef.neutralHitVFXManager.updateDamageOnSpray();
    }

    public override void SetupVFX()
    {
        if (monsterPartVisualRef.neutralHitVFXHolder != null || monsterPartVisualRef.neutralDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }
    }
}
