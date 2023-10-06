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
    [SerializeField] private float playerSpeedGrounded = 1f;
    [SerializeField] private float playerSpeedAir = 0.989f;
    [SerializeField] private float playerSpeedCurrent = 1.0f;
    [SerializeField] private float playerSpeed = 20f;
    private float jumpHeight = 15.0f;
    private float gravityValue = -57f;
    private float groundedGravity = -1f; //not applying gravity caused errors but regular gravity was too strong
    private float fallMultiplier = 2.5f; // Adjust this value to control fall speed.
    private float terminalVelocity = -56f; // Limit the maximum falling speed.
    private bool knockBack = false; //true when player is in "knockback" state
    private bool stun = false; //true when in "stun" state, hitstun
    private bool jump = false;
    private bool jumpStarted = false;

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
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isAttacking = false;

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
                playerSpeedCurrent = playerSpeedGrounded;

                if ((isJumping || isFalling) && !jumpStarted)
                {
                    isJumping = false;
                    isFalling = false;
                    attack.land();
                }

                if (isWalk )//&& attack.CurrentAnimationState().IsName("Idle"))
                {
                    attack.walk();
                }
            }
            else
            {
                playerVelocity.y += gravityValue * fallMultiplier * Time.deltaTime;
                playerSpeedCurrent = playerSpeedAir;

                if (jumpStarted) //waits for player to leave the ground so the animations aren't cancelled when still on ground
                {
                    jumpStarted = false;
                }

                if (!isFalling && playerVelocity.y < 0 && isWalk)
                {
                    isFalling = true;
                    attack.walkToFall();
                }

                if (jumpsRemaining > 1 && !Input.GetKey(KeyCode.Space))
                {
                    jumpsRemaining = 1;
                }
            }

            if (!IsBusy())
            {
                //if (move != Vector3.zero)
                //{
                    move.x = playerSpeedCurrent * move.normalized.x;
                //}
                controller.Move(move * Time.deltaTime * playerSpeed);
            }

            // Perform the jump when the Space key is pressed and there are jumps remaining.
            if (jump && jumpsRemaining > 0)
            {
                isJumping = true;

                if (groundedPlayer)
                {
                    attack.jump();
                }
                else
                {
                    attack.doubleJump();
                    //attack.jump();
                }
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                jumpsRemaining--;
                jump = false;
                jumpStarted = true;
            }
            else if (jump) jump = false;

            //AnimatorStateInfo anim = attack.CurrentAnimationState();

            //if (isAttacking && (anim.IsName("Base Layer.Idle") || anim.IsName("Base Layer.Walk") || anim.IsName("Base Layer.Run")))
            if (attack.allMonsterParts[0].isAttackingJudah())
            {
                isAttacking = false;
                
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

    private bool IsBusy() //returns true if the player is already jumping or attacking
    {
        if (isAttacking)
        {
            return true;
        }

        return false;
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

        print("dir: " + dir);
        move = dir;
    }

    public void Jump()
    {
        if (!IsBusy())
        {
            jump = true;
        }
    }

    public void Attack()
    {
        int random = Random.Range(0, 2);

        attack.attack(3);
    }

    public void Attack1()
    {
        if (!IsBusy())
        {
            isAttacking = true;
            attack.attack(1);
        }
    }

    public void Attack2()
    {
        if (!IsBusy())
        {
            isAttacking = true;
            attack.attack(2);
        }
    }

    public void Attack3()
    {
        if (!IsBusy())
        {
            isAttacking = true;
            attack.attack(3);
        }
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