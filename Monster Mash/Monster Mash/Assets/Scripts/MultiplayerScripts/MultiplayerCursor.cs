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

    public int cursorIndex;

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

        spawnedPlayer.SwitchCurrentActionMap("UI Navagation");

        cursor = GetComponent<VirtualMouseInput>();

        InputActionAsset playerInput = player.actions;

        InputActionMap controllerMap = playerInput.FindActionMap("UI Navagation");

        InputAction selectAction = controllerMap.FindAction("Select Action - Generic Gamepad");

        InputAction deselectAction = controllerMap.FindAction("Deselect Action - Generic Gamepad");

        selectAction.started += SelectAction;

        deselectAction.started += DeselectCharacter;

        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

        movingCursor = transform.GetChild(0).gameObject;

        spawnedPlayer.onDeviceLost += DisableVisuals;

        spawnedPlayer.onDeviceRegained += EnableVisuals;
    }

    private void OnDisable()
    {
        player.onDeviceLost -= DisableVisuals;

        player.onDeviceRegained -= EnableVisuals;
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
        RectTransform rect = movingCursor.GetComponent<RectTransform>();

        Vector3 mousePoint = new Vector3(rect.localPosition.x, rect.localPosition.y, 10);

        Ray ray = Camera.main.ScreenPointToRay(mousePoint);

        int layerMask = 1 << 14;

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            hit.transform.gameObject.GetComponent<MultiplayerJoinManager.IQuickplayButtonable>().ButtonSelected(this);
        }
    }

    public void SelectCharacter(MonsterData data, GameObject selectedMonster)
    { 
        if (joinManager.charactersSelected < inputManager.playerCount && !selectedCharacter)
        {
            joinManager.charactersSelected++;

            joinManager.playerInfo[player.playerIndex].monster = data;

            joinManager.playerInfo[player.playerIndex].playerInput = player.gameObject;

            selectedCharacter = true;

            Transform monsterSpawn = joinManager.monsterSpawnPoints[player.playerIndex];

            Instantiate(selectedMonster, monsterSpawn);

            if (joinManager.charactersSelected == inputManager.playerCount)
            {
                print("Start Game!");

                joinManager.allowStartGame = true;
            }
        }
    }

    public void DeselectCharacter(InputAction.CallbackContext context)
    {
        if(selectedCharacter)
        {
            selectedCharacter = false;

            joinManager.allowStartGame = false;

            joinManager.charactersSelected--;

            joinManager.playerInfo[player.playerIndex].monster = null;

            joinManager.playerInfo[player.playerIndex].playerInput = null;

            GameObject previewChar = 
                joinManager.monsterSpawnPoints[player.playerIndex].transform.GetChild(0).gameObject;

            Destroy(previewChar);
        }
    }

    void DisableVisuals(PlayerInput input)
    {
        Image cursorVisual = GetComponentInChildren<Image>();

        cursorVisual.enabled = false;

        joinManager.playerTubes[input.playerIndex].SetActive(false);

        print(inputManager.playerCount);
    }

    void EnableVisuals(PlayerInput input)
    {
        Image cursorVisual = GetComponentInChildren<Image>();

        cursorVisual.enabled = true;

        joinManager.playerTubes[input.playerIndex].SetActive(true);

        print(inputManager.playerCount);
    }
}
