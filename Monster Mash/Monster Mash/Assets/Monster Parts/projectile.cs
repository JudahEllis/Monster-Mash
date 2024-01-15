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
    public GameObject impactVisual;

    private void OnEnable()
    {
        StopAllCoroutines();
        movementModifier = 1;
        impactVisual.SetActive(false);
        mainVisual.SetActive(true);
        updateVelocity();
        StartCoroutine(autoDisable());
    }

    private void updateVelocity()
    {
        GetComponent<Rigidbody>().velocity = this.transform.forward * speed * movementModifier;
    }
    public void impact()
    {
        StartCoroutine(impactEffect());
    }

    IEnumerator impactEffect()
    {
        movementModifier = 0;
        updateVelocity();
        mainVisual.SetActive(false);
        impactVisual.SetActive(true);
        yield return new WaitForSeconds(timeFromImpactToDisable);
        impactVisual.SetActive(false);
        this.gameObject.SetActive(false);
    }

    IEnumerator autoDisable()
    {
        yield return new WaitForSeconds(timeUntilAutoDisable);
        movementModifier = 0;
        updateVelocity();
        this.gameObject.SetActive(false);
    }
}
