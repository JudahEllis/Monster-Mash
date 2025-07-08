using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSelectIconBAS : MonoBehaviour
{
    bool isOccupied = false;

    MonsterData iconMonster;

    Image monsterIcon;
    public void IconSetUp(bool slotOccupied, MonsterData monster)
    {
        monsterIcon = GetComponentsInChildren<Image>()[1];

        if(slotOccupied)
        {
            //Set MonsterIcon to the Proper Monster

            isOccupied = true;

            iconMonster = monster;
        }

        else
        {
            //Maybe Instead set the Icon to a Plus sign to Signify that you are adding a new monster
            monsterIcon.enabled = false;
        }
    }

    public void SelectButton()
    {
        if(isOccupied)
        {
            // Edit Copy or Delete Sub Menu
        }

        else
        {
            BuildAScareManager.instance.StartNewBuildAScare();
        }
    }
}
