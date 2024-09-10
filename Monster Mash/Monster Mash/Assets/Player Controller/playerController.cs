using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerControls playerControlsMap;
    private InputActionMap startingActionMap;
    private InputActionMap UIcontrols;
    private bool UIcontrolsNeeded = true;
    private InputActionMap monsterControls;
    private bool monsterControlsNeeded = false;

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
        playerInput.SwitchCurrentActionMap(newActionMap);
    }
}
