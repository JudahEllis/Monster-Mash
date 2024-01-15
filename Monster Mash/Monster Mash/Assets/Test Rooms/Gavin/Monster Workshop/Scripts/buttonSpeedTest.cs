using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSpeedTest : MonoBehaviour
{
    public monsterAttackSystem monster;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            monster.attack(0);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            monster.attackCancel(0);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            monster.attack(1);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            monster.attackCancel(1);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            monster.attack(2);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            monster.attackCancel(2);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            monster.attack(3);
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            monster.attackCancel(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) //LB
        {
            monster.attack(4);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1)) //LB
        {
            monster.attackCancel(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //RB
        {
            monster.attack(5);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2)) //RB
        {
            monster.attackCancel(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //LT
        {
            monster.attack(6);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3)) //LT
        {
            monster.attackCancel(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //RT
        {
            monster.attack(7);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4)) //RT
        {
            monster.attackCancel(7);
        }
    }
}
