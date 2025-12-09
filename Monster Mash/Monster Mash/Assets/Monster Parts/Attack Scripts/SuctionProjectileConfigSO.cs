using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterPartNameSuctionProjectileConfig", menuName = "ScriptableObjects/Attacks/SuctionProjectileConfigSO")]
public class SuctionProjectileConfigSO : ProjectileConfigSO
{
    [Header("Suction Settings")]
    [SerializeField] private float totalTickTime = 5f;

    protected override NewProjectile CreateProjectile()
    {
        SuctionProjectile projectileInstance = (SuctionProjectile)Instantiate(projectilePrefab);

        projectileInstance.ObjectPool = ObjectPool;
        projectileInstance.TotalTickTime = totalTickTime;
        projectileInstance.Speed = projectileSpeed;
        projectileInstance.Damage = damage;
        projectileInstance.LifeTime = projectileLifetime;
        projectileInstance.PlayerRef = playerRef;

        return projectileInstance;

    }
}
