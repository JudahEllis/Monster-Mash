using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHeavy : HeavyAttack
{
    public override void SetupVFX()
    {
        if (heavyHitVFXHolder != null || heavyDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }
    }

    public override void triggerHeavyAttackVisuals()
    {
        if (heavyAttackHitVFXArray.Length != 0)
        {
            heavyHitVFXHolder.transform.SetPositionAndRotation(heavyMuzzle.transform.position, heavyMuzzle.transform.rotation);

            heavyHitVFXManager.faceRightDirection(facingRight);
            heavyHitVFXManager.unleashSingleProjectile();

            if (heavyDefaultSprayVFXManager)
            {
                heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
            }
        }
    }
}
