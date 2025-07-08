using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class VirturalCursorControl : MonoBehaviour
{
    VirtualMouseInput cursor;
    void Start()
    {
        cursor = GetComponent<VirtualMouseInput>();
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
