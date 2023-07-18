using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickTransform : MonoBehaviour
{
    TransformManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<TransformManager>();
    }

    private void OnMouseDown()
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
    }
}
