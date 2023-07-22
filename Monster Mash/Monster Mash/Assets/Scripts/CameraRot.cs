using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraRot : MonoBehaviour
{
    public cursor playerCursor;
    private Vector3 lastMousePosition;

    private CinemachineVirtualCamera cam;

    private float yRotSpeed = 0.5f;

    private float xRotSpeed = 0.5f;

    private KeyCode camMove = KeyCode.Mouse2;

    private float rotSpeed = 5.0f;

    //publics
    [Header("Mouse")]
    float mouseYSensitivity = 1000.0f;
    float mouseXSensitivity = 700.0f;
    //Private
    float mouseX;
    float mouseY;
    float xRotation;
    float yRotation;

    private Quaternion targetXRot;
    private Quaternion targetYRot;

    private void Start()
    {
        targetXRot = transform.localRotation;
        targetYRot = transform.localRotation;

        cam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        //if (Input.getm)
        if (Input.GetKeyDown(camMove))
        {
            playerCursor.cameraRotating = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetKey(camMove))
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Data
            mouseX = Input.GetAxisRaw("Mouse X") * mouseYSensitivity * Time.deltaTime;
            mouseY = Input.GetAxisRaw("Mouse Y") * mouseXSensitivity * Time.deltaTime;
            //Rotation
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -65f, 65f);
            targetXRot = Quaternion.Euler(xRotation, 0f, 0f);

            yRotation += mouseX;
            targetYRot = Quaternion.Euler(0f, yRotation, 0f);

            transform.rotation = Quaternion.Lerp(transform.localRotation, targetYRot * targetXRot, rotSpeed);
        }
        else if(Input.GetKeyUp(camMove))
        {
            Cursor.lockState = CursorLockMode.None;
            correctMousePosition();
            playerCursor.cameraRotating = false;
        }
    }

    private void correctMousePosition()
    {
        Mouse.current.WarpCursorPosition(lastMousePosition);
    }
}
