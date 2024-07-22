using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System.Reflection;

public class input_handler : MonoBehaviour
{
    [SerializeField]
    private int playerIndex = 0;

    public PlayerInput playerInput;

    private InputAction movement;

    [Header("----------------Gamepad Controller Support----------------")]
    public List<availableControllerInputs> currentControllerMap;
    //here we would create a private list to hold edited controller maps before they are updated to default
    //here is where we would create a public array of texts from which the edited controller map derives its data from
    public List<availableControllerInputs> defaultControllerMap_Menu;
    public List<availableControllerInputs> defaultControllerMap_Combat;

    public List<availableKeyboardInputs> currentKeyboardMap;

    //Judah Added some items hehe
    [SerializeField] private Controller1 player;
    [SerializeField] private CustomCursor myCursor;
    private string controlType;
    private bool mapHasSwitched = false;

    private void Awake()
    {
        /*
        if (FindObjectOfType<Controller1>())
        {
            player = FindObjectOfType<Controller1>();
        }
        */

        player = GetComponent<Controller1>();

        //playerInput = GetComponent<PlayerInput>();
        //i changed this line because it was ruining my life
        //game_manager gameManager = GameObject.Find("Game Manager").GetComponent<game_manager>();
        game_manager gameManager = FindObjectOfType<game_manager>();
        gameManager.activePlayers.Add(this);
        for (int i = 0; i < gameManager.activePlayers.Count; i++)
        {
            if (gameManager.activePlayers[i] == this)
            {
                playerIndex = i;
                this.gameObject.name = "Player" + " " + (i + 1);
                break;
            }
        }
    }

    private void Start()
    {

        if (GetComponent<PlayerInput>().currentControlScheme == "Keyboard")//playerInput.devices[0].name.Contains("Keyboard"))
        {
            keyboardMouseSetUp();

            InputActionMap keyboardControls = playerInput.actions.FindActionMap(controlType);

            movement = keyboardControls.FindAction("Horizontal");

            movement.Enable();

            keyboardControls.FindAction("Spacebar").started += Spacebar_key;

            keyboardControls.FindAction("W").started += W_key;

            keyboardControls.FindAction("A").started += A_key;

            keyboardControls.FindAction("S").started += S_key;

            keyboardControls.FindAction("D").started += D_key;

            keyboardControls.FindAction("Q").started += Q_key;

            keyboardControls.FindAction("E").started += E_key;

            keyboardControls.FindAction("R").started += R_key;

            keyboardControls.FindAction("F").started += F_key;

            keyboardControls.FindAction("Left Shift").started += LeftShift_key;

            keyboardControls.FindAction("Left Control").started += LeftControl_key;

            keyboardControls.FindAction("Left Mouse").started += Left_mouse;

            keyboardControls.FindAction("Middle Mouse").started += Middle_mouse;

            keyboardControls.FindAction("Right Mouse").started += Right_mouse;

            keyboardControls.FindAction("numPad1").started += numPad_1;

            keyboardControls.FindAction("numPad2").started += numPad_2;

            keyboardControls.FindAction("numPad3").started += numPad_3;
        }

        else
        {
            controllerSetUp();

            InputActionMap controllerControls = playerInput.actions.FindActionMap(controlType);

            movement = controllerControls.FindAction("Left Stick");

            movement.Enable();

            controllerControls.FindAction("A Button").started += A_button;

            controllerControls.FindAction("B Button").started += B_button;

            controllerControls.FindAction("X Button").started += X_button;

            controllerControls.FindAction("Y Button").started += Y_button;

            controllerControls.FindAction("Left Trigger").started += leftTrigger;

            controllerControls.FindAction("Right Trigger").started += rightTrigger;

            controllerControls.FindAction("Left Bumper").started += leftBumper;

            controllerControls.FindAction("Right Bumper").started += rightBumper;

            controllerControls.FindAction("Right Stick").Enable();

            controllerControls.FindAction("DPad Up").started += Dpad_UP;

            controllerControls.FindAction("DPad Down").started += Dpad_DOWN;

            controllerControls.FindAction("DPad Left").started += Dpad_LEFT;

            controllerControls.FindAction("DPad Right").started += Dpad_RIGHT;

            controllerControls.FindAction("LeftStickClick").started += leftJoyStickClick;
        }

        print(controlType);
    }

