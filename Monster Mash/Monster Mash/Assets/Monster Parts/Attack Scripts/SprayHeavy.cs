using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayHeavy : HeavyAttack
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
        heavyHitVFXHolder.transform.SetPositionAndRotation(heavyMuzzle.transform.position, heavyMuzzle.transform.rotation);

        if (heavyDefaultSprayVFXHolder != null)
        {
            heavyDefaultSprayVFXHolder.transform.SetPositionAndRotation(heavyMuzzle.transform.position, heavyMuzzle.transform.rotation);
        }

        heavyHitVFXManager.unleashSpray();
        if (heavyDefaultSprayVFXManager)
        {
            heavyDefaultSprayVFXManager.unleashAdditionalSprayVisual();
        }
    }
}
