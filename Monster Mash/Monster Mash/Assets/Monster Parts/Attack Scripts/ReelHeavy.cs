using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHeavy : HeavyAttack
{
    private int reelAttackBuiltUpPower = 0;
    private int reelAttackCurrentThreshold = 0;
    private bool reelAttackLanded;

    public override void Init(NewMonsterPart monsterPartRef)
    {
        base.Init(monsterPartRef);
        reelAttackBuiltUpPower = monsterPartRef.reelAttackBuiltUpPower;
        reelAttackCurrentThreshold = monsterPartRef.reelAttackCurrentThreshold;
        reelAttackLanded = monsterPartRef.reelAttackLanded;
    }

    public override void triggerHeavyAttackVisuals()
    {
        // finish fixing this
        /*if (!reelAttackLanded)
        {
            //miss visual
            triggerReelCollisionsOff();
        }

        reelAttackBuiltUpPower = 0;
        reelAttackCurrentThreshold = 0;
        powerUpCheckAllowed = false;*/
    }
}