    /* On Disable

    private void OnDisable()
    {
        if (controlType == "keyboardmouse")
        {
            InputActionMap keyboardControls = playerInput.actions.FindActionMap(controlType);

            movement = keyboardControls.FindAction("Horizontal");

            movement.Disable();

            keyboardControls.FindAction("Spacebar").started -= Spacebar_key;

            keyboardControls.FindAction("W").started -= W_key;

            keyboardControls.FindAction("A").started -= A_key;

            keyboardControls.FindAction("S").started -= S_key;

            keyboardControls.FindAction("D").started -= D_key;

            keyboardControls.FindAction("Q").started -= Q_key;

            keyboardControls.FindAction("E").started -= E_key;

            keyboardControls.FindAction("R").started -= R_key;

            keyboardControls.FindAction("F").started -= F_key;

            keyboardControls.FindAction("Left Shift").started -= LeftShift_key;

            keyboardControls.FindAction("Left Control").started -= LeftControl_key;

            keyboardControls.FindAction("Left Mouse").started -= Left_mouse;

            keyboardControls.FindAction("Middle Mouse").started -= Middle_mouse;

            keyboardControls.FindAction("Right Mouse").started -= Right_mouse;

            keyboardControls.FindAction("numPad1").started -= numPad_1;

            keyboardControls.FindAction("numPad2").started -= numPad_2;

            keyboardControls.FindAction("numPad3").started -= numPad_3;
        }

        else
        { 
            InputActionMap controllerControls = playerInput.actions.FindActionMap(controlType);

            movement = controllerControls.FindAction("Left Stick");

            movement.Disable();

            controllerControls.FindAction("A Button").started -= A_button;

            controllerControls.FindAction("B Button").started -= B_button;

            controllerControls.FindAction("X Button").started -= X_button;

            controllerControls.FindAction("Y Button").started -= Y_button;

            controllerControls.FindAction("Left Trigger").started -= leftTrigger;

            controllerControls.FindAction("Right Trigger").started -= rightTrigger;

            controllerControls.FindAction("Left Bumper").started -= leftBumper;

            controllerControls.FindAction("Right Bumper").started -= rightBumper;

            controllerControls.FindAction("Right Stick").Disable();

            controllerControls.FindAction("DPad Up").started -= Dpad_UP;

            controllerControls.FindAction("DPad Down").started -= Dpad_DOWN;

            controllerControls.FindAction("DPad Left").started -= Dpad_LEFT;

            controllerControls.FindAction("DPad Right").started -= Dpad_RIGHT;
        }

    }

    */
    private void FixedUpdate()
    {
        //movePlayer();
    }

    //Establishes the correct button inputs available, whether in menus or combat and if a certain control set is being used
    #region Switching Input Actions and Action Maps

    public void controllerSetUp()
    {
        if (!mapHasSwitched)
        {
            mapHasSwitched = true;
            //print("current map: " + playerInput.currentActionMap);
            //switch input from starter to controller
            //playerInput.SwitchCurrentActionMap("XBOX");
            controlType = "XBOX";
        }
    }
    public void keyboardMouseSetUp()
    {
        if (!mapHasSwitched)
        {
            mapHasSwitched = true;
            //print("trace: " + UnityEngine.StackTraceUtility.ExtractStackTrace());
            //switch input from starter to keyboard/mouse
            //playerInput.SwitchCurrentActionMap("keyboardmouse");
            controlType = "keyboardmouse";
            //print("current map: " + playerInput.currentActionMap);
        }
    }

    public void controller_SwitchToMenuActions()
    {
        for (int i = 0; i < defaultControllerMap_Menu.Count; i++)
        {
            currentControllerMap[i].inputFunction = defaultControllerMap_Menu[i].inputFunction;
        }
    }

