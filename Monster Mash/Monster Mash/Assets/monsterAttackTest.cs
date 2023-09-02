using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterAttackTest : MonoBehaviour
{
    public bool facingRight = false;
    public bool isGrounded = true;

    //ATTACK SLOT OBJECTS
    //Attacks/Movesets are determined by owning certain objects (monster parts, tech devices, etc.)
    //An array of all the objects that can trigger actions for a monster
    public GameObject[] attackSlotObjects = new GameObject[8];
    private attackObject[] attackSlotObjectsScripts = new attackObject[8];
    private int[] attackSlotObjects_Type = new int[8];
    private string[] attackSlotObjects_TypeName = new string[8];
    private Animator[] attackSlotObjects_Animators = new Animator[8];

    //REACTION DRIVEN PARTS
    public List<Animator> allMonsterParts_Animators;
    public List<Animator> allTorsosAndHeads;
    public List<attackObject> allLegs;
    public List<attackObject> allAttackObjects;

    public void grabAttackSlotInfo()
    {
        for (int i = 0; i < attackSlotObjects.Length; i++)
        {
            if (attackSlotObjects[i] != null)
            {
                attackSlotObjectsScripts[i] = attackSlotObjects[i].GetComponent<attackObject>();
                attackSlotObjects_Type[i] = attackSlotObjectsScripts[i].objectType;
                attackSlotObjects_TypeName[i] = attackSlotObjectsScripts[i].objectClassName;

                if (attackSlotObjects[i].GetComponent<Animator>() != null)
                {
                    attackSlotObjects_Animators[i] = attackSlotObjects[i].GetComponent<Animator>();
                }
            }
        }
    }

    public void awakenTheBeast()
    {
        grabAttackSlotInfo();

        for (int i = 0; i < allAttackObjects.Count; i++)
        {
            allAttackObjects[i].triggerAnimationOffsets();
        }

        for (int u = 0; u < allMonsterParts_Animators.Count; u++)
        {
            allMonsterParts_Animators[u].SetTrigger("Idle");
        }

        /*
        for (int e = 0; e < allLegs.Count; e++)
        {
            if (allLegs[e].isPrimaryLeg)
            {
                allLegs[e].triggerStretchJump();
            }
        }
        */

    }

    public void attack(int attackSlot)
    {
        if (attackSlotObjects[attackSlot] != null)
        {
            //If it is a jumper
            if (attackSlotObjects_Type[attackSlot] == 0)
            {
                jump();
            }
            //If it is a monster part
            else if (attackSlotObjects_Type[attackSlot] == 1)
            {
                if (isGrounded)
                {
                    attackSlotObjectsScripts[attackSlot].activateAttack("Ground Attack");

                    if (attackSlotObjectsScripts[attackSlot].isRightSidedLimb)
                    {
                        braceForRightImpact();
                    }
                    else if (attackSlotObjectsScripts[attackSlot].isLeftSidedLimb)
                    {
                        braceForLeftImpact();
                    }
                }
                else
                {
                    attackSlotObjectsScripts[attackSlot].activateAttack("Airborn Attack");
                }
            }

            #region Removed Code that Serves a Purpose - Tells Us Every Type of Attack Orientated Monster Part

            /*
            else if (attackSlotObjects_Type[attackSlot] == 1 && attackSlotObjects_Animators[attackSlot].GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (attackSlotObjects_TypeName[attackSlot] == "Right Arm")
                {
                    if (isGrounded)
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Ground Attack");
                    }
                    else
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Airborn Attack");
                    }
                } 
                else if (attackSlotObjects_TypeName[attackSlot] == "Left Arm")
                {
                    if (isGrounded)
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Ground Attack");
                    }
                    else
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Airborn Attack");
                    }
                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Right Leg")
                {
                    if (isGrounded)
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Ground Attack");
                    }
                    else
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Airborn Attack");
                    }
                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Left Leg")
                {
                    if (isGrounded)
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Ground Attack");
                    }
                    else
                    {
                        attackSlotObjects_Animators[attackSlot].SetTrigger("Airborn Attack");
                    }
                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Single Eye")
                {

                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Double Eyes")
                {

                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Mouth")
                {

                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Head")
                {

                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Slashing Tail")
                {

                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Jabbing Tail")
                {

                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Spinning Tail")
                {

                }
                else if (attackSlotObjects_TypeName[attackSlot] == "Gizmo")
                {

                }
                else
                {
                    print("Monster Part not Found");
                }
            }

            */
            #endregion

            //If it is a scientific module
            else if (attackSlotObjects_Type[attackSlot] == 2)
            {

            }
        }
    }

    public void braceForLeftImpact()
    {
        for (int i = 0; i < allAttackObjects.Count; i++)
        {
            allAttackObjects[i].triggerLeftAttackStance();
        }
    }

    public void braceForRightImpact()
    {
        for (int i = 0; i < allAttackObjects.Count; i++)
        {
            allAttackObjects[i].triggerRightAttackStance();
        }
    }

    public void hit()
    {
        for (int u = 0; u < allMonsterParts_Animators.Count; u++)
        {
            allMonsterParts_Animators[u].SetTrigger("Hit");
        }

        for (int i = 0; i < allAttackObjects.Count; i++)
        {
            allAttackObjects[i].isAttacking = false;
        }
    }

    public void walk()
    {
        if (isGrounded)
        {
            for (int u = 0; u < allLegs.Count; u++)
            {
                allLegs[u].triggerWalk();
            }

        }
    }

    public void screechingStop()
    {
        if (isGrounded)
        {
            for (int u = 0; u < allLegs.Count; u++)
            {
                allLegs[u].triggerScreechingStop();
            }

        }
    }

    public void jump()
    {
        if (isGrounded)
        {
            isGrounded = false;

            for (int i = 0; i < allMonsterParts_Animators.Count; i++)
            {
                allMonsterParts_Animators[i].SetBool("Grounded", false);
                allMonsterParts_Animators[i].SetTrigger("Jump");
            }

            //TESTING PURPOSES - Basically faking a vertical jump and land
            StartCoroutine(fakeJump());

        }
    }

    IEnumerator fakeJump()
    {
        yield return new WaitForSeconds(3);
        land();
    }

    //This fall function is saved for when the player is knocked off an edge or walks over an edge (not a jump related fall)
    public void fall()
    {
        if (isGrounded)
        {
            isGrounded = false;

            for (int u = 0; u < allMonsterParts_Animators.Count; u++)
            {
                allMonsterParts_Animators[u].SetBool("Grounded", false);
                allMonsterParts_Animators[u].SetTrigger("Fall");
            }

            //TESTING PURPOSES - Basically faking a fall and land
            StartCoroutine(fakeJump());

        }
    }

    public void land()
    {
        if (isGrounded == false)
        {
            isGrounded = true;

            for (int u = 0; u < allMonsterParts_Animators.Count; u++)
            {
                allMonsterParts_Animators[u].SetBool("Grounded", true);
                allMonsterParts_Animators[u].SetTrigger("Land");
            }
        }
    }

    public void flipCharacter()
    {
        if (facingRight)
        {
            facingRight = false;
            //update parameter in my animator

            for (int i = 0; i < allTorsosAndHeads.Count; i++)
            {
                allTorsosAndHeads[i].SetBool("Facing Right", false);
            }

            Quaternion targetRot = Quaternion.Euler(0, -30, 0);
            transform.localRotation = targetRot;
        }
        else
        {
            facingRight = true;
            //update parameter in my animator

            for (int i = 0; i < allTorsosAndHeads.Count; i++)
            {
                allTorsosAndHeads[i].SetBool("Facing Right", true);
            }

            Quaternion targetRot = Quaternion.Euler(0, -150, 0);
            transform.localRotation = targetRot;
        }
    }
}
