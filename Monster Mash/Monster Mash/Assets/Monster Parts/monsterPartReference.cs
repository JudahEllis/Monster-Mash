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
    public bool hasVFX;
    public GameObject[] hitVFX;
    private int hitVFXCount;
    private Transform hitVFXParent;
    private Vector3 VFXPosition;

    public bool isProjectile;

    private void Awake()
    {
        if (hasVFX)
        {
            VFXPosition = hitVFX[0].transform.localPosition;
            hitVFXParent = hitVFX[0].transform.parent;
        }
    }

    public void setUpVFX()
    {
        if (hasVFX)
        {
            VFXPosition = hitVFX[0].transform.localPosition;
            hitVFXParent = hitVFX[0].transform.parent;
        }
    }

    public void resetSprayVFX()
    {
        for (int i = 0; i < hitVFX.Length; i++)
        {
            hitVFX[i].SetActive(false);
            hitVFX[i].transform.parent = hitVFXParent;
            hitVFX[i].transform.localPosition = VFXPosition;
            hitVFX[i].SetActive(true);
        }
    }

    public void unparentSprayVFX()
    {
        for (int i = 0; i < hitVFX.Length; i++)
        {
            hitVFX[i].transform.parent = null;
        }
    }

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

                if (isHitbox && hitVFX.Length != 0)
                {
                    if (hitVFXCount == hitVFX.Length - 1)
                    {
                        hitVFXCount = 0;
                    }
                    else
                    {
                        hitVFXCount++;
                    }

                    hitVFX[hitVFXCount].SetActive(false);
                    hitVFX[hitVFXCount].transform.parent = hitVFXParent;
                    hitVFX[hitVFXCount].transform.localPosition = VFXPosition;
                    hitVFX[hitVFXCount].transform.parent = null;
                    hitVFX[hitVFXCount].SetActive(true);
                }
            }
        }
    }
}
