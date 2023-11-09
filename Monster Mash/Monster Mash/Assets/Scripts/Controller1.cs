using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller1 : MonoBehaviour
{
    [SerializeField] LayerMask platCast;
    [SerializeField] LayerMask bounceCast;

    private CharacterController controller;
    private Rigidbody rb;
    [SerializeField] private CapsuleCollider capcol;
    [SerializeField] private bool onPlat = false;
    private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer; //get grounded state
    private PlatformFinder platFinder;
    private int jumpsRemaining; // Track the number of jumps left.
    [SerializeField] private float playerSpeedGrounded = 1f;
    [SerializeField] private float playerSpeedAir = 0.989f;
    [SerializeField] private float playerSpeedCurrent = 1.0f;
    [SerializeField] private float playerSpeed = 20f;
    private float playerSpeedRunning = 1.2f;
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

    private monsterAttackSystem attack;
    private bool isWalk = false;
    private bool facingRight = true;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isAttacking = false;
    private bool screechingStop = false;
    private float screechingStopTime = 5f;

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
        RaycastHit hit;
        Debug.DrawRay(transform.position - Vector3.down, Vector3.down * 2f);
        if (Physics.Raycast(transform.position - Vector3.down, Vector3.down, out hit, 2f, platCast) && !Input.GetKey(KeyCode.P))
        {
            hit.collider.isTrigger = false;
        }
        Debug.DrawRay(transform.position + Vector3.up, Vector3.up * 2f);
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.up, out hit, 2f, platCast))
        {
            hit.collider.isTrigger = true;
        }

        if (!knockBack)
        {
            if (!onPlat)
            {
                groundedPlayer = controller.isGrounded;
            }

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

                    if (isRunning)
                    {
                        playerSpeedCurrent = playerSpeedRunning;
                        attack.run();
                    }
                    else
                    {
                        playerSpeedCurrent = playerSpeedGrounded;
                        attack.stopRunning();
                    }
                } else { attack.stopWalking(); }
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

            if (!IsBusy() && !screechingStop)
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
            if (!attack.IsAttacking())
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (Input.GetKey(KeyCode.P))
            {
                collision.collider.isTrigger = true;
            }
        } else if (collision.gameObject.layer == 12)
        {
            jump = true;
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            //onPlat = true;
            if (Input.GetKey(KeyCode.P))
            {
                print("platformyy");
                //Physics.IgnoreCollision(rb.collisionDetectionMode = , other, true);
                //groundedPlayer = false;
                //other.gameObject.GetComponent<Collider>().enabled = false;
                other.enabled = false;
            }
            else
            {
                //groundedPlayer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            //onPlat = false;
            //Physics.IgnoreCollision(capcol, other, false);
        }
    }*/

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
                //attack.walk();
                isWalk = true;
            }

            if (!facingRight)
            {
                if (isRunning)
                {
                    StartCoroutine("ScreechStop");
                }
                attack.flipCharacter();
                facingRight = true;
            }
        }
        else if (direction < 0)
        {
            dir = Vector3.left;

            if (!isWalk)
            {
                //attack.walk();
                isWalk = true;
            }

            if (facingRight)
            {
                if (isRunning)
                {
                    StartCoroutine("ScreechStop");
                }
                attack.flipCharacter();
                facingRight = false;
            }
        }

        if (isWalk && dir == Vector3.zero)
        {
            //attack.stopWalking();
            isWalk = false;

            if (isRunning)
            {
                //attack.screechingStop();
                isRunning = false;
            }
        }

        //print("dir: " + dir);
        move = dir;
    }

    private IEnumerator ScreechStop()
    {
        print("my ooop corooutiune");
        screechingStop = true;

        yield return new WaitForSeconds(screechingStopTime);

        screechingStop = false;

        yield break;
    }

    public void SetIsRun(bool frDawg)
    {
        if (isWalk)
        {
            isRunning = frDawg;
        }
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