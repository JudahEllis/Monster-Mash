using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class input_handler : MonoBehaviour
{
    [SerializeField]
    private int playerIndex = 0;

    private PlayerInput playerInput;

    [Header("----------------Gamepad Controller Support----------------")]
    public List<availableControllerInputs> currentControllerMap;
    //here we would create a private list to hold edited controller maps before they are updated to default
    //here is where we would create a public array of texts from which the edited controller map derives its data from
    public List<availableControllerInputs> defaultControllerMap_Menu;
    public List<availableControllerInputs> defaultControllerMap_Combat;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        game_manager gameManager = GameObject.Find("Game Manager").GetComponent<game_manager>();
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

    //Establishes the correct button inputs available, whether in menus or combat and if a certain control set is being used
    #region Switching Input Actions and Action Maps

    public void controllerSetUp()
    {
        //switch input from starter to controller
        playerInput.SwitchCurrentActionMap("XBOX");
    }
    public void keyboardMouseSetUp()
    {
        //switch input from starter to keyboard/mouse
        playerInput.SwitchCurrentActionMap("keyboard/mouse");
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
        if (context.started)
        {
            Invoke(currentControllerMap[0].inputFunction, 0f);
        }
    }

    public void B_button(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[1].inputFunction, 0f);
        }
    }

    public void X_button(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[2].inputFunction, 0f);
        }
    }

    public void Y_button(CallbackContext context)
    {
        if (context.started)
        {
            Invoke(currentControllerMap[3].inputFunction, 0f);
        }
    }

    public void leftTrigger()
    {

    }

    public void rightTrigger()
    {

    }
    public void leftBumper()
    {

    }

    public void rightBumper()
    {

    }

    public void leftJoystick()
    {

    }

    public void rightJoystick()
    {

    }

    public void Dpad_UP()
    {

    }

    public void Dpad_DOWN()
    {

    }

    public void Dpad_LEFT()
    {

    }

    public void Dpad_RIGHT()
    {

    }
    #endregion

    //A translator for communication between what our player presses and what actions take place on screen
    #region Keyboard/Mouse Inputs

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
    #endregion

    //A library of combat functions called by inputs. Essentially all the tangible events
    #region Combat Interaction

    #endregion

}


//This is the magic that allows us to rename elements in lists
[System.Serializable]
public class availableControllerInputs
{
    public string inputName;
    public string inputFunction;
}


