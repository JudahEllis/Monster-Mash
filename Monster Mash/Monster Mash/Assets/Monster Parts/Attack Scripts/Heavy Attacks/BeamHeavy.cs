using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHeavy : HeavyAttack
{
    public override void triggerHeavyAttackVisuals()
    {
        monsterPartVisualRef.heavyHitVFXManager.unleashBeamVisual();
    }
    public override void endRemainingVFX()
    {
        monsterPartVisualRef.heavyHitVFXManager.endBeamVisual();
    }
}
