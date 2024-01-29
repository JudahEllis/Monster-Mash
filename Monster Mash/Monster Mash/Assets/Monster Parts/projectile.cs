using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public float speed = 10f;
    public float timeFromImpactToDisable = 2;
    public float timeUntilAutoDisable = 20;
    private int movementModifier = 1;
    public GameObject mainVisual;
    public GameObject trailVisual;
    public GameObject impactVisual;
    public Collider baseCollider;
    public Transform projectileHolder;
    private Vector3 startingPoint = new Vector3 (0,0,0);
    private Quaternion startingRotation;
    private bool impactCalled;

    public void resetPosition()
    {
        transform.parent = projectileHolder;
        transform.localPosition = startingPoint;
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localRotation = startingRotation;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        movementModifier = 1;
        impactCalled = false;
        startingRotation = transform.localRotation;
        impactVisual.SetActive(false);
        mainVisual.SetActive(true);
        trailVisual.SetActive(true);
        baseCollider.enabled = true;
        /*
        if (trailVisual.GetComponent<ParticleSystem>() != null)
        {
            trailVisual.GetComponent<ParticleSystem>().Play();
        }
        */
        resetPosition();
        updateVelocity();
        StartCoroutine(autoDisable());
    }

    private void updateVelocity()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().velocity = this.transform.forward * speed * movementModifier;
    }
    public void impact()
    {
        impactCalled = true;
        StartCoroutine(impactEffect());
    }

    IEnumerator impactEffect()
    {
        movementModifier = 0;
        updateVelocity();
        mainVisual.SetActive(false);
        impactVisual.SetActive(true);
        baseCollider.enabled = false;
        if (trailVisual.GetComponent<ParticleSystem>() != null)
        {
            trailVisual.GetComponent<ParticleSystem>().Stop();
        }
        yield return new WaitForSeconds(timeFromImpactToDisable);
        impactVisual.SetActive(false);
        trailVisual.SetActive(false);
        resetPosition();
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }

    IEnumerator autoDisable()
    {
        yield return new WaitForSeconds(timeUntilAutoDisable);
        if (impactCalled == false)
        {
            impact();
        }
    }
}
