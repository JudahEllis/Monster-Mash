using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class punchingBag : MonoBehaviour
{
    public Animation impactAnimation;
    public AnimationClip rightImpact;
    public AnimationClip leftImpact;
    public AnimationClip topImpact;
    public Vector3 startingPosition;
    public Quaternion startingRotation;
    public Transform topOfPunchingBag;
    public bool staticPunchingBag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hitbox")
        {
            if (other.transform.position.x < this.transform.position.x && (other.transform.position.y < topOfPunchingBag.position.y)) //punching from the left
            {
                if (impactAnimation != null)
                {
                    impactAnimation.Stop();
                    impactAnimation.clip = leftImpact;
                    impactAnimation.Play();
                }
            }
            else if (other.transform.position.x > this.transform.position.x && (other.transform.position.y < topOfPunchingBag.position.y)) //punching from the right
            {
                if (impactAnimation != null)
                {
                    impactAnimation.Stop();
                    impactAnimation.clip = rightImpact;
                    impactAnimation.Play();
                }
            }
            else
            {
                //currently includes both top punches and bottom punches
                if (impactAnimation != null)
                {
                    impactAnimation.Stop();
                    impactAnimation.clip = topImpact;
                    impactAnimation.Play();
                }
            }

            //rightImpact.Stop();
            //rightImpact.Play();
        }
    }

    private void OnEnable()
    {
        if (staticPunchingBag == false)
        {
            transform.position = startingPosition;
            transform.localRotation = startingRotation;
        }
    }
}
