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
    private monsterPart[] monsterPartCollection;
    public GameObject reMappingStartUpButton;
    public GameObject reMappingUI;
    private int selectedMonsterPart = 0;
    public GameObject animationTestingUI;
    public TMP_InputField buttonInput;
    private string[] monsterPartButtonInputs;
    public TMP_InputField attackDirection;
    private string[] monsterPartAttackDirections;
    public TMP_Text monsterPartName;
    private string[] monsterPartNameCollection;
    public Animator sceneCamera;

    public void removeFloor()
    {
        floor.SetActive(false);
    }

    public void bringBackFloor()
    {
        floor.SetActive(true);
    }

    public void removeGroundedLimbCheck()
    {
        groundedLimbCheck.SetActive(false);
    }

    //

    public void startRemappingProcess()
    {
        reMappingStartUpButton.SetActive(false);
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

        monsterPartCollection[0].reenableOutline();
        reMappingUI.SetActive(true);
        monsterPartName.text = monsterPartNameCollection[selectedMonsterPart];
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
            mainMonster.attackSlotMonsterParts[0] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "A";
        }
        else if (inputLetters == "B")
        {
            mainMonster.attackSlotMonsterParts[1] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "B";
        }
        else if (inputLetters == "X")
        {
            mainMonster.attackSlotMonsterParts[2] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "X";
        }
        else if (inputLetters == "Y")
        {
            mainMonster.attackSlotMonsterParts[3] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "Y";
        }
        else if (inputLetters == "LB")
        {
            mainMonster.attackSlotMonsterParts[4] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "LB";
        }
        else if (inputLetters == "RB")
        {
            mainMonster.attackSlotMonsterParts[5] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "RB";
        }
        else if (inputLetters == "LT")
        {
            mainMonster.attackSlotMonsterParts[6] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "LT";
        }
        else if (inputLetters == "RT")
        {
            mainMonster.attackSlotMonsterParts[7] = monsterPartCollection[selectedMonsterPart];
            monsterPartButtonInputs[selectedMonsterPart] = "RT";
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

    public void activateTestAnimations()
    {
        monsterPartCollection[selectedMonsterPart].disableOutline();
        reMappingUI.SetActive(false);
        animationTestingUI.SetActive(true);
    }

    public void flipCamera()
    {
        sceneCamera.SetTrigger("Flip Camera");
    }
}
