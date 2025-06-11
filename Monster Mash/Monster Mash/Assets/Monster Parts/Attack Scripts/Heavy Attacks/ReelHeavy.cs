using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHeavy : HeavyAttack
{
    public override void triggerHeavyAttackVisuals()
    {
        if (!monsterPartRef.reelAttackLanded)
        {
            //miss visual
            triggerReelCollisionsOff();
        }

        monsterPartRef.reelAttackBuiltUpPower = 0;
        monsterPartRef.reelAttackCurrentThreshold = 0;
        monsterPartRef.powerUpCheckAllowed = false;
    }

    public void triggerReelCollisionsOff() //called in attack animation
    {
        //turn off neutral vfx holder
        monsterPartRef.reelAttackLanded = false;
        monsterPartRef.reelAttackBuiltUpPower = 0;
        monsterPartRef.reelAttackCurrentThreshold = 0;
        monsterPartRef.powerUpCheckAllowed = false;

        if (monsterPartRef.attackMarkedHeavy == true)
        {
            monsterPartRef.heavyCollider.enabled = false;
        }
    }
}
