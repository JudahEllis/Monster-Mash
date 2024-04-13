using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class punchingBag : MonoBehaviour
{
    public Animation rightImpact;
    public Vector3 startingPosition;
    public Quaternion startingRotation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hitbox")
        {
            rightImpact.Stop();
            rightImpact.Play();
        }
    }

    private void OnEnable()
    {
        transform.position = startingPosition;
        transform.localRotation = startingRotation;
    }
}
