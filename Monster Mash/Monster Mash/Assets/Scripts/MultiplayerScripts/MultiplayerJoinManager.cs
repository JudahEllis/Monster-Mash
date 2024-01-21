using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MultiplayerJoinManager : MonoBehaviour
{
    private PlayerInputManager playerInputController;

    // The Scene's UI Canvas that the Player UI Controllers are spawned under
    public GameObject cursorParent;

    // The Colors that are mapped to the UI Elements to visually show which player they are
    public Color[] playerTokenColors;

    [SerializeField]
    private List<VirtualMouseInput> playerCursors;

    void Awake()
    {
        playerInputController = FindObjectOfType<PlayerInputManager>();
    }

    private void Start()
    {
        playerInputController.onPlayerJoined += AddPlayerToken;

        playerInputController.onPlayerJoined += AssignCursorControls;

    }

    // Update is called once per frame
    
    
    void LateUpdate()
    {
        //Clamps the Player Cursors' Positions so that they do not go off screen

        foreach(VirtualMouseInput cursor in playerCursors)
        {
            Vector2 mousePos = cursor.virtualMouse.position.ReadValue();

            mousePos.x = Mathf.Clamp(mousePos.x, 0f, Screen.width);
            mousePos.y = Mathf.Clamp(mousePos.y, 0f, Screen.height);

            InputState.Change(cursor.virtualMouse.position, mousePos);
        }
    }
    
    
    //Function is used to Controll the position of Player Cursors after they spawn
    //As of right now assigns a color value but that can be swapped out for a sprite at a later date
    void AddPlayerToken(PlayerInput player)
    {
        player.gameObject.transform.SetParent(cursorParent.transform, false);

        player.gameObject.GetComponent<RectTransform>().position = Vector3.zero;

        player.gameObject.GetComponentInChildren<Image>().color = playerTokenColors[
            playerInputController.playerCount - 1];
    }

    //Assigns the newly Spawned in Cursors their respective Player's controller
    //Input System likes to have them all be controlled by one so this is the manual workarond
    void AssignCursorControls(PlayerInput player)
    {
        InputActionAsset playerInput = player.GetComponent<PlayerInput>().actions;

        InputActionMap moveStickMap = playerInput.FindActionMap("UI Navagation");

        VirtualMouseInput mouseUI = player.gameObject.GetComponent<VirtualMouseInput>();

        playerCursors.Add(mouseUI);

        InputAction moveAction = moveStickMap.FindAction("Move Cursor  - Generic Gamepad");

        InputActionProperty actionProperty = new InputActionProperty(moveAction);

        mouseUI.stickAction = actionProperty;
    }
}
