using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHeavy : HeavyAttack
{
    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        Attack = HeavyAttackType.Beam;
    }
    public override void triggerHeavyAttackVisuals()
    {
        monsterPartVisualRef.heavyHitVFXManager.unleashBeamVisual();
    }
    public override void endRemainingVFX()
    {
        monsterPartVisualRef.heavyHitVFXManager.endBeamVisual();
    }

    public override void triggerHeavyAttackPowerUp()
    {
        monsterPartRef.builtUpAttackPower++;
    }

    public override void TriggerAttack()
    {
        monsterPartRef.heavyAttackInMotion = true;
    }
    public override void CancelAttack()
    {
        //end functions + turn off visuals
        monsterPartVisualRef.triggerAttackToIdle();
        monsterPartRef.StartCoroutine(beamAttackDelay());
    }

    IEnumerator beamAttackDelay()
    {
        yield return new WaitForSeconds(0.2f);
        monsterPartRef.triggerAttackCorrections(); //delaying this to allow the body time to unbrace
    }
}
