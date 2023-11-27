using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchingBag : MonoBehaviour
{
    public Animation rightImpact;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hitbox")
        {
            rightImpact.Stop();
            rightImpact.Play();
        }
    }
}
