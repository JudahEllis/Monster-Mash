using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerTest : MonoBehaviour
{
    float speed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * speed;
        } else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * speed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed;
        } else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed;
        }
    }
}