    public void controller_SwitchToCombatActions()
    {
        for (int i = 0; i < defaultControllerMap_Combat.Count; i++)
        {
            currentControllerMap[i].inputFunction = defaultControllerMap_Combat[i].inputFunction;
        }
    }
    #endregion

    //A translator for communication between what our player presses and what actions take place on screen
    #region Gamepad Inputs

    public void A_button(CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Invoke(currentControllerMap[0].inputFunction, 0f);

            aButton = true;
        }
        else
        {
            aButton = false;
        }
    }

    public void B_button(CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Invoke(currentControllerMap[1].inputFunction, 0f);

            bButton = true;
        }
        else
        {
            bButton = false;
        }
    }

    public void X_button(CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Invoke(currentControllerMap[2].inputFunction, 0f);

            xButton = true;
        }
        else
        {
            xButton = false;
        }
    }

    public void Y_button(CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Invoke(currentControllerMap[3].inputFunction, 0f);

            yButton = true;
        }
        else
        {
            yButton = false;
        }
    }

    public void leftTrigger(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[4].inputFunction, 0f);
        }
    }

    public void rightTrigger(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[5].inputFunction, 0f);
        }
    }
    public void leftBumper(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[6].inputFunction, 0f);
        }
    }

    public void rightBumper(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[7].inputFunction, 0f);
        }
    }

    public void leftJoystick(CallbackContext context)
    {
        MethodInfo methodInfo = GetType().GetMethod(currentControllerMap[8].inputFunction, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        //if (methodInfo != null || context != null)
        //{
            //methodInfo.Invoke(this, new object[] { context });

            leftStick = context.ReadValue<Vector2>();
        //}
        //else
        //{
            //leftStick = new Vector2();
        //}
    }

    public void rightJoystick(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[9].inputFunction, 0f);
        }
    }

    public void Dpad_UP(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[10].inputFunction, 0f);
        }
    }

    public void Dpad_DOWN(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[11].inputFunction, 0f);
        }
    }

    public void Dpad_LEFT(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[12].inputFunction, 0f);
        }
    }

    public void Dpad_RIGHT(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[13].inputFunction, 0f);
        }
    }

    public void leftJoyStickClick(CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Invoke(currentControllerMap[14].inputFunction, 0f);
            leftStickClick = true;
        }
        else
        {
            leftStickClick = false;
        }
    }
    #endregion

    #region Judah's Silly Input Vars hehehaha

    [SerializeField] private bool aButton = false;
    [SerializeField] private bool bButton = false;
    [SerializeField] private bool xButton = false;
    [SerializeField] private bool yButton = false;
    [SerializeField] private Vector2 leftStick = new Vector2();
    private bool leftStickClick = false;

    #endregion

    //A translator for communication between what our player presses and what actions take place on screen
    #region Keyboard/Mouse Inputs

    public void W_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[0].inputFunction, 0f);
        }
    }

    public void Horizontal_key(CallbackContext context) //A + D = horizontal axis
    {
        MethodInfo methodInfo = GetType().GetMethod(currentKeyboardMap[1].inputFunction, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (methodInfo != null)
        {
            methodInfo.Invoke(this, new object[] { context });
        }
    }

    public void A_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[2].inputFunction, 0f);
        }
    }

    public void S_key(CallbackContext context)
    {
        MethodInfo methodInfo = GetType().GetMethod(currentKeyboardMap[3].inputFunction, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (methodInfo != null)
        {
            methodInfo.Invoke(this, new object[] { context });
        }
    }

    public void D_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[4].inputFunction, 0f);
        }
    }

    public void Spacebar_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[5].inputFunction, 0f);
        }
    }

    public void Q_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[6].inputFunction, 0f);
        }
    }

    public void E_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[7].inputFunction, 0f);
        }
    }
    public void R_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[8].inputFunction, 0f);
        }
    }

    public void F_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[9].inputFunction, 0f);
        }
    }
    public void LeftShift_key(CallbackContext context)
    {
        MethodInfo methodInfo = GetType().GetMethod(currentKeyboardMap[10].inputFunction, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (methodInfo != null)
        {
            methodInfo.Invoke(this, new object[] { context });
        }
    }

    public void LeftControl_key(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[11].inputFunction, 0f);
        }
    }

    public void Left_mouse(CallbackContext context)
    {
        MethodInfo methodInfo = GetType().GetMethod(currentKeyboardMap[12].inputFunction, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (methodInfo != null)
        {
            methodInfo.Invoke(this, new object[] { context });
        }
    }

    public void Middle_mouse(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[13].inputFunction, 0f);
        }
    }

    public void Right_mouse(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[14].inputFunction, 0f);
        }
    }

    public void Move_mouse(CallbackContext context)
    {
        MethodInfo methodInfo = GetType().GetMethod(currentKeyboardMap[15].inputFunction, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (methodInfo != null)
        {
            methodInfo.Invoke(this, new object[] { context });
        }
    }

    public void numPad_1(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[16].inputFunction, 0f);
        }
    }

    public void numPad_2(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[17].inputFunction, 0f);
        }
    }

    public void numPad_3(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentKeyboardMap[18].inputFunction, 0f);
        }
    }

    /*public void Vertical_key(CallbackContext context) //A + D = horizontal axis
    {
        MethodInfo methodInfo = GetType().GetMethod(currentKeyboardMap[19].inputFunction, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (methodInfo != null)
        {
            methodInfo.Invoke(this, new object[] { context });
        }
    }*/
    #endregion

    //A library of menu functions called by inputs. Essentially all the tangible events
    #region Menu Interaction
    public void sayHi()
    {
        print("hello!");
    }

    public void sayBye()
    {
        print("goodbye!");
    }

    public void moveCursor(CallbackContext context)
    {
        if (controlType == "XBOX")
        {
            Vector2 moveInput = context.ReadValue<Vector2>().normalized;

            if (GetComponent<CustomCursor>())
            {
                GetComponent<CustomCursor>().MoveCursor(moveInput);
            }
        }
        else if (controlType == "keyboardmouse")
        {
            Vector2 moveInput = context.ReadValue<Vector2>().normalized / 2;

            if (GetComponent<CustomCursor>())
            {
                GetComponent<CustomCursor>().MoveCursor(moveInput);
            }
        }
    }

    public void selectButton(CallbackContext context)
    {
        if (context.canceled)
        {
            myCursor.SetButtonHeld(false);
        }
        else if (context.started)
        {
            myCursor.SetButtonHeld(true);
            myCursor.Select();
        }
    }
    #endregion

    //A library of combat functions called by inputs. Essentially all the tangible events
    #region Combat Interaction
    //return functions to be accessed externally from Judah's movement controller script!
    public bool GetA_Button()
    {
        return aButton;
    }

    public bool GetB_Button()
    {
        return bButton;
    }

    public bool GetX_Button()
    {
        return xButton;
    }

    public bool GetY_Button()
    {
        return yButton;
    }

    public Vector2 GetLeft_JoyStick()
    {
        return leftStick;
    }

    public bool GetLeft_Joystick_Click()
    {
        return leftStickClick;
    }

    /*private void runPlayer(CallbackContext context)
    {
        print("runninging");
        player.SetIsRun(context.performed);
    }
    private void movePlayer() //with left stick, move player either right or left
    {
        if (controlType == "XBOX")
        {
            int dir = 0;

            Vector2 moveInput = movement.ReadValue<Vector2>();

            print("moving" + dir);

            if (moveInput.x < 0)
            {
                dir = -1;
            }
            else if (moveInput.x > 0)
            {
                dir = 1;
            }

            player.Move(dir);
        }*/
    #endregion

}


//This is the magic that allows us to rename elements in lists
[System.Serializable]
public class availableControllerInputs
{
    public string inputName;
    public string inputFunction;
}

[System.Serializable]

public class availableKeyboardInputs
{
    public string inputName;
    public string inputFunction;
}


