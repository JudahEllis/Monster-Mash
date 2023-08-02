using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller1 : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 playerVelocity;
    private bool groundedPlayer; //get grounded state
    private int jumpsRemaining; // Track the number of jumps left.
    private float playerSpeed = 5.0f;
    private float jumpHeight = 3.0f;
    private float gravityValue = -9.81f;
    private float groundedGravity = -1f; //not applying gravity caused errors but regular gravity was too strong
    private float fallMultiplier = 2.5f; // Adjust this value to control fall speed.
    private float terminalVelocity = -50f; // Limit the maximum falling speed.
    private bool knockBack = false; //true when player is in "knockback" state
    private bool stun = false; //true when in "stun" state, hitstun

    [SerializeField] private GameObject[] ActiveBodyParts; //body parts used for attacks
    private int lastUsedBP; //index of most recently used ActiveBodyPart

    [SerializeField] private Vector3 impact = Vector3.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        jumpsRemaining = 2; // Set the initial jumps allowed (double jump).
    }

    void Update()
    {
        if (!knockBack || !stun)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer)
            {
                playerVelocity.y = -1f; // Reset the vertical velocity when grounded.
                jumpsRemaining = 2; // Reset the number of jumps when grounded.
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            controller.Move(move * Time.deltaTime * playerSpeed);

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

                if (jumpsRemaining > 1 && !Input.GetKey(KeyCode.Space))
                {
                    jumpsRemaining = 1;
                }
            }
        }
        else //while knockback
        {
            KnockingBack();
            playerVelocity = impact;
        }

        // Clamp the vertical velocity to prevent it from increasing indefinitely.
        for (int i = 0; i < 3; i++)
        {
            playerVelocity[i] = Mathf.Max(playerVelocity[i], terminalVelocity);
            playerVelocity[i] = Mathf.Min(playerVelocity[i], -terminalVelocity);
        }
        //playerVelocity.y = Mathf.Max(playerVelocity.y, terminalVelocity);
        //playerVelocity.y = Mathf.Min

        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            //AddImpact(Vector3.left + Vector3.up, 15f, 0.5f);
            LimbFallOff();
        }
    }

    #region LimbFallOff
    private void LimbFallOff()
    {
        GameObject myBP = ActiveBodyParts[0];
        myBP.transform.parent = null;
        myBP.GetComponent<Rigidbody>().isKinematic = false;
    }
    #endregion

    #region KnockBack
    public void AddImpact(Vector3 dir, float force, float dur)
    {
        ApplyKnockback(dir * force, dur);
    }

    private void ApplyKnockback(Vector3 dir, float dur)
    {
        StopCoroutine("StartStopKnockback");
        StopKnockback();
        knockBack = true;
        stun = true;
        impact = dir;
        StartCoroutine("StartStopKnockback", dur);
    }

    private void KnockingBack()
    {
        if (impact.y > 1)
        {
            impact.y *= 0.8f;
        } else if (impact.y > 0)
        {
            //impact.y = 0.0f;
        }
    }

    private IEnumerator StartStopKnockback(float dur)
    {
        yield return new WaitForSeconds(dur);

        StopKnockback();

        yield break;
    }
    private void StopKnockback()
    {
        knockBack = false;
        stun = false;
        impact = Vector3.zero;
        playerVelocity = Vector3.zero;
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.CompareTag("Ground") && knockBack)
        {
            //print("hit that wall");
            StopCoroutine("StartStopKnockback");
            StopKnockback();
        }
    }

    #endregion
}