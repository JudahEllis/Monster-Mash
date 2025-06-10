using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHeavy : HeavyAttack
{
    public override void endRemainingVFX()
    {
        heavyHitVFXManager.endBeamVisual();
    }
}
