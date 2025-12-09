using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NewProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public IObjectPool<NewProjectile> ObjectPool { set => objectPool = value; }
    public float Speed { set => speed = value; }
    public int Damage { set => damage = value; }
    public float LifeTime { set => lifeTime = value; }
    public bool IsReleased {  set => isReleased = value; }
    public playerController PlayerRef { set => playerRef = value; }

    protected IObjectPool<NewProjectile> objectPool;
    protected float speed;
    protected int damage;
    protected float lifeTime;
    protected bool isReleased;
    protected playerController playerRef;

    public virtual void Fire()
    {
        rb.AddForce(transform.forward *  speed, ForceMode.VelocityChange);

        // Safety measure incase the projectile escapes the stage
        StartCoroutine(DelayedDeactivate(lifeTime));
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        playerController potentialPlayer = other.GetComponentInParent<playerController>();

        if (other.CompareTag("Solid"))
        {
            DeactivateProjectile();
        }
        else if (potentialPlayer != null && potentialPlayer != playerRef)
        {
            potentialPlayer.damaged(damage, false, transform.position, other.ClosestPointOnBounds(transform.position));
            DeactivateProjectile();
        }
    }

    protected virtual IEnumerator DelayedDeactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateProjectile();
    }

    protected virtual void DeactivateProjectile()
    {
        if (isReleased) { return; }
        isReleased = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        objectPool.Release(this);
    }
}
