using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public float speed = 10f;
    public float timeFromImpactToDisable = 2;
    public float timeUntilAutoDisable = 20;
    private int movementModifier = 1;
    private bool upwardDraftActivated = false;
    public bool needsUpwardDraft;
    public float timeUntilUpwardDraft = 0;
    public float upwardDraftRotationSpeed = 0;
    public Quaternion intendedRotation;
    public GameObject mainVisual;
    public GameObject trailVisual;
    public GameObject impactVisual;
    public Collider baseCollider;
    public Transform projectileHolder;
    private Vector3 startingPoint = new Vector3 (0,0,0);
    private Quaternion startingRotation;
    private bool impactCalled;
    public bool isCenterOfSpray;

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
        upwardDraftActivated = false;
        startingRotation = transform.localRotation;
        if (mainVisual != null)
        {
            mainVisual.SetActive(true);
        }
        if (impactVisual != null)
        {
            impactVisual.SetActive(false);
        }
        if (trailVisual != null)
        {
            trailVisual.SetActive(true);
        }
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
        StartCoroutine(updraftTimer());
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
        upwardDraftActivated = false;
        updateVelocity();
        if (mainVisual != null)
        {
            mainVisual.SetActive(false);
        }
        if (impactVisual != null)
        {
            impactVisual.SetActive(true);
        }
        baseCollider.enabled = false;
        if (trailVisual != null)
        {
            if (trailVisual.GetComponent<ParticleSystem>() != null)
            {
                trailVisual.GetComponent<ParticleSystem>().Stop();
            }
        }
        yield return new WaitForSeconds(timeFromImpactToDisable);
        if (impactVisual != null)
        {
            impactVisual.SetActive(false);
        }
        if (trailVisual != null)
        {
            trailVisual.SetActive(false);
        }
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

    public void Update()
    {
        if (upwardDraftActivated)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, intendedRotation, upwardDraftRotationSpeed * Time.deltaTime);

            if (transform.rotation == intendedRotation)
            {
                upwardDraftActivated = false;
            }

            updateVelocity();
        }
    }

    IEnumerator updraftTimer()
    {
        yield return new WaitForSeconds(timeUntilUpwardDraft);
        if (needsUpwardDraft)
        {
            upwardDraftActivated = true;
        }
    }
}
