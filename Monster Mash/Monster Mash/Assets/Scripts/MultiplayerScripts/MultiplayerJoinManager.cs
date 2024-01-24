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
    [SerializeField]
    private GameObject cursorParent;

    // The Colors that are mapped to the UI Elements to visually show which player they are
    [SerializeField]
    private Color[] playerTokenColors;

    [SerializeField]
    private List<VirtualMouseInput> playerCursors;

    private int charactersSelected;

    private bool allowStartGame = false;

    [SerializeField]
    private CanvasGroup characterSelectButtons;

    [SerializeField]
    private CanvasGroup stageSelectButtons;

    [System.Serializable]
    public class PlayerInformation
    {
        public int selectedCharIndex = -1;

        public int playerIndex;
    }

    [SerializeField]
    private List<PlayerInformation> playerInfo;

    void Awake()
    {
        playerInputController = FindObjectOfType<PlayerInputManager>();

        allowStartGame = false;
    }

    private void Start()
    {
        playerInputController.onPlayerJoined += AddPlayerToken;

        playerInputController.onPlayerJoined += AssignCursorControls;

        playerInputController.onPlayerJoined += AssignPlayerInformation;

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

        InputActionMap controllerMap = playerInput.FindActionMap("UI Navagation");

        VirtualMouseInput mouseUI = player.gameObject.GetComponent<VirtualMouseInput>();

        playerCursors.Add(mouseUI);

        InputAction moveAction = controllerMap.FindAction("Move Cursor  - Generic Gamepad");

        InputAction selectAction = controllerMap.FindAction("Select Action - Generic Gamepad");

        InputAction startGame = controllerMap.FindAction("Start Game - Generic Gamepad");

        InputActionProperty moveActionProperty = new InputActionProperty(moveAction);

        InputActionProperty selectActionProperty = new InputActionProperty(selectAction);

        mouseUI.stickAction = moveActionProperty;

        mouseUI.leftButtonAction = selectActionProperty;

        startGame.started += CharacterToStageSelect;
    }

    void AssignPlayerInformation(PlayerInput player)
    {
        PlayerInformation controllerInfo = new PlayerInformation();

        controllerInfo.playerIndex = player.playerIndex;

        playerInfo.Add(controllerInfo);
    }

    public void SelectCharacter(int charIndex)
    {
       if(charactersSelected < playerInputController.playerCount)
        {
            charactersSelected++;

            if(charactersSelected == playerInputController.playerCount)
            {
                print("Proceed");

                allowStartGame = true;
            }
        }
    }

    void CharacterToStageSelect(InputAction.CallbackContext context)
    {
        if(allowStartGame)
        {
            characterSelectButtons.alpha = 0;
            characterSelectButtons.blocksRaycasts = false;

            stageSelectButtons.alpha = 1;
            stageSelectButtons.blocksRaycasts = true;
        }
    }


}
