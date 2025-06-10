using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JabHeavy : HeavyAttack
{
    public override void triggerHeavyAttackVisuals()
    {
        if (jabOrSlashLanded == false && heavyMissVFXHolder != null)
        {
            //turn on miss visual if neutral vfx holder's script hasn't made contact
            heavyMissVFXManager.unleashJabOrSlash();
        }
    }
}
