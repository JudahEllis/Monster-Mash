using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public int playerIndex;
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
    Vector2 joystickVector; //gives us direction on x axis
    float joystickValue; //gives us nuance of input between magnitudes
    //
    private float walkSpeed = 10f;
    private float runSpeed = 40f;
    private float groundedModifer = 1;
    private float airbornModifer = 0.75f;
    private float currentGroundedStateModifier = 1;
    bool canMove = false;
    //
    bool grounded = false;
    bool canJump = true;
    bool jumpButtonReset = false;
    int numberOfJumps = 2;
    int numberOfJumpsLeft = 2;
    private Vector2 jumpValue;
    private bool jumpValueHasBeenRead = true;
    private float bigJumpPower = 80;
    private float littleJumpPower = 60;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerControlsMap = new PlayerControls();

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
            canMove = true;
        }
        else
        {
            myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            canMove = false;
        }
    }

    private void Update()
    {
        //This section moves the x axis of the player
        //For moving the y axis of the player, check out the jumping category of the movement section
        if (canMove)
        {

            if (isGrounded() && grounded == false)
            {
                land();
            }

            if (joystickValue >= 0.9f)
            {
                if (joystickVector.y >= 0.2f)
                {
                    //big jump
                    if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                    {
                        bigJump();
                    }
                }

                //run
                if (joystickVector.x > 0.2f)
                {
                    myRigidbody.velocity = new Vector2(1 * runSpeed, myRigidbody.velocity.y);
                }
                else if (joystickVector.x < -0.2f)
                {
                    myRigidbody.velocity = new Vector2(-1 * runSpeed, myRigidbody.velocity.y);
                }

            }
            else if (joystickValue > 0.2f)
            {
                if (joystickVector.y >= 0.2f && joystickVector.y < 0.5f)
                {
                    //little jump
                    if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                    {
                        littleJump();
                    }
                }
                else if (joystickVector.y >= 0.5f)
                {
                    //big jump
                    if (canJump && numberOfJumpsLeft > 0 && jumpButtonReset)
                    {
                        bigJump();
                    }
                }

                //walk
                if (joystickVector.x > 0.2f)
                {
                    myRigidbody.velocity = new Vector2(1 * walkSpeed, myRigidbody.velocity.y);
                }
                else if (joystickVector.x < -0.2f)
                {
                    myRigidbody.velocity = new Vector2(-1 * walkSpeed, myRigidbody.velocity.y);
                }

            }
            else
            {
                //myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            }

            if (joystickVector.y < 0.2f && jumpButtonReset == false)
            {
                jumpButtonReset = true;
            }

        }
    }

    #region Movement
    public void OnMove(InputAction.CallbackContext context)
    {
        //we need to determine if this is a walk or a jump
        //var joystickValue = context.ReadValue<Vector2>();
        joystickVector = context.ReadValue<Vector2>().normalized;
        joystickValue = context.ReadValue<Vector2>().magnitude;

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, solidGroundLayer);
    }

    private void land()
    {
        grounded = true;
        numberOfJumpsLeft = numberOfJumps;
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
        yield return new WaitForSeconds(0.1f);
        canJump = true;
    }

    #endregion

}
