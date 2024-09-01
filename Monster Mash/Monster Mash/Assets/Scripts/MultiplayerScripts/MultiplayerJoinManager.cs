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

    [SerializeField]
    public GameObject[] playerTubes;

    public List<Transform> monsterSpawnPoints;

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
        public MonsterData monster;

        public GameObject characterModel;

        public int playerIndex;

        public GameObject playerInput;
    }

    public List<PlayerInformation> playerInfo;

    void Awake()
    {
        playerInputController = FindObjectOfType<PlayerInputManager>();

        allowStartGame = false;

        PlayerInput[] inputs = FindObjectsOfType<PlayerInput>();

        foreach(VirtualMouseInput cursor in playerCursors)
        {
            MultiplayerCursor playerCursor = cursor.gameObject.GetComponent<MultiplayerCursor>();

            int index = playerCursor.cursorIndex;

            foreach(PlayerInput input in inputs)
            {
                if (index == input.playerIndex)
                {
                    AddPlayerToken(input);

                    AssignCursorControls(input);

                    AssignPlayerInformation(input);
                }
            }
            
        }
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
        playerCursors[player.playerIndex].gameObject.SetActive(true);

        playerCursors[player.playerIndex].gameObject.GetComponent<MultiplayerCursor>().Enabled(player);

        playerTubes[player.playerIndex].SetActive(true);

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
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 11, Camera.main.transform.position.z);
        }
    }

    public interface IQuickplayButtonable
    {
        void ButtonSelected(MultiplayerCursor cursor);
    }
}
