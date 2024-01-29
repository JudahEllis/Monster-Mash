using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfxHolder : MonoBehaviour
{
    public monsterPartReference[] damageGivingVFX;
    private GameObject[] damageGivingObjects; 
    public List<monsterPartReference> referencesToIgnore = new List<monsterPartReference>();
    public bool usesContainers;

    public void grabReferences()
    {
        List<monsterPartReference> tempVFXList = new List<monsterPartReference>();

        if (usesContainers)
        {
            for (int u = 0; u < transform.childCount; u++)
            {
                for (int i = 0; i < transform.GetChild(u).transform.childCount; i++)
                {
                    tempVFXList.Add(transform.GetChild(u).transform.GetChild(i).GetComponent<monsterPartReference>());
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                tempVFXList.Add(transform.GetChild(i).GetComponent<monsterPartReference>());
            }
        }

        damageGivingVFX = new monsterPartReference[tempVFXList.Count];
        damageGivingObjects = new GameObject[tempVFXList.Count];

        for (int i = 0; i < damageGivingVFX.Length; i++)
        {
            damageGivingVFX[i] = tempVFXList[i];
            damageGivingObjects[i] = tempVFXList[i].gameObject;
        }

    }
    public void collisionOcclusion()
    {
        if (damageGivingVFX.Length > 0)
        {
            for (int i = 0; i < damageGivingVFX.Length; i++)
            {
                damageGivingVFX[i].GetComponent<monsterPartReference>().referencesToIgnore = referencesToIgnore;
            }
        }
    }

    public void rechargeBurstProjectiles(GameObject burstHolder)
    {
        if (usesContainers)
        {
            burstHolder.SetActive(false);

            for (int i = 0; i < damageGivingObjects.Length; i++)
            {
                damageGivingObjects[i].SetActive(true);
            }
        }
    }
}
