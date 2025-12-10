using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "MonsterPartNameProjectileConfig", menuName = "ScriptableObjects/Attacks/ProjectileConfigSO")]
public class ProjectileConfigSO : ScriptableObject
{
    [SerializeField] protected NewProjectile projectilePrefab;
    [SerializeField] protected float projectileSpeed = 20;
    [SerializeField] protected float projectileLifetime = 10;

    [Header("Object Pool Settings")]
    [SerializeField] protected int poolStartSize = 10;
    [SerializeField] protected int poolMaxSize = 100;
    [SerializeField] protected bool preWarm = true;

    public IObjectPool<NewProjectile> ObjectPool { get; private set; }

    protected int damage;
    protected playerController playerRef;
    private Transform projectileMuzzle;
    private NewMonsterPart partRef;
    

    public void SetupPool(ProjectileRefrenceData data)
    {
        damage = data.Damage;
        playerRef = data.PlayerRef;
        partRef = data.MonsterPartRef;
        projectileMuzzle = data.ProjectileMuzzle;

        // ?? is compound assignment syntax which checks if the refrence is null before assigning it.
        // OnAwakenTheBeast might be invoked multiple times and we dont want to waste resources overwriting the pool each time
        ObjectPool ??= new ObjectPool<NewProjectile>(CreateProjectile,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, false, poolStartSize, poolMaxSize);

        if (preWarm)
        {
            PreWarm();
        }
    }

    private void PreWarm()
    {
        List<NewProjectile> projectiles = new();
       
        for (int i = 0; i < poolStartSize; i++)
        {
            projectiles.Add(ObjectPool.Get());
        }

        foreach (var projectile in projectiles)
        {
            ObjectPool.Release(projectile);
        }
    }

    protected virtual NewProjectile CreateProjectile()
    {
        NewProjectile projectileInstance = Instantiate(projectilePrefab);
        SetupProjectile(projectileInstance);

        return projectileInstance;
    }

    protected virtual void SetupProjectile(NewProjectile projectileInstance)
    {
        projectileInstance.ObjectPool = ObjectPool;
        projectileInstance.Speed = projectileSpeed;
        projectileInstance.Damage = damage;
        projectileInstance.LifeTime = projectileLifetime;
        projectileInstance.PlayerRef = playerRef;
    }

    protected virtual void OnGetFromPool(NewProjectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
        pooledObject.transform.position = projectileMuzzle.transform.position;
        Quaternion rotation = CalculateSpawnRotation();
        pooledObject.transform.rotation = rotation;
    }

    private Quaternion CalculateSpawnRotation()
    {
        Quaternion rotation = Quaternion.identity;

        switch (playerRef.LastInputDirection)
        {
            case playerController.InputDirection.Forward:
            case playerController.InputDirection.Backward:
                rotation = Quaternion.LookRotation(partRef.transform.forward);

                // tails fire backwards so we need to reverse the rotation
                if (partRef.PartType is MonsterPartType.Tail)
                {
                    rotation = Quaternion.Inverse(rotation);
                }

                break;
            // The monster parts can have slight up and down rotations so we dont want to rely on the up/down vectors of the parts
            case playerController.InputDirection.Up:
                rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                break;
            case playerController.InputDirection.Down:
                rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                break;
        }

        return rotation;
    }

    protected virtual void OnReleaseToPool(NewProjectile pooledObject)
    {
        pooledObject.IsReleased = false;
        pooledObject.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyPooledObject(NewProjectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
