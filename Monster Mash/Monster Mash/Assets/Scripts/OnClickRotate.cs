using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickRotate : MonoBehaviour
{
    RotateManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<RotateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        print("oh hell yeah");
        manager.NewRotObj(transform.parent.gameObject);
    }
}
