using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller1 : MonoBehaviour
{
    [SerializeField] LayerMask platCast;
    [SerializeField] LayerMask bounceCast;
    [SerializeField] LayerMask groundCheck;

    private CharacterController controller;
    private Rigidbody rb;
    [SerializeField] private CapsuleCollider capcol;
    private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer; //get grounded state
    private PlatformFinder platFinder;
    private bool platCanSolid = true;
    private float platCooldown = 1f;
    [SerializeField]private bool platDropInput = false;
    [SerializeField] private int jumpsRemaining; // Track the number of jumps left.
    [SerializeField] private float playerSpeedGrounded = 1f;
    [SerializeField] private float playerSpeedAir = 0.989f;
    [SerializeField] private float playerSpeedCurrent = 1.0f;
    [SerializeField] private float playerSpeed = 20f;
    private float playerSpeedRunning = 1.5f;
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
    [SerializeField] private bool isRunning = false;

    [SerializeField] private GameObject[] ActiveBodyParts; //body parts used for attacks
    private int lastUsedBP; //index of most recently used ActiveBodyPart

    [SerializeField] private Vector3 impact = Vector3.zero;

    [SerializeField] private GameObject dummy;
    private Rigidbody dummyRb;
    private DummyCollision col;
    private bool checkKB = false;

    private monsterAttackSystem monsterAnim;
    private bool isWalk = false;
    private bool facingRight = true;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isAttacking = false;
    private bool screechingStop = false;
    private float screechingStopTime = 0.55f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        if (FindObjectOfType<monsterAttackSystem>())
        {
            monsterAnim = FindObjectOfType<monsterAttackSystem>();

            monsterAnim.awakenTheBeast();

            //attack.flipCharacter();
        }

        platFinder = GetComponentInChildren<PlatformFinder>();

        if (GameObject.Find("Dummy"))
        {
            dummy = GameObject.Find("Dummy");
            dummyRb = dummy.GetComponent<Rigidbody>();
            col = dummy.GetComponent<DummyCollision>();
        }

        jumpsRemaining = 2; // Set the initial jumps allowed (double jump).
    }

    private void FixedUpdate()
    {
        /*if (!platFinder.Grounded()) //if the semisolid platform collider is NOT grounded, check if at least CC is grounded
        {
            groundedPlayer = controller.isGrounded;
        }
        else
        {
            groundedPlayer = true;

            if (!jumpStarted)
            {
                playerVelocity.y = 0f;
            }
        }*/
    }
    void Update()
    {
        if (!isFalling && playerVelocity.y < 0 && isWalk && !groundedPlayer && !Physics.Raycast(transform.position - Vector3.down, Vector3.down, 4f, groundCheck))
        {
            isFalling = true;
            monsterAnim.walkToFall();
        }

        RaycastHit hit;
        Debug.DrawRay(transform.position - Vector3.down, Vector3.down * 2f, Color.red);
        if (Physics.Raycast(transform.position - Vector3.down, Vector3.down, out hit, 2f, platCast))
        {
            if (platCanSolid)
            {
                hit.collider.isTrigger = false;
            }

            if (platDropInput)
            {
                StartCoroutine("PlatMakeSolidCooldown");
                monsterAnim.goThroughPlatform();
                hit.collider.isTrigger = true;
            }
        }
        Debug.DrawRay(transform.position + Vector3.up, Vector3.up * 2f);
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.up, out hit, 2f, platCast))
        {

            hit.collider.isTrigger = true;
        }

        isAttacking = monsterAnim.IsAttacking();

        if (!knockBack)
        {
            groundedPlayer = controller.isGrounded;

            if (groundedPlayer)
            {
                monsterAnim.land();
                playerVelocity.y = -1f; // Reset the vertical velocity when grounded.
                if (jumpsRemaining < 2) jumpsRemaining = 2; // Reset the number of jumps when grounded.
                playerSpeedCurrent = playerSpeedGrounded;

                if ((isJumping || isFalling) && !jumpStarted)
                {
                    isJumping = false;
                    isFalling = false;
                    monsterAnim.land();
                }

                if (isWalk )
                {
                    monsterAnim.walk();

                    if (isRunning)
                    {
                        playerSpeedCurrent = playerSpeedRunning;
                        monsterAnim.run();
                    }
                    else
                    {
                        playerSpeedCurrent = playerSpeedGrounded;
                        monsterAnim.stopRunning();
                    }
                } else { monsterAnim.stopWalking(); }
            }
            else
            {
                playerVelocity.y += gravityValue * fallMultiplier * Time.deltaTime;
                playerSpeedCurrent = playerSpeedAir;

                if (jumpStarted) //waits for player to leave the ground so the animations aren't cancelled when still on ground
                {
                    jumpStarted = false;
                }

                /*if (!isFalling && playerVelocity.y < 0 && isWalk)
                {
                    isFalling = true;
                    monsterAnim.walkToFall();
                }*/

                if (jumpsRemaining > 1 && !Input.GetKey(KeyCode.Space))
                {
                    //jumpsRemaining -= 1;
                }
            }

            if (!screechingStop)//(/*IsBusy()*/true)//!isAttacking && !screechingStop)
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
                    monsterAnim.jump();
                }
                else
                {
                    monsterAnim.doubleJump();
                }
                int jumpMulti = 1;

                if (Physics.Raycast(transform.position - Vector3.down, Vector3.down, out hit, 2f, bounceCast))
                {
                    print("BOUNCE!");
                    //jumpMulti = 3;
                }
                playerVelocity.y = Mathf.Sqrt(jumpHeight * jumpMulti * -3.0f * gravityValue);
                jumpsRemaining--;
                jump = false;
                jumpStarted = true;
            }
            else if (jump) jump = false;

            //AnimatorStateInfo anim = attack.CurrentAnimationState();

            //if (isAttacking && (anim.IsName("Base Layer.Idle") || anim.IsName("Base Layer.Walk") || anim.IsName("Base Layer.Run")))

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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (platDropInput)//Input.GetKey(KeyCode.P))
            {
                collision.collider.isTrigger = true;
            }
        } else if (collision.gameObject.layer == 12)
        {
            jump = true;
            jumpsRemaining = 3;
        }
    }

    private bool IsBusy() //returns true if the player is already attacking
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
                isWalk = true;
            }

            if (!facingRight)
            {
                if (isRunning)
                {
                    StartCoroutine("ScreechStop");
                }
                monsterAnim.flipCharacter();
                facingRight = true;
            }
        }
        else if (direction < 0)
        {
            dir = Vector3.left;

            if (!isWalk)
            {
                isWalk = true;
            }

            if (facingRight)
            {
                if (isRunning)
                {
                    StartCoroutine("ScreechStop");
                }
                monsterAnim.flipCharacter();
                facingRight = false;
            }
        }

        if (isWalk && dir == Vector3.zero)
        {
            //monsterAnim.stopWalking();
            isWalk = false;

            if (isRunning)
            {
                //monsterAnim.screechingStop();
                //isRunning = false;
            }
        }

        //print("dir: " + dir);
        move = dir;
    }

    public void CanPlatformDrop(bool isDoingTheThing) //player is pressing down
    {
        platDropInput = isDoingTheThing;
    }
    private IEnumerator ScreechStop()
    {
        print("my ooop corooutiune");
        screechingStop = true;

        yield return new WaitForSeconds(screechingStopTime);

        screechingStop = false;

        yield break;
    }

    private IEnumerator PlatMakeSolidCooldown()
    {
        platCanSolid = false;

        yield return new WaitForSeconds(platCooldown);

        platCanSolid = true;

        yield break;
    }

    public void SetIsRun(bool frDawg)
    {
        //if (isWalk)
        //{
            isRunning = frDawg;
        //}
    }

    public void Jump()
    {
        //if (!isAttacking)//!IsBusy())
        //{
            jump = true;
        //}
    }

    public void Attack1()
    {
        if (!isAttacking)//!IsBusy())
        {
            isAttacking = true;
            if (!isRunning)
            {
                monsterAnim.attack(1);
            } else
            {
                monsterAnim.dashAttack();
            }
        }
    }

    public void Attack2()
    {
        if (!isAttacking)//!IsBusy())
        {
            isAttacking = true;
            if (!isRunning)
            {
                monsterAnim.attack(2);
            }
            else
            {
                monsterAnim.dashAttack();
            }
        }
    }

    public void Attack3()
    {
        if (!isAttacking)//!IsBusy())
        {
            isAttacking = true;
            if (!isRunning)
            {
                monsterAnim.attack(3);
            }
            else
            {
                monsterAnim.dashAttack();
            }
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