using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoLimb_Connection : MonoBehaviour
{
    public Collider triggerBubble;

    [Header("Head Connection Data")]
    public bool isLeftHeadConnection = false;
    public bool isRightHeadConnection = false;
    public bool isFaceConnection = false;
    public bool isTopHeadConnection = false;
    public bool isBackHeadConnection = false;

    [Header("Torso Connection Data")]
    public bool isLeftUpperTorsoConnection = false;
    public bool isRightUpperTorsoConnection = false;
    public bool isNeckTorsoConnection = false;
    public bool isLeftLowerTorsoConnection = false;
    public bool isRightLowerTorsoConnection = false;
    public bool isTailTorsoConnection = false;
    public bool isShoulderBladeTorsoConnection = false;
    public bool isChestTorsoConnection = false;
    public bool isBellyTorsoConnection = false;

    public void enableColliders()
    {
        triggerBubble.enabled = true;
        StartCoroutine(limbConnectionAutoStop());
    }

    IEnumerator limbConnectionAutoStop()
    {
        yield return new WaitForEndOfFrame();// I still have to test if this is enough time
        disableColliders();
    }

    public void disableColliders()
    {
        triggerBubble.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Connection - Monster Part")
        {
            other.gameObject.transform.parent = this.gameObject.transform;

            if (other.gameObject.GetComponent<monsterPart>() != null)
            {
                monsterPart monsterPartScript = other.gameObject.GetComponent<monsterPart>();
                monsterPartScript.isJointed = true;
                monsterPartScript.isLeftEarLimb = isLeftHeadConnection;
                monsterPartScript.isRightEarLimb = isRightHeadConnection;
                monsterPartScript.isFacialLimb = isFaceConnection;
                monsterPartScript.isTopHeadLimb = isTopHeadConnection;
                monsterPartScript.isBacksideHeadLimb = isBackHeadConnection;
                monsterPartScript.isLeftShoudlerLimb = isLeftUpperTorsoConnection;
                monsterPartScript.isRightShoulderLimb = isRightUpperTorsoConnection;
                monsterPartScript.isNeckLimb = isNeckTorsoConnection;
                monsterPartScript.isLeftPelvisLimb = isLeftLowerTorsoConnection;
                monsterPartScript.isRightPelvisLimb = isRightLowerTorsoConnection;
                monsterPartScript.isTailLimb = isTailTorsoConnection;
                monsterPartScript.isShoulderBladeLimb = isShoulderBladeTorsoConnection;
                monsterPartScript.isChestLimb = isChestTorsoConnection;
                monsterPartScript.isBellyLimb = isBellyTorsoConnection;

                //this section may be removed at a later date if we decide to separate left and right limbs as separate limbs and deserving of a hardcoded orientation
                //Dont forget!!! Horns, Eyes, and Mouths need to be excluded from this grouping 

                if (isLeftHeadConnection || isLeftUpperTorsoConnection || isLeftLowerTorsoConnection)
                {
                    monsterPartScript.isLeftSidedLimb = true;
                    monsterPartScript.isRightSidedLimb = false;
                }
                if (isRightHeadConnection || isRightUpperTorsoConnection || isRightLowerTorsoConnection)
                {
                    monsterPartScript.isRightSidedLimb = true;
                    monsterPartScript.isLeftSidedLimb = false;
                }
            }
        }
    }
}
