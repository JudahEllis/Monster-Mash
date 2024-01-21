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
    public GameObject uiCanvas;

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

        playerInputController.onPlayerJoined += AssignTokenControls;

        print(playerInputController.playerCount);
    }

    // Update is called once per frame
    
    
    void LateUpdate()
    {
        foreach(VirtualMouseInput cursor in playerCursors)
        {
            Vector2 mousePos = cursor.virtualMouse.position.ReadValue();

            mousePos.x = Mathf.Clamp(mousePos.x, 0f, Screen.width);
            mousePos.y = Mathf.Clamp(mousePos.y, 0f, Screen.height);

            InputState.Change(cursor.virtualMouse.position, mousePos);
        }
    }
    
    

    void AddPlayerToken(PlayerInput player)
    {
        player.gameObject.transform.SetParent(uiCanvas.transform, false);

        player.gameObject.GetComponent<RectTransform>().position = Vector3.zero;

        player.gameObject.GetComponentInChildren<Image>().color = playerTokenColors[
            playerInputController.playerCount - 1];

        print(playerInputController.playerCount);
    }

    void AssignTokenControls(PlayerInput player)
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
