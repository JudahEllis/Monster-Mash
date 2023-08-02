using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour
{
    private Rigidbody2D rb;
    private float playerSpeed = 5.0f;
    private float jumpForce = 8.0f;
    private bool grounded;
    private CapsuleCollider2D cap;
    private bool wall;
    private float move;
    private float fallMultiplier = -1f;

    [SerializeField] private LayerMask groundLayerMask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cap = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        // Check for grounded state (you can use raycasting or other methods).
        grounded = IsGrounded();

        // Horizontal movement.
        move = Input.GetAxis("Horizontal");

        if (!HitWall(move))
        {
            rb.velocity = new Vector2(move * playerSpeed, rb.velocity.y);
        }

        // Jumping.
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (!grounded)
        {
            //rb.velocity += new Vector2(0, rb.gravityScale * fallMultiplier);
        }
    }

    private bool IsGrounded()
    {
        // Implement your own grounded check, e.g., using raycasting or collider checks.
        // Return true if the player is grounded, false otherwise.
        /*if(Physics2D.Raycast(transform.position, Vector2.down, cap.size.y /2 + 0.1f, groundLayerMask))
        {
            //print("YES");
            return true;
        }*/

        if (Physics2D.CapsuleCast(transform.position, cap.size, cap.direction, 0, Vector2.down, 0.1f, groundLayerMask))
        {
            return true;
        }

        //print("NOO");
        return false;
    }

    private bool HitWall(float direction)
    {
        /*if (Physics2D.Raycast(transform.position, Vector2.right * direction, cap.size.x /2 +0.1f, groundLayerMask))
        {
            return true;
        }*/

        if (Physics2D.CapsuleCast(transform.position, new Vector2(cap.size.x, cap.size.y * 0.5f), cap.direction, 0, Vector2.right * direction, 0.1f, groundLayerMask))
        {
            return true;
        }

        return false;
    }
}