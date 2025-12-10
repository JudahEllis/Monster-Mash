using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionProjectile : NewProjectile
{
    public float TotalTickTime { set => totalTickTime = value; }

    private Vector3 contactPoint;
    private Vector3 originalScale;
    private float totalTickTime;
    private Coroutine tickCoroutine;

    protected override void OnPlayerHit(Collider other, playerController player)
    {
        contactPoint = other.ClosestPointOnBounds(transform.position);
        originalScale = transform.localScale;

        transform.SetParent(other.transform);
        tickCoroutine ??= StartCoroutine(TickDamage(player));
    }

    private IEnumerator TickDamage(playerController target)
    {
        float tickDelay = 1f;
        float elapsed = 0;

        while (elapsed < totalTickTime)
        {
            elapsed += Time.deltaTime;
            target.damaged(damage, false, transform.position, contactPoint);
            yield return new WaitForSeconds(tickDelay);
        }

        if (elapsed >= totalTickTime)
        {
            transform.SetParent(null);
            transform.localScale = originalScale;
            tickCoroutine = null;
            DeactivateProjectile();
        }
    }
}
