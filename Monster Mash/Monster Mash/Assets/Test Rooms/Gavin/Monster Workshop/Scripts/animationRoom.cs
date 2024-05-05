using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class animationRoom : MonoBehaviour
{
    public GameObject floor;
    public GameObject groundedLimbCheck;
    public monsterAttackSystem mainMonster;
    public monsterPart[] monsterPartCollection;
    public GameObject reMappingStartUpButton;
    public GameObject reMappingUI;
    public GameObject resetMappingUI;
    public GameObject inputListener;
    private int selectedMonsterPart = 0;
    public GameObject animationTestingUI;
    public TMP_InputField buttonInput;
    private string[] monsterPartButtonInputs;
    public TMP_InputField attackDirection;
    private string[] monsterPartAttackDirections;
    public TMP_Text monsterPartName;
    private string[] monsterPartNameCollection;
    public Animator sceneCamera;
    private bool facingLeft = true;
    public GameObject leftCloseRangeTarget;
    public GameObject leftMediumRangeTarget;
    public GameObject leftLongRangeTarget;
    public GameObject rightCloseRangeTarget;
    public GameObject rightMediumRangeTarget;
    public GameObject rightLongRangeTarget;
    private int cameraNumber = -1; //facing left first
    private bool floorActive = true;
    private bool monsterAwake = false;

    public void removeFloor()
    {
        floor.SetActive(false);
        floorActive = false;
        switchTarget();
    }

    public void bringBackFloor()
    {
        floor.SetActive(true);
        floorActive = true;
        switchTarget();
    }

    public void removeGroundedLimbCheck()
    {
        groundedLimbCheck.SetActive(false);
    }

    //

    private void Awake()
    {
        resetAnimationRoom();
    }

    public void resetAnimationRoom()
    {
        reMappingStartUpButton.SetActive(true);
        if (inputListener != null)
        {
            inputListener.SetActive(false);
        }
        reMappingUI.SetActive(false);
        //mainMonster.removeAllLimbParenting();
        mainMonster.connectCurrentLimbs();
        //mainMonster.connectCurrentLimbs();//currently have this playing twice to grab first the torsos and then their heads
    }

    public void startRemappingProcess()
    {
        //mainMonster.removeAllLimbParenting();
        mainMonster.connectCurrentLimbs();
        StartCoroutine(remappingProcessDelay());
    }

    IEnumerator remappingProcessDelay()
    {
        reMappingStartUpButton.SetActive(false);
        animationTestingUI.SetActive(false);
        if (resetMappingUI != null)
        {
            resetMappingUI.SetActive(false);
        }
        if (inputListener != null)
        {
            inputListener.SetActive(true);
        }
        yield return new WaitForSeconds(1);

        if (monsterAwake == false)
        {
            mainMonster.awakenTheBeast();
            monsterAwake = true;

            monsterPartCollection = mainMonster.GetComponentsInChildren<monsterPart>();
            monsterPartAttackDirections = new string[monsterPartCollection.Length];
            monsterPartNameCollection = new string[monsterPartCollection.Length];
            monsterPartButtonInputs = new string[monsterPartCollection.Length];

            for (int i = 0; i < monsterPartCollection.Length; i++)
            {
                monsterPartCollection[i].setUpOutline();
                monsterPartAttackDirections[i] = "";
                monsterPartNameCollection[i] = monsterPartCollection[i].gameObject.name;
                monsterPartButtonInputs[i] = "";
            }
        }

        monsterPartCollection[0].reenableOutline();
        selectedMonsterPart = 0;
        attackDirection.text = monsterPartAttackDirections[selectedMonsterPart];
        monsterPartName.text = monsterPartNameCollection[selectedMonsterPart];
        buttonInput.text = monsterPartButtonInputs[selectedMonsterPart];
        reMappingUI.SetActive(true);
    }

    public void cyclePartsRight()
    {
        if (selectedMonsterPart == monsterPartCollection.Length - 1)
        {
            monsterPartCollection[selectedMonsterPart].disableOutline();
            selectedMonsterPart = 0;
            monsterPartCollection[selectedMonsterPart].reenableOutline();
        }
        else
        {
            monsterPartCollection[selectedMonsterPart].disableOutline();
            selectedMonsterPart++;
            monsterPartCollection[selectedMonsterPart].reenableOutline();
        }

        attackDirection.text = monsterPartAttackDirections[selectedMonsterPart];
        monsterPartName.text = monsterPartNameCollection[selectedMonsterPart];
        buttonInput.text = monsterPartButtonInputs[selectedMonsterPart];
    }

    public void cyclePartsLeft()
    {
        if (monsterPartCollection[selectedMonsterPart] == monsterPartCollection[0])
        {
            monsterPartCollection[selectedMonsterPart].disableOutline();
            selectedMonsterPart = monsterPartCollection.Length - 1;
            monsterPartCollection[selectedMonsterPart].reenableOutline();
        }
        else
        {
            monsterPartCollection[selectedMonsterPart].disableOutline();
            selectedMonsterPart--;
            monsterPartCollection[selectedMonsterPart].reenableOutline();
        }

        attackDirection.text = monsterPartAttackDirections[selectedMonsterPart];
        monsterPartName.text = monsterPartNameCollection[selectedMonsterPart];
        buttonInput.text = monsterPartButtonInputs[selectedMonsterPart];
    }

    public void updateButtonInput()
    {
        string inputLetters = buttonInput.text.ToString();

        if (inputLetters == "A")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "A")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[0] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "A";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else if (inputLetters == "B")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "B")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[1] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "B";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else if (inputLetters == "X")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "X")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[2] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "X";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else if (inputLetters == "Y")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "Y")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[3] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "Y";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else if (inputLetters == "LB")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "LB")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[4] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "LB";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else if (inputLetters == "RB")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "RB")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[5] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "RB";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else if (inputLetters == "LT")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "LT")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[6] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "LT";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else if (inputLetters == "RT")
        {
            for (int i = 0; i < monsterPartButtonInputs.Length; i++)
            {
                if (monsterPartButtonInputs[i] == "RT")
                {
                    monsterPartButtonInputs[i] = "";
                }
            }

            mainMonster.attackSlotMonsterParts[7] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "RT";
            mainMonster.grabAttackSlotInfo();
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
        }
        else
        {
            buttonInput.text = "";
            print("Input not recognized. Try retyping your button input for " + monsterPartNameCollection[selectedMonsterPart]);
            monsterPartButtonInputs[selectedMonsterPart] = inputLetters;
        }
    }

    public void updateAttackDirection()
    {
        string direction = attackDirection.text.ToString();

        if (direction == "FORWARDS")
        {
            monsterPartCollection[selectedMonsterPart].attackAnimationID = 1;
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
            monsterPartAttackDirections[selectedMonsterPart] = direction;
        }
        else if (direction == "BACKWARDS")
        {
            monsterPartCollection[selectedMonsterPart].attackAnimationID = -1;
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
            monsterPartAttackDirections[selectedMonsterPart] = direction;
        }
        else if (direction == "UPWARDS")
        {
            monsterPartCollection[selectedMonsterPart].attackAnimationID = 2;
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
            monsterPartAttackDirections[selectedMonsterPart] = direction;
        }
        else if (direction == "DOWNWARDS")
        {
            monsterPartCollection[selectedMonsterPart].attackAnimationID = 0;
            monsterPartCollection[selectedMonsterPart].changeAttackAnimationAtRuntime();
            monsterPartAttackDirections[selectedMonsterPart] = direction;
        }
        else
        {
            attackDirection.text = "";
            print("Attack not recognized.Try retyping your attack direction for " + monsterPartNameCollection[selectedMonsterPart]);
            monsterPartAttackDirections[selectedMonsterPart] = direction;
        }

    }

    public void textBeingUpdated()
    {
        inputListener.SetActive(false);
    }

    public void textDoneBeingUpdated()
    {
        inputListener.SetActive(true);
    }

    public void activateTestAnimations()
    {
        mainMonster.connectCurrentLimbs();
        mainMonster.grabAttackSlotInfo();
        monsterPartCollection[selectedMonsterPart].disableOutline();
        reMappingUI.SetActive(false);
        if (resetMappingUI != null)
        {
            resetMappingUI.SetActive(true);
        }
        animationTestingUI.SetActive(true);
        if (inputListener != null)
        {
            inputListener.SetActive(true);
        }
        switchTarget();
    }

    public void flipCamera()
    {
        sceneCamera.SetTrigger("Flip Camera");

        if (facingLeft)
        {
            facingLeft = false;
        }
        else
        {
            facingLeft = true;
        }

        cameraNumber = cameraNumber * -1;
        switchTarget();
    }

    public void closeCamera()
    {
        //sceneCamera.ResetTrigger("Close Range");
        sceneCamera.ResetTrigger("Medium Range");
        sceneCamera.ResetTrigger("Long Range");
        sceneCamera.SetTrigger("Close Range");

        if (facingLeft)
        {
            cameraNumber = -1;
        }
        else
        {
            cameraNumber = 1;
        }

        switchTarget();
    }

    public void mediumCamera()
    {
        sceneCamera.ResetTrigger("Close Range");
        //sceneCamera.ResetTrigger("Medium Range");
        sceneCamera.ResetTrigger("Long Range");
        sceneCamera.SetTrigger("Medium Range");

        if (facingLeft)
        {
            cameraNumber = -2;
        }
        else
        {
            cameraNumber = 2;
        }

        switchTarget();
    }

    public void longCamera()
    {
        sceneCamera.ResetTrigger("Close Range");
        sceneCamera.ResetTrigger("Medium Range");
        //sceneCamera.ResetTrigger("Long Range");
        sceneCamera.SetTrigger("Long Range");

        if (facingLeft)
        {
            cameraNumber = -3;
        }
        else
        {
            cameraNumber = 3;
        }

        switchTarget();
    }

    private void switchTarget()
    {
        if (cameraNumber == -1)
        {
            leftCloseRangeTarget.SetActive(true);
            leftMediumRangeTarget.SetActive(false);
            leftLongRangeTarget.SetActive(false);
            rightCloseRangeTarget.SetActive(false);
            rightMediumRangeTarget.SetActive(false);
            rightLongRangeTarget.SetActive(false);

            if (floorActive)
            {
                leftCloseRangeTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                leftCloseRangeTarget.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if (cameraNumber == -2)
        {
            leftCloseRangeTarget.SetActive(false);
            leftMediumRangeTarget.SetActive(true);
            leftLongRangeTarget.SetActive(false);
            rightCloseRangeTarget.SetActive(false);
            rightMediumRangeTarget.SetActive(false);
            rightLongRangeTarget.SetActive(false);

            if (floorActive)
            {
                leftMediumRangeTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                leftMediumRangeTarget.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if (cameraNumber == -3)
        {
            leftCloseRangeTarget.SetActive(false);
            leftMediumRangeTarget.SetActive(false);
            leftLongRangeTarget.SetActive(true);
            rightCloseRangeTarget.SetActive(false);
            rightMediumRangeTarget.SetActive(false);
            rightLongRangeTarget.SetActive(false);

            if (floorActive)
            {
                leftLongRangeTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                leftLongRangeTarget.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if (cameraNumber == 1)
        {
            leftCloseRangeTarget.SetActive(false);
            leftMediumRangeTarget.SetActive(false);
            leftLongRangeTarget.SetActive(false);
            rightCloseRangeTarget.SetActive(true);
            rightMediumRangeTarget.SetActive(false);
            rightLongRangeTarget.SetActive(false);

            if (floorActive)
            {
                rightCloseRangeTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                rightCloseRangeTarget.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if (cameraNumber == 2)
        {
            leftCloseRangeTarget.SetActive(false);
            leftMediumRangeTarget.SetActive(false);
            leftLongRangeTarget.SetActive(false);
            rightCloseRangeTarget.SetActive(false);
            rightMediumRangeTarget.SetActive(true);
            rightLongRangeTarget.SetActive(false);

            if (floorActive)
            {
                rightMediumRangeTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                rightMediumRangeTarget.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if (cameraNumber == 3)
        {
            leftCloseRangeTarget.SetActive(false);
            leftMediumRangeTarget.SetActive(false);
            leftLongRangeTarget.SetActive(false);
            rightCloseRangeTarget.SetActive(false);
            rightMediumRangeTarget.SetActive(false);
            rightLongRangeTarget.SetActive(true);

            if (floorActive)
            {
                rightLongRangeTarget.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                rightLongRangeTarget.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
