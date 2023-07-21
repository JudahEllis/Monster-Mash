using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickTransform : MonoBehaviour
{
    TransformManager manager;

    // Start is called before the first frame update
    void Awake()
    {
        manager = FindObjectOfType<TransformManager>();
    }

    /*private void OnMouseDown()
    {
        print("oh hell yeah");

        //determine limb vs torso

        if (GetComponent<BoxCollider>())
        {
            manager.NewRotObj(transform.parent.parent.gameObject);
        }
        else
        {
            manager.NewRotObj(transform.parent.gameObject);
        }
    }*/

    private void OnMouseOver()
    {
        //print("Oh hell yeah over");

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (GetComponent<BoxCollider>())
            {
                manager.NewRotObj(transform.parent.parent.gameObject);
            }
            else
            {
                manager.NewRotObj(transform.parent.gameObject);
            }
        }
    }
}
