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
            int attackDirection = monster.attackSlotMonsterParts[0].attackAnimationID;
            monster.attack(0, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            monster.attackCancel(0);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            int attackDirection = monster.attackSlotMonsterParts[1].attackAnimationID;
            monster.attack(1, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            monster.attackCancel(1);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            int attackDirection = monster.attackSlotMonsterParts[2].attackAnimationID;
            monster.attack(2, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            monster.attackCancel(2);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            int attackDirection = monster.attackSlotMonsterParts[3].attackAnimationID;
            monster.attack(3, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.Y))
        {
            monster.attackCancel(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) //LB
        {
            int attackDirection = monster.attackSlotMonsterParts[4].attackAnimationID;
            monster.attack(4, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.Alpha1)) //LB
        {
            monster.attackCancel(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //RB
        {
            int attackDirection = monster.attackSlotMonsterParts[5].attackAnimationID;
            monster.attack(5, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2)) //RB
        {
            monster.attackCancel(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //LT
        {
            int attackDirection = monster.attackSlotMonsterParts[6].attackAnimationID;
            monster.attack(6, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3)) //LT
        {
            monster.attackCancel(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //RT
        {
            int attackDirection = monster.attackSlotMonsterParts[7].attackAnimationID;
            monster.attack(7, attackDirection);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4)) //RT
        {
            monster.attackCancel(7);
        }
    }
}
