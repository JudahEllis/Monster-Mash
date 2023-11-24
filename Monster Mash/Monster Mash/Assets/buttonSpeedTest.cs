using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSpeedTest : MonoBehaviour
{
    private string answer;
    private bool answerRead = false;
    public monsterAttackSystem monster;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //answerRead = false;
            //answer = "Button Held";
            monster.attack(1);
            //StartCoroutine(speedometer());
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            monster.attackCancel(1);
            //StopAllCoroutines();

            if (answerRead == false)
            {
                //answer = "Button Up";
                //monster.attackCancel(1);
                //StopAllCoroutines();
                //print(answer);
            }
        }
    }

    IEnumerator speedometer()
    {
        yield return new WaitForSeconds(0.09f);
        //print(answer);
        answerRead = true;
    }
}
