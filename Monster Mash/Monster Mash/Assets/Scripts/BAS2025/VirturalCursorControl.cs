using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VirturalCursorControl : MonoBehaviour
{
    VirtualMouseInput cursor;

    PlayerInput input;

    RectTransform rect;

    GraphicRaycaster graphicRaycaster;
    PointerEventData eventData = new PointerEventData(null);
    EventSystem eventSystem;
    void Start()
    {
        cursor = GetComponent<VirtualMouseInput>();

        input = FindObjectOfType<PlayerInput>();

        rect = GetComponent<RectTransform>();
    }

    public void EnableCursor()
    {
        InputActionMap dragAndDrop = input.actions.FindActionMap("BuildAScare-DragAndDrop");

        InputAction moveAction = dragAndDrop.FindAction("Cursor Movement");

        InputActionProperty moveActionProperty = new InputActionProperty(moveAction);

        cursor.stickAction = moveActionProperty;

        Image cursorVisual = GetComponentInChildren<Image>();

        cursorVisual.enabled = true;
    }

    public void DisableCursor()
    {
        Image cursorVisual = GetComponentInChildren<Image>();

        cursorVisual.enabled = false;

        InputActionProperty moveActionProperty = new InputActionProperty(null);

        cursor.stickAction = moveActionProperty;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 mousePos = cursor.virtualMouse.position.ReadValue();

        mousePos.x = Mathf.Clamp(mousePos.x, 0f, Screen.width);
        mousePos.y = Mathf.Clamp(mousePos.y, 0f, Screen.height);

        InputState.Change(cursor.virtualMouse.position, mousePos);
    }
}
