using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSpeedTest : MonoBehaviour
{
    public monsterAttackSystem monster;

    // Update is called once per frame
    void Update()
    {
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
    }
}
