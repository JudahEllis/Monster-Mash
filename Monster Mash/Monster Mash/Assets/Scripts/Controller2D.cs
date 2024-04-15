using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller2D : MonoBehaviour
{
    #region GetComponent & manually set variables/ miscellaneous
    private Rigidbody2D rb; //player rigidbody 2D
    private CapsuleCollider2D cap; //basically called to check player size info, frictionless for edge collision to prevent sticking
    [SerializeField]private Collider2D box; //collider for ground, has friction, used for semi solid landing and ground check //change to circle as test for slopes
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform playerNose; //debugging visual indicator of player facing left or right, will be deprecated when art is present
    #endregion

    #region horizontal movements variables
    private float playerSpeed = 12.0f; //player walking speed
    private float playerRunSpeed = 15.6f; //run speed
    private float playerAirSpeed = 11.0f; //horizontal speed for forward airborne movement
    private float playerAirBackwardSpeed = 4.0f; //horizontal speed for backwards airborne movement
    private float decelerate = 1f; //deceleration while skidding to stop (run to turn around)
    private float airDrag; //horizontal midair movement speed, used as a multiplier to velocity.x
    private float fastAirDrag = 0.95f; //airDrag is set to either of these (fast or slow), can add/adjust as needed
    private float slowAirDrag = 0.65f;
    private int move; //tracks player movement input, always set to either +1, 0, or -1 (1 == right, 0 == no movement, -1 == left)
    private Vector3 xMovement; //player velocity is set to this, all horizontal movement is applied thru this vector, vertical is added to this and calculated separate
    private bool facingRight = true; //tracks which way the player faces, doesnt actually rotate the gameobject because that's animation stuff
    private bool isRun = false; //tracks if player is running, player can only run when grounded
    private bool isSkid = false; //tracks if skid coroutine is currently running, should only be active when grounded and skid condition

    //slope code vars
    private Vector2 capSize; //capCol.size
    private float slopeDetectDistance = 1.5f; //raycast length
    private float slopeDownAngle; //angle of the slope the player is standing on
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private Vector3 slopeNormPerp; //aligned with slope angle
    private Vector3 slopeNormPerpOld;
    private bool onSlope = false; //true if player is on slope
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;
    #endregion

    #region all jumping vars
    private float jumpForce = 10f; //player jump power/speed/force
    private float midAirJumpForce = 26f; //jumpForce of midair jump duh 
    private float jumpCounter; //tracks duration of jump button press when grounded (for holding jump button = higher jump) only works when grounded because i say so
    private float jumpTime = 0.2f; //max duration of grounded jump
    private bool isJumping = false; //for grounded jump, longer hold = higher jump, true when grounded jump is happening and player is still holding jump input
    private float jumpMultiplier = 14f; //similar to jumpForce but used in place while player is holding jump input for a higher jump
    private float fallMultiplier = 4f; //when player is falling, velocity.y is multiplied by this var
    private float gravityScale = 6.28f; //Rigidbody2D.gravityScale is set to this, i DO NOT set Physics2D.gravity to this because that's scary DONT GET CONFUSED HAHAHA
    private bool groundedNoGravity = false; //true when the player is grounded so y velocity should not be affected
    private float fastFallSpeed = 3f; //fast fall when player holds down midair
    private int midAirJumps = 1; //possession of wings might change this number, total of midair jumps allowed
    [SerializeField]private int jumpsLeft = 0; //tracks midair jumps left allowed
    [SerializeField]private bool grounded; //check this to see if player is grounded
    [SerializeField]private bool canPhase = false; //true when player is holding down input to phase down thru semi-solid
    [SerializeField] private GameObject[] semiSolids; //array of all (if any) semi solid platforms in scene
    private LayerMask semiSolidLayer; //stored to change for grounded check
    [SerializeField]private bool isOnSemiSolid = false; //tracks when the player is on a semi solid platform
    private BoxCollider2D lastSemi; //BC2D of last semisolid stood on
    #endregion

    #region tracks double tapping of left and right inputs, soon to be deprecated 
    public KeyCode keyToDetect1 = KeyCode.A;
    public KeyCode keyToDetect2 = KeyCode.D;
    public float doubleTapTimeThreshold = 0.3f; // Adjust as needed
    private bool key1WasPressed;
    private bool key2WasPressed;
    private float timeSinceLastPress1;
    private float timeSinceLastPress2;
    private int key1result;
    private int key2result;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cap = GetComponent<CapsuleCollider2D>();
        //box = GetComponent<BoxCollider2D>();

        rb.gravityScale = gravityScale;

        capSize = cap.size;

        #region frictionless collider ignores collisions with semi solids
        semiSolids = GameObject.FindGameObjectsWithTag("semi_solid");

        semiSolidLayer = LayerMask.GetMask("Semi_solid");

        for (int i = 0; i < semiSolids.Length; i++)
        {
            Physics2D.IgnoreCollision(cap, semiSolids[i].GetComponent<BoxCollider2D>(), true);
        }
        #endregion
    }

    private void Update()
    {
        #region debugging visual for player direction, soon to be deprecated
        if (facingRight && playerNose != null)
        {
            playerNose.localPosition = new Vector2(0.5f, playerNose.localPosition.y);
        } else if (playerNose != null)
        {
            playerNose.localPosition = new Vector2(-0.5f, playerNose.localPosition.y);
        }
        #endregion

        #region double tap input detection, soon to be deprecated
        if (Input.GetKeyDown(keyToDetect1) && !Input.GetKey(keyToDetect2))
        {
            if (key1WasPressed && Time.time - timeSinceLastPress1 < doubleTapTimeThreshold)
            {
                // Key was double-tapped
                Debug.Log("Key double-tapped");
                key1result = 2;

                key1WasPressed = false;
            }
            else
            {
                // Key was single-tapped
                Debug.Log("Key single-tapped");
                key1result = 1;

                key1WasPressed = true;
                timeSinceLastPress1 = Time.time;
            }
        }

        if (Input.GetKeyDown(keyToDetect2) && !Input.GetKey(keyToDetect1))
        {
            if (key2WasPressed && Time.time - timeSinceLastPress2 < doubleTapTimeThreshold)
            {
                // Key was double-tapped
                Debug.Log("Key double-tapped");
                key2result = 2;

                key2WasPressed = false;
            }
            else
            {
                // Key was single-tapped
                Debug.Log("Key single-tapped");
                key2result = 1;

                key2WasPressed = true;
                timeSinceLastPress2 = Time.time;
            }
        }
        #endregion

        MoveX(); //calls horizontal movement checks each frame

        #region jump logic! should probably be moved to a new function for real input handler implementation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                if (isSkid) //makes sure skid coroutine is inactive
                {
                    StopCoroutine("SkidToStop");
                    isSkid = false;
                }

                if (move != 0) //jumping from a stand still (NOT moving horizontally) will feel different from a FORWARD jump, kinda like smash bros
                {
                    airDrag = fastAirDrag;
                } else
                {
                    airDrag = slowAirDrag;
                }

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft = midAirJumps;
                isJumping = true;
                print("isJump true");
                jumpCounter = 0;

            } else if (jumpsLeft > 0)
            {
                if (isSkid)
                {
                    StopCoroutine("SkidToStop");
                    isSkid = false;
                }

                if (move != 0)
                {
                    airDrag = fastAirDrag;
                }
                else
                {
                    airDrag = slowAirDrag;
                }

                rb.velocity = new Vector2(rb.velocity.x, midAirJumpForce);
                jumpsLeft--;

                if (move == 1)
                {
                    facingRight = true;
                } else if (move == -1)
                {
                    facingRight = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) { isJumping = false; print("isJump false"); }

            float t = jumpCounter / jumpTime;

            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rb.velocity = new Vector2(rb.velocity.x, jumpMultiplier);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping)
        {
            print("isJump false");
            isJumping = false;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            canPhase = true;
            groundLayerMask &= ~semiSolidLayer; //semi_solid platform is removed from grounded check when canPhase == true

            if (rb.velocity.y < 0.0f)
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * fastFallSpeed * Time.deltaTime;
            }
        }
        else
        {
            canPhase = false;
            groundLayerMask |= semiSolidLayer; //semi_solid platform is added to grounded check when canPhase == false

            if (isOnSemiSolid)
            {
                //Physics2D.IgnoreCollision(box, lastSemi, false);
            }

            if (rb.velocity.y < 0.0f)
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * Time.deltaTime;
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        IsGrounded(); //updates grounded variable
        //DisableYMove();
        DetectSlope(); //detects when player stands on slope
        ApplyMove(); //sets player velocity
    }

    private void IsGrounded() //updates grounded variable
    {
        if (Physics2D.BoxCast(new Vector2(transform.position.x, cap.bounds.min.y), new Vector2(cap.bounds.size.x, 0.25f), 0, Vector2.down, 0.1f, groundLayerMask))
        {
            grounded = true;
            jumpsLeft = 1;
        } else
        {
            grounded = false;
        }
    }

    private void DisableYMove() //checks if gravity should be disabled and sets it
    {
        if (grounded && !isJumping && Mathf.Abs(rb.velocity.x) > 0.0f)
        {
            rb.gravityScale = 0.0f;
        } else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private bool HitWall(float direction) //check if player has anything in front
    {
        if (Physics2D.CapsuleCast(transform.position, new Vector2(cap.size.x, cap.size.y * 0.5f), cap.direction, 0, Vector2.right * direction, 0.1f, groundLayerMask))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("semi_solid"))
        {
            isOnSemiSolid = true;

            lastSemi = collision.gameObject.GetComponent<BoxCollider2D>();

            if (canPhase)
            {
                //PlatformEffector2D plat = collision.gameObject.GetComponent<PlatformEffector2D>();
                Physics2D.IgnoreCollision(box, collision.gameObject.GetComponent<BoxCollider2D>(), true);
                jumpsLeft = midAirJumps;
                //box.enabled = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("semi_solid"))
        {
            print("on plat!");
            if (canPhase)
            {
                //PlatformEffector2D plat = collision.gameObject.GetComponent<PlatformEffector2D>();
                Physics2D.IgnoreCollision(box, collision.gameObject.GetComponent<BoxCollider2D>(), true);
                //box.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("semi_solid"))
        {
            isOnSemiSolid = false;

            //PlatformEffector2D plat = collision.gameObject.GetComponent<PlatformEffector2D>();
            Physics2D.IgnoreCollision(box, collision.gameObject.GetComponent<BoxCollider2D>(), false);
            //box.enabled = true;
        }
    }


    #region horizontal movement functions
    private void MoveX()
    {
        // Horizontal movement.
        if (Input.GetKey(KeyCode.D))
        {
            move = 1;
        } else if (Input.GetKey(KeyCode.A))
        {
            move = -1;
        } else
        {
            move = 0;
        }

        //disable move
        //move = 0;

        if (!HitWall(move))
        {
            if (grounded)
            {
                if ((key1result == 2 && move < 0) || (key2result == 2 && move > 0))
                {
                    isRun = true;
                    xMovement = new Vector3(move * playerRunSpeed, 0.0f, 0.0f);
                }
                else
                {
                    isRun = false;
                    xMovement = new Vector3(move * playerSpeed, 0.0f, 0.0f);
                }
            } else
            {
                if (move == 0 && Mathf.Approximately(rb.velocity.x, 0.0f)) // Jumping in place
                {
                    airDrag = slowAirDrag;
                }
                else if (facingRight)
                {
                    if (move > 0)
                    {
                        xMovement = new Vector3(playerAirSpeed, 0.0f, 0.0f);
                    } else if (move == 0)
                    {
                        xMovement = new Vector3(rb.velocity.x * airDrag, 0.0f, 0.0f);
                    } else if (move < 0)
                    {
                        xMovement = new Vector3(-playerAirBackwardSpeed, 0.0f, 0.0f);
                    }
                } else
                {
                    if (move > 0)
                    {
                        xMovement = new Vector3(playerAirBackwardSpeed, 0.0f, 0.0f);
                    }
                    else if (move == 0)
                    {
                        xMovement = new Vector3(rb.velocity.x * airDrag, 0.0f, 0.0f);
                    }
                    else if (move < 0)
                    {
                        xMovement = new Vector3(-playerAirSpeed, 0.0f, 0.0f);
                    }
                }
            }
        }
    }

    private void ApplyMove()
    {
        if (!isSkid) //x movement
        {
            /*if (grounded && onSlope)
            {
                //xMovement *= slopeNormPerp.x * -1;
                //xMovement.y = playerSpeed * slopeNormPerp.y * -1;
                
            } else*/ if (grounded) {
                if (xMovement.x > 0)
                {
                    facingRight = true;
                }
                else if (xMovement.x < 0)
                {
                    facingRight = false;
                }
            }

            if (grounded && (isRun && (move < 0 && rb.velocity.x > 0) || (move > 0 && rb.velocity.x < 0)))
            {
                isSkid = true;
                StartCoroutine("SkidToStop");
            }
            else
            {
                rb.velocity = xMovement + new Vector3(0.0f, rb.velocity.y, 0.0f);

                Quaternion fromTo = Quaternion.FromToRotation(slopeNormPerpOld, slopeNormPerp);
                rb.velocity = fromTo * rb.velocity;
            }
        }
    }

    private IEnumerator SkidToStop()
    {
        isSkid = true;

        float startPos = transform.position.x;
        float maxSkid = 3.0f;
        int dir = 1;

        if (rb.velocity.x > 0.0f) //find direction to skid
        {
            dir = 1;
            facingRight = false; //face opposite of skid
        }
        else if (rb.velocity.x < 0)
        {
            dir = -1;
            facingRight = true; //face opposite of skid
        }

        float endPos = transform.position.x + (maxSkid * dir);

        float i = 0;

        if (endPos > startPos)
        {
            while (endPos > transform.position.x)
            {
                if (i > 25)
                {
                    break;
                }

                i++;

                rb.velocity = new Vector2(rb.velocity.x * Mathf.Pow(1f - decelerate * Time.deltaTime, 2f), rb.velocity.y);

                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (endPos < transform.position.x)
            {
                if (i > 25)
                {
                    break;
                }

                i++;

                rb.velocity = new Vector2(rb.velocity.x * Mathf.Pow(1f - decelerate * Time.deltaTime, 2f), rb.velocity.y);

                yield return new WaitForFixedUpdate();
            }
        }

        rb.velocity = new Vector2(0, rb.velocity.y);

        isSkid = false;
        yield break;
    }


    //slope code
    private void DetectSlope()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, capSize.y / 2);
        DetectSlopeHorizontal(checkPos);
        DetectSlopeVertical(checkPos);
    }

    private void DetectSlopeHorizontal(Vector2 checkPos)
    {
        RaycastHit2D hitFront = Physics2D.Raycast(checkPos, transform.right, slopeDetectDistance, groundLayerMask);
        RaycastHit2D hitBack = Physics2D.Raycast(checkPos, transform.localEulerAngles, slopeDetectDistance, groundLayerMask);

        if (hitFront)
        {
            onSlope = true;
            slopeSideAngle = Vector2.Angle(hitFront.normal, Vector2.up);
        }
        else if (hitBack)
        {
            onSlope = true;
            slopeSideAngle = Vector2.Angle(hitBack.normal, Vector2.up);
        }
        else
        {
            print("hell no");
            onSlope = false;
            slopeSideAngle = 0.0f;
        }
    }

    private void DetectSlopeVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeDetectDistance, groundLayerMask);

        if (hit)
        {
            slopeNormPerpOld = slopeNormPerp;
            slopeNormPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld || slopeDownAngle != 0f)
            {
                print("slope = true");
                onSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (onSlope && move == 0)
        {
            rb.sharedMaterial = fullFriction;
            print("hell yeah gamer");
        }
        else
        {
            rb.sharedMaterial = null;
        }
    }
    #endregion
}