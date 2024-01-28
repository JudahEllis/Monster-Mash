using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MultiplayerCursor : MonoBehaviour
{
    PlayerInput player;

    VirtualMouseInput cursor;

    [HideInInspector]
    public MultiplayerJoinManager joinManager;

    [HideInInspector]
    public PlayerInputManager inputManager;

    bool selectedCharacter = false;

    GraphicRaycaster graphicRaycaster;
    PointerEventData eventData = new PointerEventData(null);

    GameObject movingCursor;
    

    // Start is called before the first frame update
    public void Enabled(PlayerInput spawnedPlayer)
    {
        player = spawnedPlayer;
        cursor = GetComponent<VirtualMouseInput>();

        InputActionAsset playerInput = player.actions;

        InputActionMap controllerMap = playerInput.FindActionMap("UI Navagation");

        InputAction selectAction = controllerMap.FindAction("Select Action - Generic Gamepad");

        selectAction.started += SelectAction;

        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

        movingCursor = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame

    private void Update()
    {
       
    }
    void LateUpdate()
    {
        Vector2 mousePos = cursor.virtualMouse.position.ReadValue();

        mousePos.x = Mathf.Clamp(mousePos.x, 0f, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0f, Screen.height);

        InputState.Change(cursor.virtualMouse.position, mousePos);
    }

    public void SelectAction(InputAction.CallbackContext context)
    {
        eventData.position = movingCursor.transform.position;

        List<RaycastResult> rayResults = new List<RaycastResult>();

        graphicRaycaster.Raycast(eventData, rayResults);

        if (rayResults.Count > 0)
        {
            GameObject button = rayResults[0].gameObject.transform.parent.gameObject;

            if(button.CompareTag("SelectionButton"))
            {
                button.GetComponent<MultiplayerJoinManager.IQuickplayButtonable>().ButtonSelected(this);
            }
        }
    }

    public void SelectCharacter(GameObject character)
    { 
        if (joinManager.charactersSelected < inputManager.playerCount && !selectedCharacter)
        {
            joinManager.charactersSelected++;

            joinManager.playerInfo[player.playerIndex].selectedCharacter = character;

            joinManager.playerInfo[player.playerIndex].playerInput = player.gameObject;

            selectedCharacter = true;

            if (joinManager.charactersSelected == inputManager.playerCount)
            {
                print("Start Game!");

                joinManager.allowStartGame = true;
            }
        }
    }

    public void SelectStage(int stageIndex)
    {
        SceneManager.LoadSceneAsync(stageIndex);

        
    }

    public void DeselectCharacter()
    {

    }
}
