using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller1 : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private int jumpsRemaining; // Track the number of jumps left.
    private float playerSpeed = 5.0f;
    private float jumpHeight = 3.0f;
    private float gravityValue = -9.81f;
    private float fallMultiplier = 2.5f; // Adjust this value to control fall speed.
    private float terminalVelocity = -50f; // Limit the maximum falling speed.

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpsRemaining = 2; // Set the initial jumps allowed (double jump).
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            playerVelocity.y = 0f; // Reset the vertical velocity when grounded.
            jumpsRemaining = 2; // Reset the number of jumps when grounded.
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Perform the jump when the Space key is pressed and there are jumps remaining.
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpsRemaining--;
        }

        // Apply gravity only when not grounded and add fallMultiplier when falling.
        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * fallMultiplier * Time.deltaTime;
        }

        // Clamp the vertical velocity to prevent it from increasing indefinitely.
        playerVelocity.y = Mathf.Max(playerVelocity.y, terminalVelocity);

        controller.Move(playerVelocity * Time.deltaTime);
    }
}