using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NewProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public IObjectPool<NewProjectile> ObjectPool { set => objectPool = value; }
    public float Speed { set => speed = value; }

    private IObjectPool<NewProjectile> objectPool;
    private float speed;
    private bool isReleased;

    public void Fire()
    {
        rb.AddForce(transform.forward *  speed, ForceMode.Acceleration);
    }


    private void OnCollisionEnter(Collision collision)
    {
        /*if (isReleased) { return; }
        

        isReleased = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        objectPool.Release(this);*/
    }
}
