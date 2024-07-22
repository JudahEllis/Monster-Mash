using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller2D : MonoBehaviour
{
    #region Variable Declaration
    //GetComponent & manually set variables/ miscellaneous
    private Rigidbody2D rb; //player rigidbody 2D
    private CapsuleCollider2D cap; //basically called to check player size info, frictionless for edge collision to prevent sticking
    [SerializeField] private Collider2D footCircle; //circle collider foot shape for smooth slope movement
    [SerializeField] private Collider2D footBox; //box collider foot shape for ledge landings and no-slip movement
    private Collider2D footCurrent; //which foot shape is set currently, circle or box
    [SerializeField] private bool footIsBox = true;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform playerNose; //debugging visual indicator of player facing left or right, will be deprecated when art is present

    //horizontal movements variables
    [SerializeField] private float playerCrawlSpeed = 0.33f;
    [SerializeField] private float playerSlowSpeed = 0.5f;
    [SerializeField] private float playerSpeed = 12.0f; //player walking speed
    [SerializeField] private float playerRunSpeed = 15.6f; //run speed
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
    private bool isOnSlope = false;

    //edge detection code
    private float edgeDetectDist = 0.95f;
    [SerializeField] private bool onEdge = false;
    [SerializeField] bool frontHit = false;
    [SerializeField] bool backHit = false;

    //all jumping vars
    [SerializeField] private float jumpForce = 10f; //player jump power/speed/force
    [SerializeField] private float midAirJumpForce = 26f; //jumpForce of midair jump duh 
    private float jumpCounter; //tracks duration of jump button press when grounded (for holding jump button = higher jump) only works when grounded because i say so
    private float jumpTime = 0.3f; //max duration of grounded jump
    [SerializeField] private bool isJumping = false; //for grounded jump, longer hold = higher jump, true when grounded jump is happening and player is still holding jump input
    [SerializeField] private float jumpMultiplier = 14f; //similar to jumpForce but used in place while player is holding jump input for a higher jump
    [SerializeField] private float fallMultiplier = 4f; //when player is falling, velocity.y is multiplied by this var
    private float gravityScale = 6.28f; //Rigidbody2D.gravityScale is set to this, i DO NOT set Physics2D.gravity to this because that's scary DONT GET CONFUSED HAHAHA
    private float fastFallSpeed = 2.5f; //fast fall when player holds down midair
    private int midAirJumps = 1; //possession of wings might change this number, total of midair jumps allowed
    [SerializeField] private int jumpsLeft = 0; //tracks midair jumps left allowed
    [SerializeField] private bool grounded; //check this to see if player is grounded
    [SerializeField] private bool canPhase = false; //true when player is holding down input to phase down thru semi-solid
    [SerializeField] private GameObject[] semiSolids; //array of all (if any) semi solid platforms in scene
    private LayerMask semiSolidLayer; //stored to change for grounded check
    [SerializeField] private bool isOnSemiSolid = false; //tracks when the player is on a semi solid platform
    private BoxCollider2D lastSemi; //BC2D of last semisolid stood on
    [SerializeField] private bool jumpButtonReleased = false; //inputhandler doesn't have GetKeyDown, so i must manually check button releases

    //input_handler stuffy stuff
    private bool hasInputHandler = false;
    [SerializeField]private input_handler myInput;

    //anims/ Monster Attack System script stuff
    [SerializeField] private monsterAttackSystem monsterAtt;
    [SerializeField] private bool hasMonsterAtt = false;
    [SerializeField] private bool monsterArtFacingRight = false;
    [SerializeField] private bool monsterArtIsWalk = false;
    [SerializeField] private bool monsterArtIsRun = false;

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

    #region Start & Updates

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        cap = GetComponent<CapsuleCollider2D>();

        FootBoxOn();
        footCurrent = footBox;

        rb.gravityScale = gravityScale;

        capSize = cap.size;

        if (myInput == null && FindObjectOfType<input_handler>())
        {
            //myInput = FindObjectOfType<input_handler>();
            hasInputHandler = true;
        }
        else { hasInputHandler = false; }

        if (transform.GetChild(1).GetComponentInChildren<monsterAttackSystem>())
        {
            print("locked in MonsterAttackSystem, Gamer!");
            monsterAtt = transform.GetChild(1).GetComponentInChildren<monsterAttackSystem>();
        }

        if (monsterAtt != null)
        {
            StartCoroutine("SetUpMonsterAttDelay");
        }
        else { hasMonsterAtt = false; }

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
        RotatePlayer();

        #region double tap input detection, soon to be deprecated
        if (!hasInputHandler && (Input.GetKeyDown(keyToDetect1) && !Input.GetKey(keyToDetect2)))
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

        if (!hasInputHandler && (Input.GetKeyDown(keyToDetect2) && !Input.GetKey(keyToDetect1)))
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
        if (hasInputHandler && !myInput.GetX_Button())
        {
            jumpButtonReleased = true;
        }

        if ((!hasInputHandler && Input.GetKeyDown(KeyCode.Space)) || (hasInputHandler && myInput.GetX_Button()))
        {
            if (grounded && jumpsLeft == 2)
            {
                if (move != 0) //jumping from a stand still (NOT moving horizontally) will feel different from a FORWARD jump, kinda like smash bros
                {
                    airDrag = fastAirDrag;
                }
                else
                {
                    airDrag = slowAirDrag;
                }

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpsLeft--;
                isJumping = true;
                jumpCounter = 0;
                jumpButtonReleased = false;

                if (hasMonsterAtt) monsterAtt.jump();
            }
            else if (jumpsLeft > 0 && (!hasInputHandler || jumpButtonReleased))
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
                }
                else if (move == -1)
                {
                    facingRight = false;
                }

                if (hasMonsterAtt) monsterAtt.jump();
            }
        }

        if (((!hasInputHandler && Input.GetKeyUp(KeyCode.Space)) || (hasInputHandler && !myInput.GetX_Button())) && isJumping)
        {
            //print("isJump false");
            isJumping = false;
        }

        if ((!hasInputHandler && Input.GetAxis("Vertical") < 0) || (hasInputHandler && myInput.GetLeft_JoyStick().y <= -0.44f))
        {
            canPhase = true;
            OffEdge();

            if (rb.velocity.y < 0.0f)
            {
                rb.velocity -= -Physics2D.gravity * fallMultiplier * fastFallSpeed * Time.deltaTime;
                if (hasMonsterAtt) monsterAtt.forceFall();
            }
        }
        else
        {
            canPhase = false;

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

    #endregion

    #region Collisions

    private void IsGrounded() //updates grounded variable
    {
        Vector2 boxSize = new Vector2(transform.localScale.x * 0.95f, transform.localScale.y * 0.25f);

        RaycastHit2D myHit = Physics2D.BoxCast(new Vector2(transform.position.x, cap.bounds.min.y),
            boxSize, 0, Vector2.down, transform.localScale.y * 0.1f, groundLayerMask);

        if (myHit && !isJumping)//&& rb.velocity.y <= 0.0f)
        {
            if (!grounded && hasMonsterAtt)
            {
                monsterAtt.land();
            }

            grounded = true;
            jumpsLeft = 1 + midAirJumps;
        }
        else
        {
            grounded = false;
            isOnSlope = false;
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
            //rb.velocity = new Vector2();
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

    #endregion

    #region horizontal movement functions

    private void RotatePlayer()
    {
        if (facingRight && playerNose != null)
        {
            playerNose.localPosition = new Vector2(0.5f, playerNose.localPosition.y);

            if (hasMonsterAtt && !monsterArtFacingRight)
            {
                monsterAtt.flipCharacter();
                monsterArtFacingRight = true;
            }
        }
        else if (playerNose != null)
        {
            playerNose.localPosition = new Vector2(-0.5f, playerNose.localPosition.y);

            if (hasMonsterAtt && monsterArtFacingRight)
            {
                monsterAtt.flipCharacter();
                monsterArtFacingRight = false;
            }
        }
    }

    private void AnimatePlayerWalk(bool walk)
    {
        if (hasMonsterAtt && walk && !monsterArtIsWalk)
        {
            monsterArtIsWalk = true;
            monsterAtt.walk();
        }
        else if (hasMonsterAtt && !walk && monsterArtIsWalk)
        {
            monsterArtIsWalk = false;
            monsterAtt.stopWalking();
        }
    }

    private void AnimatePlayerRun(bool run)
    {
        if (hasMonsterAtt && run && !monsterArtIsRun)
        {
            monsterArtIsRun = true;
            monsterAtt.run();
        }
        else if (hasMonsterAtt && !run && monsterArtIsRun)
        {
            monsterArtIsRun = false;
            monsterAtt.stopRunning();
        }
    }

    private void MoveX()
    {
        // Horizontal movement.
        if ((!hasInputHandler && Input.GetKey(KeyCode.D)) || (hasInputHandler && myInput.GetLeft_JoyStick().x > 0.1f))
        {
            move = 1;

            if (grounded) facingRight = true;

        }
        else if ((!hasInputHandler && Input.GetKey(KeyCode.A)) || (hasInputHandler && myInput.GetLeft_JoyStick().x < -0.1f))
        {
            move = -1;

            if (grounded) facingRight = false;
        }
        else
        {
            move = 0;

            //AnimatePlayerWalk(false);
            //AnimatePlayerWalk(false);
        }

        //disable move
        //move = 0;

        if (!HitWall(move))
        {
            //if (grounded)
            //{
            if ((!hasInputHandler && (key1result == 2 || key2result == 2)) || (hasInputHandler && myInput.GetLeft_Joystick_Click()) && grounded)
            {
                isRun = true;
                xMovement = new Vector3(move * playerRunSpeed, 0.0f, 0.0f);
            }
            else
            {
                isRun = false;
                xMovement = new Vector3(move * playerSpeed, 0.0f, 0.0f);
            }
            //}
            /*else
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
                    }
                    else if (move == 0)
                    {
                        xMovement = new Vector3(rb.velocity.x * airDrag, 0.0f, 0.0f);
                    }
                    else if (move < 0)
                    {
                        xMovement = new Vector3(-playerAirBackwardSpeed, 0.0f, 0.0f);
                    }
                }
                else
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
        */
        }
    }

    private void ApplyMove()
    {
        float xInput = 1f;

        if (hasInputHandler)
        {
            xInput = Mathf.Abs(myInput.GetLeft_JoyStick().x);

            if (xInput >= 0.69f || (!grounded && xInput <= 0.15f))
            {
                xInput = 1f;

                if (grounded)
                {
                    AnimatePlayerRun(true);
                }
            }
            else if (xInput < 0.69f && xInput >= 0.4f)
            {
                xInput = playerSlowSpeed;

                if (grounded)
                {
                    AnimatePlayerRun(true);
                }
            }
            else if (xInput < 0.4f && xInput > 0.15f)
            {
                xInput = playerCrawlSpeed;

                if (grounded)
                {
                    AnimatePlayerWalk(true);
                }
            }
            else
            {
                AnimatePlayerWalk(false);
                AnimatePlayerRun(false);
            }
        }

        rb.velocity = xMovement * xInput + new Vector3(0.0f, rb.velocity.y, 0.0f);

        if (grounded && isOnSlope)
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
        RaycastHit2D hitFront = Physics2D.Raycast(checkPos, transform.right, transform.localScale.x * slopeDetectDistance, groundLayerMask);
        RaycastHit2D hitBack = Physics2D.Raycast(checkPos, transform.localEulerAngles, transform.localScale.x * slopeDetectDistance, groundLayerMask);

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
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, transform.localScale.y * slopeDetectDistance, groundLayerMask);

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
            isOnSlope = true;
        }
        else
        {
            rb.sharedMaterial = null;
            isOnSlope = false;
        }
    }

    private void DetectEdge()
    {
        if (grounded && !canPhase && !isJumping && rb.velocity.y <= 0.0f)
        {
            float myDist = 0.65f;
            Vector2 checkPos = new Vector3(transform.position.x, cap.bounds.min.y);

            RaycastHit2D hitFront = Physics2D.Raycast(checkPos + new Vector2(transform.localScale.x * myDist, 0.0f),
                -transform.up, transform.localScale.y * edgeDetectDist, groundLayerMask);

            RaycastHit2D hitBack = Physics2D.Raycast(checkPos - new Vector2(transform.localScale.x * myDist, 0.0f),
                -transform.up, transform.localScale.y * edgeDetectDist, groundLayerMask);

            Debug.DrawRay(checkPos + new Vector2(transform.localScale.x * myDist, 0.0f), transform.localScale.y * (-transform.up) * edgeDetectDist, Color.red);
            Debug.DrawRay(checkPos - new Vector2(transform.localScale.x * myDist, 0.0f), transform.localScale.y * (-transform.up) * edgeDetectDist, Color.blue);

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

    #region MonsterAttackSystem

    private IEnumerator SetUpMonsterAttDelay() //delays Controller2D from accessing MonsterAttackSystem Script before it is set up properly
    {
        yield return new WaitForSeconds(3f);

        hasMonsterAtt = true;

        yield break;
    }

    #endregion
}