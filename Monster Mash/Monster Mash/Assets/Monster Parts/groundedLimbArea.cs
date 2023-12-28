using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundedLimbArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<monsterPartReference>() != null)
        {
            if (other.GetComponent<monsterPartReference>().partReference)
            {
                monsterPart partOnGround = other.GetComponent<monsterPartReference>().partReference;
                partOnGround.isGroundedLimb = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<monsterPartReference>() != null)
        {
            if (other.GetComponent<monsterPartReference>().partReference)
            {
                monsterPart partOnGround = other.GetComponent<monsterPartReference>().partReference;
                partOnGround.isGroundedLimb = false;
            }
        }
    }
}
