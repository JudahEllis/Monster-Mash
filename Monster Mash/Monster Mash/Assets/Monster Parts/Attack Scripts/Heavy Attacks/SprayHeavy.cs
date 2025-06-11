using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayHeavy : HeavyAttack
{
    public override void SetupVFX()
    {
        if (monsterPartVisualRef.heavyHitVFXHolder != null || monsterPartVisualRef.heavyDefaultSprayVFXHolder != null)
        {
            StoredParentSetup();
        }
    }

    public override void triggerHeavyAttackVisuals()
    {
        monsterPartVisualRef.heavyHitVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.heavyMuzzle.transform.position, monsterPartVisualRef.heavyMuzzle.transform.rotation);

        if (monsterPartVisualRef.heavyDefaultSprayVFXHolder != null)
        {
            monsterPartVisualRef.heavyDefaultSprayVFXHolder.transform.SetPositionAndRotation(monsterPartVisualRef.heavyMuzzle.transform.position, monsterPartVisualRef.heavyMuzzle.transform.rotation);
        }

        monsterPartVisualRef.heavyHitVFXManager.unleashSpray();
        if (monsterPartVisualRef.heavyDefaultSprayVFXManager)
        {
            monsterPartVisualRef.heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
        }
    }
}
