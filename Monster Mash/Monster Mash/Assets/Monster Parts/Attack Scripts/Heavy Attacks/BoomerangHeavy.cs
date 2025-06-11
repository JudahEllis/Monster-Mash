using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangHeavy : HeavyAttack
{
    public override void SetupVFX()
    {
        if (monsterPartVisualRef.heavyHitVFXHolder != null || monsterPartVisualRef.heavyDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }

        if (monsterPartVisualRef.heavyHitVFXHolder != null)
        {
            monsterPartVisualRef.heavyHitVFXManager.isBoomerangHolder = true;
        }
    }

    public override void triggerHeavyAttackVisuals()
    {
        if (monsterPartVisualRef.heavyAttackHitVFXArray.Length != 0)
        {
            monsterPartVisualRef.heavyHitVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.heavyMuzzle.transform.position, monsterPartVisualRef.heavyMuzzle.transform.rotation);

            monsterPartVisualRef.heavyHitVFXManager.faceRightDirection(monsterPartRef.facingRight);
            monsterPartVisualRef.heavyHitVFXManager.unleashSingleProjectile();

            if (monsterPartVisualRef.heavyDefaultSprayVFXManager)
            {
                monsterPartVisualRef.heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
            }
        }
    }
}
