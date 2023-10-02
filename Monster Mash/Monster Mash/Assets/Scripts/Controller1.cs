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
    private float playerSpeed = 20.0f;
    private float jumpHeight = 15.0f;
    private float gravityValue = -9.81f;
    private float groundedGravity = -1f; //not applying gravity caused errors but regular gravity was too strong
    private float fallMultiplier = 2.5f; // Adjust this value to control fall speed.
    private float terminalVelocity = -50f; // Limit the maximum falling speed.
    private bool knockBack = false; //true when player is in "knockback" state
    private bool stun = false; //true when in "stun" state, hitstun
    private bool jump = false;

    private Vector3 move;

    [SerializeField] private GameObject[] ActiveBodyParts; //body parts used for attacks
    private int lastUsedBP; //index of most recently used ActiveBodyPart

    [SerializeField] private Vector3 impact = Vector3.zero;

    [SerializeField] private GameObject dummy;
    private Rigidbody dummyRb;
    private DummyCollision col;
    private bool checkKB = false;

    private monsterAttackSystem attack;
    private bool isWalk = false;
    private bool facingRight = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        if (FindObjectOfType<monsterAttackSystem>())
        {
            attack = FindObjectOfType<monsterAttackSystem>();

            attack.awakenTheBeast();

            //attack.flipCharacter();
        }

        if (GameObject.Find("Dummy"))
        {
            dummy = GameObject.Find("Dummy");
            dummyRb = dummy.GetComponent<Rigidbody>();
            col = dummy.GetComponent<DummyCollision>();
        }

        jumpsRemaining = 2; // Set the initial jumps allowed (double jump).
    }

    void Update()
    {

        if (!knockBack)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer)
            {
                playerVelocity.y = -1f; // Reset the vertical velocity when grounded.
                jumpsRemaining = 2; // Reset the number of jumps when grounded.
            }


            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                transform.forward = move;
            }

            // Perform the jump when the Space key is pressed and there are jumps remaining.
            if (jump && jumpsRemaining > 0)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                jumpsRemaining--;
                jump = false;
            }
            else if (jump) jump = false;

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
            //playerVelocity = impact;
        }

        // Clamp the vertical velocity to prevent it from increasing indefinitely.
        for (int i = 0; i < 3; i++)
        {
            playerVelocity[i] = Mathf.Max(playerVelocity[i], terminalVelocity);
            playerVelocity[i] = Mathf.Min(playerVelocity[i], -terminalVelocity);
        }

        controller.Move(playerVelocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            //AddImpact(Vector3.left + Vector3.up, 15f, 0.5f);
            //LimbFallOff();
            AddImpact();
        }
    }


    #region public Input functions for input_handler script
    public void Move(int direction)
    {
        Vector3 dir = Vector3.zero;

        if (direction > 0)
        {
            dir = Vector3.right;

            if (!isWalk)
            {
                attack.walk();
                isWalk = true;
            }

            if (!facingRight)
            {
                attack.flipCharacter();
                facingRight = true;
            }
        }
        else if (direction < 0)
        {
            dir = Vector3.left;

            if (!isWalk)
            {
                attack.walk();
                isWalk = true;
            }

            if (facingRight)
            {
                attack.flipCharacter();
                facingRight = false;
            }
        }

        if (isWalk && dir == Vector3.zero)
        {
            attack.stopWalking();
            isWalk = false;
        }

        move = dir;
    }

    public void KeyboardMoveLeft(bool canMove)
    {
        if (canMove)
        {
            move = Vector3.left;
        } else
        {
        }
    }

    public void KeyboardMoveRight()
    {
        move = Vector3.right;
    }

    public void Jump()
    {
        jump = true;
    }
    #endregion

    #region LimbFallOff
    private void LimbFallOff()
    {
        GameObject myBP = ActiveBodyParts[0];
        myBP.transform.parent = null;
        myBP.GetComponent<Rigidbody>().isKinematic = false;
    }
    #endregion

    #region KnockBack

    void AddImpact()
    {
        checkKB = false;
        knockBack = true;
        dummyRb.velocity = Vector3.zero;
        dummy.transform.position = transform.position + new Vector3(0, 0.9f, 0);
        dummyRb.AddForce(new Vector3(-15, 10, 0), ForceMode.Impulse);
        Invoke("Check", 0.25f);
    }

    void KnockingBack()
    {
        if (checkKB && col.GetCollide())
        {
            dummyRb.velocity = Vector3.zero;
            knockBack = false;
        }

        controller.Move(dummy.transform.position - transform.position);
    }

    void Check()
    {
        checkKB = true;
    }

    #endregion
}