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

    [SerializeField]
    private List<VirtualMouseInput> playerCursors;

    [HideInInspector]
    public int charactersSelected;

    [HideInInspector]
    public bool allowStartGame = false;

    [SerializeField]
    private CanvasGroup characterSelectButtons;

    [SerializeField]
    private CanvasGroup stageSelectButtons;

    [System.Serializable]
    public class PlayerInformation
    {
        public GameObject selectedCharacter;

        public int playerIndex;

        public GameObject playerInput;
    }

    public List<PlayerInformation> playerInfo;

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
        
     
    }

    
    
    //Function is used to Controll the position of Player Cursors after they spawn
    //As of right now assigns a color value but that can be swapped out for a sprite at a later date
    void AddPlayerToken(PlayerInput player)
    {
        playerCursors[playerInputController.playerCount - 1].gameObject.SetActive(true);

        playerCursors[playerInputController.playerCount - 1].gameObject.GetComponent<MultiplayerCursor>().Enabled(player);

        player.gameObject.transform.position = Vector3.zero;

        allowStartGame = false;

    }

    //Assigns the newly Spawned in Cursors their respective Player's controller
    //Input System likes to have them all be controlled by one so this is the manual workarond
    void AssignCursorControls(PlayerInput player)
    {
        InputActionAsset playerInput = player.GetComponent<PlayerInput>().actions;

        InputActionMap controllerMap = playerInput.FindActionMap("UI Navagation");

        VirtualMouseInput mouseUI = playerCursors[playerInputController.playerCount - 1]; ;

        InputAction moveAction = controllerMap.FindAction("Move Cursor  - Generic Gamepad");

        InputAction selectAction = controllerMap.FindAction("Select Action - Generic Gamepad");

        InputAction startGame = controllerMap.FindAction("Start Game - Generic Gamepad");

        InputActionProperty moveActionProperty = new InputActionProperty(moveAction);

        mouseUI.stickAction = moveActionProperty;

        startGame.started += StartAction;
    }

    //Function to Assign Variables Needed when Loading into Scenes of Gameplay
    void AssignPlayerInformation(PlayerInput player)
    {
        player.gameObject.name = ("Player Input Controller" + playerInputController.playerCount);

        playerCursors[playerInputController.playerCount - 1].gameObject.GetComponent<MultiplayerCursor>().joinManager = this;

        playerCursors[playerInputController.playerCount - 1].gameObject.GetComponent<MultiplayerCursor>().inputManager = playerInputController;

        PlayerInformation controllerInfo = new PlayerInformation();

        controllerInfo.playerIndex = player.playerIndex;

        controllerInfo.playerInput = player.gameObject;

        playerInfo.Add(controllerInfo);
    }

    //Is Assigned to the Start Button, only works when all players have selected a charcater
    void StartAction(InputAction.CallbackContext context)
    { 
        if(allowStartGame)
        { 
            characterSelectButtons.alpha = 0;
            characterSelectButtons.blocksRaycasts = false;

            stageSelectButtons.alpha = 1;
            stageSelectButtons.blocksRaycasts = true;
        }
    }

    public interface IQuickplayButtonable
    {
        void ButtonSelected(MultiplayerCursor cursor);
    }
}
