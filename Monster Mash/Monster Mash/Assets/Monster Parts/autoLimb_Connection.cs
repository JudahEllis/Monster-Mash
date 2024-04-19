using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoLimb_Connection : MonoBehaviour
{
    public Collider triggerBubble;
    public Animator connectedBodyPiece;
    public List<monsterPart> monsterPartMemory = new List<monsterPart>();

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

    public void enableColliders() //rename this for easier outside knowledge
    {
        triggerBubble.enabled = true;
        StartCoroutine(limbConnectionAutoStop());
    }

    IEnumerator limbConnectionAutoStop()
    {
        yield return new WaitForSeconds(1);// I still have to test if this is enough time
        disableColliders();
    }

    public void disableColliders()
    {
        triggerBubble.enabled = false;
    }

    public void clearAllMonsterPartMemory() //when we start anew, we clear all autolimb memory
    {
        monsterPartMemory.Clear();
    }

    public void clearSpecificMonsterPartMemory(monsterPart partToBeRemoved) //if a piece has been picked up/altered at all we clear its memory from autolimbs
    {
        if (monsterPartMemory.Contains(partToBeRemoved))
        {
            monsterPartMemory.Remove(partToBeRemoved);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Connection - Monster Part")
        {
            other.gameObject.transform.parent = this.gameObject.transform;

            if (other.gameObject.GetComponent<monsterPart>() != null)
            {
                monsterPart monsterPartScript = other.gameObject.GetComponent<monsterPart>();

                if (monsterPartMemory.Contains(monsterPartScript) == false) //added this part just so all this info isnt run on all parts every time a piece is added
                {
                    monsterPartMemory.Add(monsterPartScript);
                    monsterPartScript.connectedMonsterPart = connectedBodyPiece;
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
                        if (monsterPartScript.isHorn == false)
                        {
                            monsterPartScript.isLeftSidedLimb = true;
                            monsterPartScript.isRightSidedLimb = false;
                        }
                    }
                    else if (isRightHeadConnection || isRightUpperTorsoConnection || isRightLowerTorsoConnection)
                    {
                        if (monsterPartScript.isHorn == false)
                        {
                            monsterPartScript.isRightSidedLimb = true;
                            monsterPartScript.isLeftSidedLimb = false;
                        }
                    }
                    else
                    {
                        monsterPartScript.isRightSidedLimb = false;
                        monsterPartScript.isLeftSidedLimb = false;
                    }
                }
               
            }
        }
    }
}
