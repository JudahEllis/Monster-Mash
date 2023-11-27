using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterPartReference : MonoBehaviour
{
    public monsterAttackSystem mainSystem;
    public monsterPart partReference;
    public bool isHitbox;
    public bool isHurtbox;
    public bool hasVFX;
    public GameObject[] hitVFX;
    private int hitVFXCount;
    private Transform hitVFXParent;
    private Vector3 VFXPosition;

    private void Awake()
    {
        if (hasVFX)
        {
            VFXPosition = hitVFX[0].transform.localPosition;
            hitVFXParent = hitVFX[0].transform.parent;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHitbox && hitVFX != null)
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
