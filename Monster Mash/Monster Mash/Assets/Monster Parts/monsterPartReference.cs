using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPartReference : MonoBehaviour
{
    public monsterAttackSystem mainSystem;
    public monsterPart partReference;
    public List<monsterPartReference> referencesToIgnore = new List<monsterPartReference>();
    public bool isHitbox;
    public bool isHurtbox;
    public bool isProjectile;
    public bool isJabOrSlash;
    public bool isReel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<monsterPartReference>() != null)
        {
            if (referencesToIgnore.Contains(other.GetComponent<monsterPartReference>()) == false)
            {
                if (isProjectile && isHitbox)
                {
                    GetComponent<projectile>().impact();
                }

                if (isJabOrSlash && isHitbox)
                {
                    partReference.triggerJabOrSlashHitDetect();
                }

                if (isReel && isHitbox)
                {
                    partReference.triggerReelHitDetect(); //probably pass along monster part reference script of whatever this hits for "grabbing"
                    //print("reel me in, scotty");
                }
            }
        }
    }
}
