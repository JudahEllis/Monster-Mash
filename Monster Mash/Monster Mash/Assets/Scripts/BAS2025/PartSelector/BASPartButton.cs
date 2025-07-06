using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BASPartButton : MonoBehaviour
{
    public string monsterPartReference;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayJostleAnim()
    {
        anim.SetLayerWeight(1, 1);
        anim.SetTrigger("Jostle");
    }
    public void JostleAnimDone()
    {
        anim.SetLayerWeight(1, 0);
    }
    public void CheckIndex()
    {
        //grid.CheckScrollPage(gameObject);
    }
}
