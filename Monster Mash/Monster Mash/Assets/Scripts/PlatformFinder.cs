using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFinder : MonoBehaviour
{
    private bool isGrounded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            isGrounded = true;
            print("landed");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            isGrounded = false;
            print("notLanded");
        }
    }

    public bool Grounded()
    {
        return isGrounded;
    }
}
