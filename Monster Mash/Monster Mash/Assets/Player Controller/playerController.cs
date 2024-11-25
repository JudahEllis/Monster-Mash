using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public int playerIndex;
    public monsterAttackSystem myMonster;
    public CapsuleCollider2D bodyCollider;
    public CircleCollider2D smallBodyCollider;
    public BoxCollider2D groundFrictionCollider;
    public brainSFX mySFXBrain;
    //public MeshRenderer standingVisual;
    public MeshRenderer ballVisual;
    public Transform groundCheck;
    public Transform headCheck;
    public LayerMask solidGroundLayer;
    public LayerMask semiSolidGroundLayer;
    private PlayerInput playerInput;
    private PlayerControls playerControlsMap;
    private InputActionMap startingActionMap;
    private InputActionMap UIcontrols;
    //private bool UIcontrolsNeeded = true;
    private InputActionMap monsterControls;
    //private bool monsterControlsNeeded = false;

    //
    bool monsterControllerActive = false;
    public bool facingRight;
    Vector2 leftJoystickVector; //gives us direction on x axis
    float leftJoystickValue; //gives us nuance of input between magnitudes
    //
    private float walkSpeed = 5f;
    private float runSpeed = 25f;
    private float groundedModifer = 1;
    private float airbornModifer = 0.75f;
    private float currentGroundedStateModifier = 1;
    private bool canMove = true;
    public bool isWalking = false;
    public bool isRunning = false;
    //
    public bool grounded = false;
    //public bool onSemiSolid = false;
    private bool isCrouching = false;
    private bool isPhasingThroughPlatform;
    private bool isFastFalling = false;
    bool canJump = true;
    bool jumpButtonReset = false;
    //public bool primedForBigJump = false;
    public int numberOfJumps = 2;
    public int numberOfJumpsLeft = 2;
    private Vector2 jumpValue;
    private bool jumpValueHasBeenRead = true;
    private float bigJumpPower = 65;//80
    private float littleJumpPower = 45;//60
    private float gravityPower;
    //
    private float rollSpeed = 50f;//50
    Vector2 rightJoystickVector; //gives us direction on x axis for roll
    public bool isRolling = false;
    private bool canRoll = true;
    //
    private float dashSpeed = 60f;//60
    public bool isDashing = false;
    private bool canDash = true;
    //
    private bool ledgeHopAvailable = true;
    //
    public bool grabbingWall = false;
    private float wallGrabbingGravityPower = 0.5f;
    private float wallJumpPower = 28f;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerControlsMap = new PlayerControls();
        gravityPower = myRigidbody.gravityScale;
        updateBrainSFX();

        startingActionMap = playerInput.actions.FindActionMap("Starting Action Map");
        UIcontrols = playerInput.actions.FindActionMap("UI Controls");
        monsterControls = playerInput.actions.FindActionMap("Monster Controls");

        if (UIcontrols!= null)
        {
            playerInput.SwitchCurrentActionMap("UI Controls");
            playerControlsMap.StartingActionMap.Disable();
            //print("New Action Map: " + playerInput.currentActionMap);
        }
    }

    public void switchActionMap(string newActionMap)
    {
        //might cut this first section, its a bit unecessary
        if (UIcontrols != null)
        {
            if (newActionMap == "UI Controls" && playerInput.currentActionMap == UIcontrols)
            {
                return;
            }
        }

        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(newActionMap);
        }

        if (newActionMap == "Monster Controls")
        {
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            monsterControllerActive = true;
        }
        else
        {
            myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            monsterControllerActive = false;
        }
    }

    private void Update()
    {
        //This section moves the x axis of the player
        //For moving the y axis of the player, check out the jumping category of the movement section
        //chances are we'll be moving most of this movement to a seperate script so that we can enable or disable with ease and not have all this running all the time
        if (monsterControllerActive)
        {
            if (isGrounded() || isSemiGrounded())
            {
                if (grounded == false)
                {
                    land();
                }
            }

            if (canMove)
            {

                if (isGrounded() || isSemiGrounded())
                {
                    if (grounded == false)
                    {
                        land();
                    }

                    if (isWalking == false && isRunning == false && isPhasingThroughPlatform == false && groundFrictionCollider.enabled == false)
                    {
                        turnOnFriction();
                    }

                    #region Run and Walk on Ground
                    if ((leftJoystickVector.x > 0.9f || leftJoystickVector.x < -0.9f))
                    {
                        //run

                        if (leftJoystickVector.x > 0.9f)
                        {
                            //right
                            myRigidbody.velocity = new Vector2(1 * runSpeed, myRigidbody.velocity.y);
                        }
                        else
                        {
                            //left
                            myRigidbody.velocity = new Vector2(-1 * runSpeed, myRigidbody.velocity.y);
                        }
                    }
                    else if ((leftJoystickVector.x > 0.2f || leftJoystickVector.x < -0.2f))
                    {
                        //walk

                        if (leftJoystickVector.x > 0.2f)
                        {
                            //right
                            myRigidbody.velocity = new Vector2(1 * walkSpeed, myRigidbody.velocity.y);
                        }
                        else
                        {
                            //left
                            myRigidbody.velocity = new Vector2(-1 * walkSpeed, myRigidbody.velocity.y);
                        }
                    }
                    else
                    {
                        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);

                        if (isWalking)
                        {
                            isWalking = false;
                            isRunning = false;
                            stopWalkingVisual();
                            turnOnFriction();
                        }

                        if (isRunning)
                        {
                            isRunning = false;
                            isWalking = false;
                            stopRunningVisual();
                            //turnOnFriction();
                            StartCoroutine(slideToStop());
                        }
                    }
                    #endregion

                }
                else
                {
                    if (grounded)
                    {
                        grounded = false;
                        isRunning = false;
                        isWalking = false;
                        fallVisual();
                    }

                    #region Run and Walk in Air
                    if (leftJoystickVector.x > 0.9f || leftJoystickVector.x < -0.9f)
                    {
                        //run

                        if (leftJoystickVector.x > 0.9f)
                        {
                            //right
                            myRigidbody.velocity = new Vector2(1 * runSpeed / 1.8f, myRigidbody.velocity.y);
                        }
                        else
                        {
                            //left
                            myRigidbody.velocity = new Vector2(-1 * runSpeed / 1.8f, myRigidbody.velocity.y);
                        }
                    }
                    else if (leftJoystickVector.x > 0.2f || leftJoystickVector.x < -0.2f)
                    {
                        //walk

                        if (leftJoystickVector.x > 0.2f)
                        {
                            //right
                            myRigidbody.velocity = new Vector2(1 * walkSpeed / 2, myRigidbody.velocity.y);
                        }
                        else
                        {
                            //left
                            myRigidbody.velocity = new Vector2(-1 * walkSpeed / 2, myRigidbody.velocity.y);
                        }
                    }
                    else
                    {
                        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x - 0.001f, myRigidbody.velocity.y);

                        if (isWalking)
                        {
                            isWalking = false;
                            isRunning = false;
                            stopWalkingVisual();
                            stopRunningVisual();
                            turnOnFriction();
                        }

                        if (isRunning)
                        {
                            isRunning = false;
                            isWalking = false;
                            stopWalkingVisual();
                            stopRunningVisual();
                            turnOnFriction();
                        }
                    }
                    #endregion
   
                }

                if (isBelowSemiGround() && isGrounded() == false)
                {
                    if (isPhasingThroughPlatform == false)
                    {
                        phase();
                        isPhasingThroughPlatform = true;
                        isCrouching = false;
                        isFastFalling = false;
                    }
                }


                #region Jumping
                /*
                if (leftJoystickVector.y > 0.25f && leftJoystickValue > 0.25f)
                {
                    //big jump
                    if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                    {
                        bigJump();
                    }
                }
                else if (leftJoystickVector.y > 0.1f)
                {
                    //little jump
                    if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                    {
                        if (primedForBigJump)
                        {
                            bigJump();
                        }
                        else
                        {
                            littleJump();
                        }
                    }
                }
                */

                if (leftJoystickVector.y > 0.2f)
                {
                    //big jump
                    if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                    {
                        bigJump();
                    }
                }

                if (leftJoystickVector.y < 0.05f && jumpButtonReset == false)
                {
                    jumpButtonReset = true;
                }
                #endregion

                #region Crouching, Phasing through Platforms, and Fast Falling
                if (leftJoystickVector.y < -0.9f && (leftJoystickVector.x < 0.1f && leftJoystickVector.x > -0.1f))
                {
                    //down stick -> either crouch or go through semi solid or fast fall
                    if (isGrounded())
                    {
                        //crouch
                        if (isCrouching == false)
                        {
                            startCrouchVisual();
                            isCrouching = true;
                            isPhasingThroughPlatform = false;
                            isFastFalling = false;

                            if (isRunning || isWalking)
                            {
                                isRunning = false;
                                isWalking = false;
                                StartCoroutine(slideToStop());
                            }
                            else
                            {
                                turnOnFriction();
                            }
                        }
                    }
                    else if (isSemiGrounded())
                    {
                        //fall through platform
                        if (isPhasingThroughPlatform == false)
                        {
                            phase();
                            phaseThroughPlatformVisual();
                            isPhasingThroughPlatform = true;
                            isCrouching = false;
                            isFastFalling = false;
                        }
                    }
                    else
                    {
                        //fast fall

                        /*

                        if (isFastFalling == false)
                        {
                            isFastFalling = true;
                            isPhasingThroughPlatform = false;
                            isCrouching = false;
                        }
                        */
                    }
                }
                else
                {
                    if (isCrouching)
                    {
                        endCrouchVisual();
                        isCrouching = false;
                    }
                }
                #endregion

            }


            if (isRolling)
            {
                #region Rolling Momentum
                if (rightJoystickVector.x > 0.2f)
                {
                    myRigidbody.velocity = new Vector2(1 * rollSpeed, myRigidbody.velocity.y);
                }
                else if (rightJoystickVector.x < -0.2f)
                {
                    myRigidbody.velocity = new Vector2(-1 * rollSpeed, myRigidbody.velocity.y);
                }
                #endregion
            }

            if (isDashing && grabbingWall == false)
            {
                #region Entering Wall Grab
                if (rightJoystickVector.x > 0.1f)
                {
                    myRigidbody.velocity = new Vector2(1 * dashSpeed, 0);

                    if (wallCheck(1) && grabbingWall == false && wallToFloorCheck() == false)
                    {
                        wallGrab(1);
                    }
                }
                else if (rightJoystickVector.x < -0.1f)
                {
                    myRigidbody.velocity = new Vector2(-1 * dashSpeed, 0);

                    if (wallCheck(-1) && grabbingWall == false && wallToFloorCheck() == false)
                    {
                        wallGrab(-1);
                    }
                }

                #endregion
            }
            else if (grabbingWall)
            {
                #region Exiting Wall Grab by land, slipping, or jumping
                if ((isGrounded() || isSemiGrounded()) && grounded == false)
                {
                    land();
                }
                else if (wallCheck(1) == false && wallCheck(-1) == false && grabbingWall)
                {
                    slippedOffWall();
                }

                if (facingRight && leftJoystickVector.x < -0.1f)
                {
                    //jump left
                    wallJump(-1);
                    facingRight = false;
                    flipLeftVisual();
                    bigJumpVisual();
                }
                else if (facingRight == false && leftJoystickVector.x > 0.1f)
                {
                    //jump right
                    wallJump(1);
                    facingRight = true;
                    flipRightVisual();
                    bigJumpVisual();
                }
                #endregion
            }

        }

       
    }

    #region Left Stick - Walk, Run, Big Jump, Little Jump
    public void OnLeftStick(InputAction.CallbackContext context)
    {
        leftJoystickVector = context.ReadValue<Vector2>();
        leftJoystickValue = context.ReadValue<Vector2>().magnitude;

        if (context.canceled)
        {
            jumpButtonReset = true;
        }

        if (context.performed && grabbingWall == false && isDashing == false && isRolling == false)
        {
            if (facingRight == false && leftJoystickVector.x > 0.2f)
            {
                //face right
                facingRight = true;
                flipRightVisual();
            }
            else if (facingRight && leftJoystickVector.x < -0.2f)
            {
                //face left
                facingRight = false;
                flipLeftVisual();
            }
        }

        if (context.performed)
        {
            if (isPhasingThroughPlatform == false && groundFrictionCollider.enabled && isCrouching == false)
            {
                turnOffFriction();
            }

            if (leftJoystickVector.x > 0.9f || leftJoystickVector.x < -0.9f)
            {
                //run

                if (isRunning == false)
                {
                    isRunning = true;
                    isWalking = false;
                    startRunningVisual();
                    turnOffFriction();
                }
            }
            else if (leftJoystickVector.x > 0.2f || leftJoystickVector.x < -0.2f)
            {
                //walk

                if (isWalking == false && isRunning == false)
                {
                    isWalking = true;
                    isRunning = false;
                    startWalkingVisual();
                    turnOffFriction();
                }
            }
        }

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, solidGroundLayer);
    }

    private bool isSemiGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, semiSolidGroundLayer);
    }

    private bool isBelowSemiGround()
    {
        return Physics2D.OverlapCircle(headCheck.position, 1f, semiSolidGroundLayer);
    }

    private void land()
    {
        grounded = true;
        numberOfJumpsLeft = numberOfJumps;
        StopCoroutine(jumpRecharge());
        bodyCollider.enabled = true;
        smallBodyCollider.enabled = true;
        isPhasingThroughPlatform = false;
        isFastFalling = false;
        myRigidbody.gravityScale = gravityPower;
        canJump = true;
        canDash = true;
        canRoll = true;
        grabbingWall = false;
        canMove = true;
        isRunning = false;
        isWalking = false;
        //primedForBigJump = false;
        landVisual();
        playLandSound();

        if (isRunning || isWalking)
        {
            turnOffFriction();
        }
        else if (leftJoystickVector.x < 0.2f && leftJoystickVector.x > -0.2f)
        {
            turnOnFriction();
        }
    }

    private void bigJump()
    {
        if (numberOfJumpsLeft > 0)
        {
            if (numberOfJumpsLeft == numberOfJumps)
            {
                playJumpSound();
                bigJumpVisual();
            }
            else
            {
                playDoubleJumpSound();
                doubleJumpVisual();
            }

            numberOfJumpsLeft--;
            grounded = false;
            jumpButtonReset = false;
            isRunning = false;
            isWalking = false;
            //primedForBigJump = true;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, bigJumpPower);
            StartCoroutine(jumpRecharge());

            if (isBelowSemiGround())
            {
                phase();
                isPhasingThroughPlatform = true;
                isCrouching = false;
                isFastFalling = false;
            }
        }
    }

    /*
    private void littleJump()
    {
        if (numberOfJumpsLeft > 0)
        {
            if (numberOfJumpsLeft == numberOfJumps)
            {
                playJumpSound();
            }
            else
            {
                playDoubleJumpSound();
            }

            numberOfJumpsLeft--;
            grounded = false;
            jumpButtonReset = false;
            primedForBigJump = true;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, littleJumpPower);
            StartCoroutine(jumpRecharge());
            littleJumpVisual();
            playJumpSound();

            if (isBelowSemiGround())
            {
                phase();
                isPhasingThroughPlatform = true;
                isCrouching = false;
                isFastFalling = false;
            }
        }

    }
    */

    IEnumerator jumpRecharge()
    {
        canJump = false;
        yield return new WaitForSeconds(0.2f);
        canJump = true;
    }

    #endregion

    #region Right Stick - Dash and Roll
    public void OnRightStick(InputAction.CallbackContext context)
    {
        rightJoystickVector = context.ReadValue<Vector2>();

        if (context.performed && grabbingWall == false && isDashing == false && isRolling == false)
        {
            if (facingRight == false && rightJoystickVector.x > 0.2f)
            {
                //face right
                facingRight = true;
                flipRightVisual();
            }
            else if (facingRight && rightJoystickVector.x < -0.2f)
            {
                //face left
                facingRight = false;
                flipLeftVisual();
            }
        }

        if (context.performed && (canDash || canRoll))
        {

            if (isGrounded() || isSemiGrounded())
            {
                if (canRoll && isRolling == false)
                {
                    if (rightJoystickVector.x > 0.2f || rightJoystickVector.x < -0.2f)
                    {
                        StartCoroutine(rollTime());
                    }
                }
            }
            else
            {
                if (canDash && isDashing == false)
                {
                    if (rightJoystickVector.x > 0.2f || rightJoystickVector.x < -0.2f)
                    {
                        StartCoroutine(dashTime());

                        if (facingRight == false && rightJoystickVector.x > 0.2f)
                        {
                            //face right
                            facingRight = true;
                            flipRightVisual();
                        }
                        else if (facingRight && rightJoystickVector.x < -0.2f)
                        {
                            //face left
                            facingRight = false;
                            flipLeftVisual();
                        }
                    }
                }
            }

        }

    }

    IEnumerator rollTime()
    {
        //start
        turnOffFriction();
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        isRolling = true;
        canMove = false;
        canRoll = false;
        canDash = false;
        bodyCollider.enabled = false;
        smallBodyCollider.enabled = true;
        //standingVisual.enabled = false;
        //ballVisual.enabled = true;
        rollVisual();
        yield return new WaitForSeconds(0.125f);
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        bodyCollider.enabled = true;
        smallBodyCollider.enabled = true;
        //turnOnFriction();
        //standingVisual.enabled = true;
        //ballVisual.enabled = false;
        isRolling = false;
        isDashing = false;
        canMove = true;
        canDash = true;
        isRunning = false;
        isWalking = false;
        StartCoroutine(rollRecharge());
        //stop
    }

    IEnumerator rollRecharge()
    {
        yield return new WaitForSeconds(1f);
        canRoll = true;
    }

    IEnumerator dashTime()
    {
        //start
        turnOffFriction();
        isDashing = true;
        canMove = false;
        canDash = false;
        canRoll = false;
        bodyCollider.enabled = false;
        smallBodyCollider.enabled = true;
        groundFrictionCollider.enabled = true;
        //standingVisual.enabled = false;
        //ballVisual.enabled = true;
        myRigidbody.gravityScale = 0;
        startDashAttackVisual();
        yield return new WaitForSeconds(0.25f);
        isRunning = false;
        isWalking = false;

        if (grabbingWall == false)
        {
            myRigidbody.gravityScale = gravityPower;
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            bodyCollider.enabled = true;
            smallBodyCollider.enabled = true;
            //turnOnFriction();
            //standingVisual.enabled = true;
            //ballVisual.enabled = false;
            isDashing = false;
            isRolling = false;
            grabbingWall = false;
            canRoll = true;
            canMove = true;
            endDashAttackVisual();
        }
    }

    private bool wallCheck(int direction)
    {
        return Physics2D.Raycast(transform.position, direction * transform.right, 2f, solidGroundLayer);
    }

    private bool wallToFloorCheck()
    {
        return Physics2D.Raycast(transform.position, -transform.up, 3f, solidGroundLayer);
    }

    private void wallGrab(int direction)
    {
        StopCoroutine(dashTime());
        endDashAttackVisual();
        wallGrabVisual();
        canMove = false;
        grabbingWall = true;
        canRoll = false;
        canDash = false;
        isRolling = false;
        isDashing = false;
        myRigidbody.gravityScale = wallGrabbingGravityPower;
        myRigidbody.velocity = new Vector2(0, 0);
        //print("Grabbing Wall");
        //change visual based on direction
    }
    private void wallJump(int direction)
    {
        myRigidbody.gravityScale = gravityPower;
        jumpButtonReset = false;
        myRigidbody.velocity = new Vector2(direction * wallJumpPower, littleJumpPower);
        StartCoroutine(jumpRecharge());
        bodyCollider.enabled = true;
        //standingVisual.enabled = true;
        //ballVisual.enabled = false;
        isDashing = false;
        isRolling = false;
        canRoll = true;
        canDash = true;
        grabbingWall = false;
        canMove = true;
    }

    private void slippedOffWall()
    {
        myRigidbody.gravityScale = gravityPower;
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        bodyCollider.enabled = true;
        smallBodyCollider.enabled = true;
        //standingVisual.enabled = true;
        //ballVisual.enabled = false;
        isDashing = false;
        isRolling = false;
        canRoll = true;
        canDash = true;
        grabbingWall = false;
        canMove = true;
    }


    #endregion

    #region Monster Attack System Visual Communication

    private void flipLeftVisual()
    {
        myMonster.flipLeft();
    }

    private void flipRightVisual()
    {
        myMonster.flipRight();
    }

    private void startRunningVisual()
    {
        myMonster.run();
    }

    private void stopRunningVisual()
    {
        myMonster.stopRunning();
    }

    private void startWalkingVisual()
    {
        myMonster.walk();
    }

    private void stopWalkingVisual()
    {
        myMonster.stopWalking();
    }

    private void bigJumpVisual()
    {
        myMonster.jump();
    }

    private void doubleJumpVisual()
    {
        myMonster.doubleJump();
    }

    private void phaseThroughPlatformVisual()
    {
        myMonster.goThroughPlatform();
    }

    private void fallVisual()
    {
        myMonster.walkToFall();
    }

    private void landVisual()
    {
        myMonster.land();
    }

    private void startCrouchVisual()
    {
        myMonster.crouch();
    }

    private void endCrouchVisual()
    {
        myMonster.stopCrouching();
    }

    private void startDashAttackVisual()
    {
        myMonster.dashAttack();
    }

    private void endDashAttackVisual()
    {
        myMonster.endDashAttack();
    }

    private void wallGrabVisual()
    {
        myMonster.wallGrabbedCorrections();
    }

    private void rollVisual()
    {
        myMonster.roll();
    }

    private void crouchVisual()
    {
        myMonster.crouch();
    }
    #endregion

    #region Monster Attack System Audio Communication
    private void updateBrainSFX()
    {
        mySFXBrain.updateBrainSounds();
    }

    private void playJumpSound()
    {
        /*
        int randomJumpInt = UnityEngine.Random.Range(1, 3);

        if (randomJumpInt == 1)
        {
            mySFXBrain.playJump1Sound();
        }
        else if (randomJumpInt == 2)
        {
            mySFXBrain.playJump2Sound();
        }
        else
        {
            mySFXBrain.playJump3Sound();
        }
        */
        mySFXBrain.playJumpSound();
    }

    private void playDoubleJumpSound()
    {
        int randomDoubleJumpInt = UnityEngine.Random.Range(1, 3);

        if (randomDoubleJumpInt == 1)
        {
            mySFXBrain.playDoubleJump1Sound();
        }
        else if (randomDoubleJumpInt == 2)
        {
            mySFXBrain.playDoubleJump2Sound();
        }
        else
        {
            mySFXBrain.playDoubleJump3Sound();
        }
    }

    private void playLandSound()
    {
        /*
        int randomLandInt = UnityEngine.Random.Range(1, 3);

        if (randomLandInt == 1)
        {
            mySFXBrain.playLand1Sound();
        }
        else if (randomLandInt == 2)
        {
            mySFXBrain.playLand2Sound();
        }
        else
        {
            mySFXBrain.playLand3Sound();
        }
        */

        mySFXBrain.playLandSound();
    }

    #endregion

    public void turnOnFriction()
    {
        if (isRunning || isWalking)
        {
            return;
        }
        else
        {
            groundFrictionCollider.enabled = true;
        }
    }

    public void turnOffFriction()
    {
        groundFrictionCollider.enabled = false;
    }

    private void phase()
    {
        bodyCollider.enabled = false;
        smallBodyCollider.enabled = false;
        groundFrictionCollider.enabled = false;
        isPhasingThroughPlatform = true;
    }

    private void antiPhase()
    {
        bodyCollider.enabled = true;
        smallBodyCollider.enabled = true;
        //groundFrictionCollider.enabled = true;
        isPhasingThroughPlatform = false;
    }

    IEnumerator slideToStop()
    {
        yield return new WaitForSeconds(0.1f);

        if ((leftJoystickVector.x < 0.2f && leftJoystickVector.x > -0.2f))
        {
            turnOnFriction();
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPhasingThroughPlatform && collision.gameObject.tag == "Semi Solid")
        {
            antiPhase();
        }
    }
}
