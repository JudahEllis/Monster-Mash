using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundedLimbArea : MonoBehaviour
{
    private List<NewMonsterPart> allParts;

    private List<NewMonsterPart> GetAllPartsInRoot()
    {
        var root = transform.root;
        var allParts = new List<NewMonsterPart>(root.GetComponentsInChildren<NewMonsterPart>(true));
        return allParts;
    }

    private void Start()
    {
        allParts = GetAllPartsInRoot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Solid") || collision.CompareTag("Semi Solid"))
        {
            foreach (var part in allParts)
            {
                part.isInGroundedArea = true;
                part.OnLandedDuringAttack();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Solid") || collision.CompareTag("Semi Solid"))
        {
            foreach (var part in allParts)
            {
                part.isInGroundedArea = false;
            }
        }
    }
}