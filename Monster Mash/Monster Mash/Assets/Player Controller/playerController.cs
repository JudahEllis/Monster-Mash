using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public int playerIndex;
    public CapsuleCollider2D bodyCollider;
    public MeshRenderer standingVisual;
    public MeshRenderer ballVisual;
    public Transform groundCheck;
    public LayerMask solidGroundLayer;
    private PlayerInput playerInput;
    private PlayerControls playerControlsMap;
    private InputActionMap startingActionMap;
    private InputActionMap UIcontrols;
    //private bool UIcontrolsNeeded = true;
    private InputActionMap monsterControls;
    //private bool monsterControlsNeeded = false;

    //
    bool monsterControllerActive = false;
    Vector2 joystickVector; //gives us direction on x axis
    float joystickValue; //gives us nuance of input between magnitudes
    //
    private float walkSpeed = 10f;
    private float runSpeed = 38f;
    private float groundedModifer = 1;
    private float airbornModifer = 0.75f;
    private float currentGroundedStateModifier = 1;
    public bool canMove = true;
    //
    public bool grounded = false;
    bool canJump = true;
    bool jumpButtonReset = false;
    public int numberOfJumps = 2;
    public int numberOfJumpsLeft = 2;
    private Vector2 jumpValue;
    private bool jumpValueHasBeenRead = true;
    private float bigJumpPower = 80;
    private float littleJumpPower = 60;
    private float gravityPower;
    //
    private float rollSpeed = 50f;
    Vector2 rightJoystickVector; //gives us direction on x axis for roll
    private bool isRolling = false;
    private bool canRoll = true;
    //
    private float dashSpeed = 60f;
    private bool isDashing = false;
    private bool canDash = true;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerControlsMap = new PlayerControls();
        gravityPower = myRigidbody.gravityScale;

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
        if (monsterControllerActive)
        {
            if (canMove)
            {

                if (isGrounded() && grounded == false)
                {
                    land();
                }

                if (isGrounded() == false && grounded)
                {
                    grounded = false; //falling
                }

                if (joystickValue >= 0.92f)
                {
                    if (joystickVector.y >= 0.2f)
                    {
                        if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                        {
                            bigJump();
                        }
                    }
                    else if (joystickVector.y >= 0.15f)
                    {
                        if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                        {
                            littleJump();
                        }
                    }

                    //run
                    if (isGrounded())
                    {
                        if (joystickVector.x > 0.2f)
                        {
                            myRigidbody.velocity = new Vector2(1 * runSpeed, myRigidbody.velocity.y);
                        }
                        else if (joystickVector.x < -0.2f)
                        {
                            myRigidbody.velocity = new Vector2(-1 * runSpeed, myRigidbody.velocity.y);
                        }
                    }
                    else
                    {
                        if (joystickVector.x > 0.2f)
                        {
                            myRigidbody.velocity = new Vector2(1 * runSpeed / 1.8f, myRigidbody.velocity.y);
                        }
                        else if (joystickVector.x < -0.2f)
                        {
                            myRigidbody.velocity = new Vector2(-1 * runSpeed / 1.8f, myRigidbody.velocity.y);
                        }
                    }

                }
                else if (joystickValue > 0.1f)
                {
                    if (joystickVector.y >= 0.1f && joystickVector.y < 0.35f)
                    {
                        //little jump
                        if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                        {
                            littleJump();
                            //print(joystickVector.y);
                        }
                    }
                    else if (joystickVector.y >= 0.35f)
                    {
                        //big jump
                        if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                        {
                            bigJump();
                            //print(joystickVector.y);
                        }
                    }


                    if (isGrounded())
                    {
                        if (joystickVector.x > 0.1f)
                        {
                            myRigidbody.velocity = new Vector2(1 * walkSpeed, myRigidbody.velocity.y);
                        }
                        else if (joystickVector.x < -0.1f)
                        {
                            myRigidbody.velocity = new Vector2(-1 * walkSpeed, myRigidbody.velocity.y);
                        }
                    }
                    else
                    {
                        if (joystickVector.x > 0.1f)
                        {
                            myRigidbody.velocity = new Vector2(1 * walkSpeed / 2, myRigidbody.velocity.y);
                        }
                        else if (joystickVector.x < -0.1f)
                        {
                            myRigidbody.velocity = new Vector2(-1 * walkSpeed / 2, myRigidbody.velocity.y);
                        }
                    }
                    //walk

                }
                else
                {
                    //myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
                    if (grounded)
                    {
                        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
                    }
                    else
                    {
                        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x - 0.001f, myRigidbody.velocity.y);
                    }
                }

                ///*
                if (joystickVector.y < 0.05f && jumpButtonReset == false)
                {
                    jumpButtonReset = true;
                }
                //*/
            }

            if (isRolling)
            {
                if (rightJoystickVector.x > 0.1f)
                {
                    myRigidbody.velocity = new Vector2(1 * rollSpeed, myRigidbody.velocity.y);
                }
                else if (rightJoystickVector.x < -0.1f)
                {
                    myRigidbody.velocity = new Vector2(-1 * rollSpeed, myRigidbody.velocity.y);
                }
            }

            if (isDashing)
            {
                if (rightJoystickVector.x > 0.1f)
                {
                    myRigidbody.velocity = new Vector2(1 * dashSpeed, 0);
                }
                else if (rightJoystickVector.x < -0.1f)
                {
                    myRigidbody.velocity = new Vector2(-1 * dashSpeed, 0);
                }
            }
        }

       
    }

    #region Movement

    //lets rename this to onLeftStick
    public void OnMove(InputAction.CallbackContext context)
    {
        //we need to determine if this is a walk or a jump
        //var joystickValue = context.ReadValue<Vector2>();
        joystickVector = context.ReadValue<Vector2>();
        joystickValue = context.ReadValue<Vector2>().magnitude;

        if (context.canceled)
        {
            jumpButtonReset = true;
        }

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, solidGroundLayer);
    }

    private void land()
    {
        grounded = true;
        numberOfJumpsLeft = numberOfJumps;
        StopCoroutine(jumpRecharge());
        canJump = true;
        //jumpButtonReset = true;
        canDash = true;
        canRoll = true;
    }

    private void bigJump()
    {
        if (numberOfJumpsLeft > 0)
        {
            numberOfJumpsLeft--;
            grounded = false;
            jumpButtonReset = false;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, bigJumpPower);
            StartCoroutine(jumpRecharge());
        }
    }

    private void littleJump()
    {
        if (numberOfJumpsLeft > 0)
        {
            numberOfJumpsLeft--;
            grounded = false;
            jumpButtonReset = false;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, littleJumpPower);
            StartCoroutine(jumpRecharge());
        }

    }

    IEnumerator jumpRecharge()
    {
        canJump = false;
        yield return new WaitForSeconds(0.2f);
        canJump = true;
    }

    #endregion

    //lets rename this to onRightStick
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightJoystickVector = context.ReadValue<Vector2>();

            if (isGrounded())
            {
                if (canRoll)
                {
                    if (rightJoystickVector.x > 0.1f || rightJoystickVector.x < -0.1f)
                    {
                        StartCoroutine(rollTime());
                    }
                }
            }
            else
            {
                if (canDash)
                {
                    if (rightJoystickVector.x > 0.1f || rightJoystickVector.x < -0.1f)
                    {
                        StartCoroutine(dashTime());
                    }
                }
            }

        }

    }

    IEnumerator rollTime()
    {
        //start
        isRolling = true;
        canMove = false;
        canRoll = false;
        canDash = false;
        bodyCollider.enabled = false;
        standingVisual.enabled = false;
        ballVisual.enabled = true;
        yield return new WaitForSeconds(0.25f);
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        bodyCollider.enabled = true;
        standingVisual.enabled = true;
        ballVisual.enabled = false;
        isRolling = false;
        isDashing = false;
        canMove = true;
        canDash = true;
        StartCoroutine(rollRecharge());
        //stop
    }

    IEnumerator rollRecharge()
    {
        yield return new WaitForSeconds(0.5f);
        canRoll = true;
    }

    IEnumerator dashTime()
    {
        //start
        isDashing = true;
        canMove = false;
        canDash = false;
        canRoll = false;
        bodyCollider.enabled = false;
        standingVisual.enabled = false;
        ballVisual.enabled = true;
        myRigidbody.gravityScale = 0;
        yield return new WaitForSeconds(0.25f);
        myRigidbody.gravityScale = gravityPower;
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        bodyCollider.enabled = true;
        standingVisual.enabled = true;
        ballVisual.enabled = false;
        isDashing = false;
        isRolling = false;
        canRoll = true;
        canMove = true;
    }

}
