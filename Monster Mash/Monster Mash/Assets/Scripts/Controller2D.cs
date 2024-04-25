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
    [SerializeField] private Collider2D footCircle; //circle collider foot shape for smooth slope movement
    [SerializeField] private Collider2D footBox; //box collider foot shape for ledge landings and no-slip movement
    private Collider2D footCurrent; //which foot shape is set currently, circle or box
    [SerializeField] private bool footIsBox = true;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform playerNose; //debugging visual indicator of player facing left or right, will be deprecated when art is present
    #endregion

    #region horizontal movements variables
    private float playerSpeed = 12.0f; //player walking speed
    private float playerRunSpeed = 15.6f; //run speed
    private float playerAirSpeed = 11.0f; //horizontal speed for forward airborne movement
    private float playerAirBackwardSpeed = 4.0f; //horizontal speed for backwards airborne movement
    private float airDrag; //horizontal midair movement speed, used as a multiplier to velocity.x
    private float fastAirDrag = 0.95f; //airDrag is set to either of these (fast or slow), can add/adjust as needed
    private float slowAirDrag = 0.65f;
    private int move; //tracks player movement input, always set to either +1, 0, or -1 (1 == right, 0 == no movement, -1 == left)
    private Vector3 xMovement; //player velocity is set to this, all horizontal movement is applied thru this vector, vertical is added to this and calculated separate
    private bool facingRight = true; //tracks which way the player faces, doesnt actually rotate the gameobject because that's animation stuff
    private bool isRun = false; //tracks if player is running, player can only run when grounded

    //slope code vars
    private Vector2 capSize; //capCol.size
    private float slopeDetectDistance = 1.5f; //raycast length
    private float slopeDownAngle; //angle of the slope the player is standing on
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private Vector3 slopeNormPerp; //aligned with slope angle
    private Vector3 slopeNormPerpOld;
    [SerializeField] private bool onSlope = false; //true if player is on slope
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;

    //edge detection code
    private float edgeDetectDist = 1.25f;
    [SerializeField] private bool onEdge = false;
    [SerializeField] bool frontHit = false;
    [SerializeField] bool backHit = false;
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
    private float fastFallSpeed = 2.5f; //fast fall when player holds down midair
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

        FootBoxOn();
        footCurrent = footBox;

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
                if (move != 0) //jumping from a stand still (NOT moving horizontally) will feel different from a FORWARD jump, kinda like smash bros
                {
                    airDrag = fastAirDrag;
                } else
                {
                    airDrag = slowAirDrag;
                }

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //jumpsLeft = midAirJumps;
                jumpsLeft--;
                isJumping = true;
                //print("isJump true");
                jumpCounter = 0;

            } else if (jumpsLeft > 0)
            {
                OffEdge(); //ensure player is affected by physics as regular/ reset

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
            if (jumpCounter > jumpTime) { isJumping = false; }

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
            //print("isJump false");
            isJumping = false;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            canPhase = true;
            OffEdge();
            //groundLayerMask &= ~semiSolidLayer; //semi_solid platform is removed from grounded check when canPhase == true

            if (rb.velocity.y < 0.0f)
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * fastFallSpeed * Time.deltaTime;
            }
        }
        else
        {
            canPhase = false;
            //groundLayerMask |= semiSolidLayer; //semi_solid platform is added to grounded check when canPhase == false

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
        DetectSlope(); //detects when player stands on slope, effects xMovement accordingly
        DetectEdge(); //detect if playing stands on platform edge, effects gravity
        ApplyMove(); //sets player velocity
    }

    private void IsGrounded() //updates grounded variable
    {
        Vector2 boxSize = new Vector2(cap.bounds.size.x * 0.95f, 0.75f);

        RaycastHit2D myHit = Physics2D.BoxCast(new Vector2(transform.position.x, cap.bounds.min.y),
            boxSize, 0, Vector2.down, 0.1f, groundLayerMask);

        if (myHit && rb.velocity.y <= 0.0f)
        {
            grounded = true;
            jumpsLeft = 1 + midAirJumps;
        } else
        {
            grounded = false;
        }

        #region ya boy box
        float castDistance = 0.1f;

        Vector2 boxCenter = new Vector2(transform.position.x, cap.bounds.min.y);
        Vector2 boxCastDirection = Vector2.down;

        // Calculate the initial corner positions of the box
        Vector2 bottomLeft = boxCenter - new Vector2(boxSize.x / 2, boxSize.y / 2);
        Vector2 bottomRight = boxCenter + new Vector2(boxSize.x / 2, -boxSize.y / 2);
        Vector2 topLeft = boxCenter - new Vector2(boxSize.x / 2, -boxSize.y / 2);
        Vector2 topRight = boxCenter + new Vector2(boxSize.x / 2, boxSize.y / 2);

        // Draw the box at its initial position
        Debug.DrawLine(bottomLeft, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, topRight, Color.green);
        Debug.DrawLine(topRight, topLeft, Color.green);
        Debug.DrawLine(topLeft, bottomLeft, Color.green);

        // Calculate and draw the box at its final position (after moving in the cast direction)
        Vector2 endBottomLeft = bottomLeft + boxCastDirection * castDistance;
        Vector2 endBottomRight = bottomRight + boxCastDirection * castDistance;
        Vector2 endTopLeft = topLeft + boxCastDirection * castDistance;
        Vector2 endTopRight = topRight + boxCastDirection * castDistance;

        Debug.DrawLine(endBottomLeft, endBottomRight, Color.red);
        Debug.DrawLine(endBottomRight, endTopRight, Color.red);
        Debug.DrawLine(endTopRight, endTopLeft, Color.red);
        Debug.DrawLine(endTopLeft, endBottomLeft, Color.red);

        // Draw lines to show the cast direction and distance
        Debug.DrawLine(bottomLeft, endBottomLeft, Color.blue);
        Debug.DrawLine(bottomRight, endBottomRight, Color.blue);
        Debug.DrawLine(topLeft, endTopLeft, Color.blue);
        Debug.DrawLine(topRight, endTopRight, Color.blue);
        #endregion
    }

    private bool HitWall(float direction) //check if player has anything in front
    {
        if (Physics2D.CapsuleCast(transform.position, new Vector2(cap.size.x, cap.size.y * 0.5f), cap.direction, 0, Vector2.right * direction, 0.1f, groundLayerMask))
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("semi_solid") && onEdge)
        {
            rb.velocity = new Vector2();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("semi_solid"))
        {
            if (canPhase)
            {
                OffEdge();
                groundLayerMask &= ~semiSolidLayer; //semi_solid platform is removed from grounded check when canPhase == true
                Physics2D.IgnoreCollision(footBox, collision.gameObject.GetComponent<BoxCollider2D>(), true);
                Physics2D.IgnoreCollision(footCircle, collision.gameObject.GetComponent<BoxCollider2D>(), true);
            }
        }
        else if (collision.gameObject.CompareTag("slope") && grounded)
        {
            FootCircleOn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("semi_solid"))
        {
            isOnSemiSolid = false;

            Physics2D.IgnoreCollision(footBox, collision.gameObject.GetComponent<BoxCollider2D>(), false);
            Physics2D.IgnoreCollision(footCircle, collision.gameObject.GetComponent<BoxCollider2D>(), false);
        }
    }

    private void FootCircleOn() //turns off box collider, switches out for circle collider
    {
        if (footIsBox) //only run when circle is not yet on and switched
        {
            footCircle.enabled = true;
            footCurrent = footCircle;
            footBox.enabled = false;
            footIsBox = false;
        }
    }

    private void FootBoxOn() //turns off circle collider, switches out for box collider
    {
        if (!footIsBox) //onnly runs when box is not yet on and switched
        {
            footBox.enabled = true;
            footCurrent = footBox;
            footCircle.enabled = false;
            footIsBox = true;
        }
    }


    #region horizontal movement functions
    private void MoveX()
    {
        // Horizontal movement.
        if (Input.GetKey(KeyCode.D))
        {
            move = 1;

            if (grounded) facingRight = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            move = -1;

            if (grounded) facingRight = false;
        }
        else
        {
            move = 0;
        }

        //disable move
        //move = 0;

        if (!HitWall(move))
        {
            if (grounded)
            {
                if ((key1result == 2) || (key2result == 2))
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
        rb.velocity = xMovement + new Vector3(0.0f, rb.velocity.y, 0.0f);

        if (grounded)
        {
            Quaternion fromTo = Quaternion.FromToRotation(slopeNormPerpOld, slopeNormPerp);
            rb.velocity = fromTo * rb.velocity;
        }
    }

    //slope code
    private void DetectSlope()
    {
        if (grounded)
        {
            Vector2 checkPos = transform.position - new Vector3(0.0f, capSize.y / 2);
            DetectSlopeHorizontal(checkPos);
            DetectSlopeVertical(checkPos);
        }
        else
        {
            onSlope = false;
        }
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
                onSlope = true;
            }

            slopeDownAngleOld = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (onSlope && move == 0)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = null;
        }
    }

    private void DetectEdge()
    {
        if (grounded && !canPhase)
        {
            float myDist = 0.65f;
            Vector2 checkPos = transform.position - new Vector3(0.0f, capSize.y / 2);

            RaycastHit2D hitFront = Physics2D.Raycast(checkPos + new Vector2(myDist, 0.0f), -transform.up, edgeDetectDist, groundLayerMask);
            RaycastHit2D hitBack = Physics2D.Raycast(checkPos - new Vector2(myDist, 0.0f), -transform.up, edgeDetectDist, groundLayerMask);

            Debug.DrawRay(checkPos + new Vector2(myDist, 0.0f), (-transform.up) * edgeDetectDist, Color.red);
            Debug.DrawRay(checkPos - new Vector2(myDist, 0.0f), (-transform.up) * edgeDetectDist, Color.blue);

            frontHit = hitFront;
            backHit = hitBack;

            if ((hitFront && !hitBack) || (!hitFront && hitBack))
            {
                OnEdge();
            }
            else
            {
                OffEdge();
            }
        }
        else if (onEdge)
        {
            OffEdge();
        }
    }

    private void OnEdge()
    {
        onEdge = true;

        rb.gravityScale = 0.0f;
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);

        FootBoxOn();
        //rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
    }

    private void OffEdge()
    {
        onEdge = false;
        rb.gravityScale = gravityScale;

        FootCircleOn();
        //rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }
    #endregion
}