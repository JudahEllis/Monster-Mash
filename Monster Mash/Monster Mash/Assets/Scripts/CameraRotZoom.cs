using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraRotZoom : MonoBehaviour
{
    // public cursor playerCursor;
    private Vector3 lastMousePosition;

    [SerializeField] private CinemachineVirtualCamera cam;

    public float zoomMin = 80f;
    public float zoomMax = 120f;

    private KeyCode camMove = KeyCode.Mouse2;

    private float rotSpeed = 5.0f;

    // PlayerControls playerControls;

    CinemachineComponentBase componentBase;
    float cameraDistance;
    float zoom_sensitivity = 1000.0f;

    //publics
    [Header("Mouse")]
    float mouseYSensitivity = 1000.0f;
    float mouseXSensitivity = 700.0f;
    //Private
    float mouseX;
    float mouseY;
    float xRotation;
    float yRotation;

    float startXf;
    float startYf;

    private Quaternion targetXRot;
    private Quaternion targetYRot;

    private Quaternion startXRot;
    private Quaternion startYRot;

    private float startCamDist;

    bool zoomIn = false;
    bool zoomOut = false;

    [SerializeField] GameObject cursor_control;

    private void Start()
    {
        if(componentBase == null)
        {
            componentBase = cam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }

        //if (playerControls == null)
        //{
        //    playerControls = new PlayerControls();
        //    playerControls.PlayerMovement.TorsoZoomIn.performed += ctx => zoomIn = ctx.ReadValueAsButton();
        //    playerControls.PlayerMovement.TorsoZoomOut.performed += ctx => zoomOut = ctx.ReadValueAsButton();
        //}

        targetXRot = transform.localRotation;
        targetYRot = transform.localRotation;

        startXRot = transform.localRotation;
        startYRot = transform.localRotation;

        startXf = xRotation;
        startYf = yRotation;

        startCamDist = (componentBase as Cinemachine3rdPersonFollow).CameraDistance;
    }

    void FixedUpdate()
    {
        //if (Input.getm)
        if (Input.GetKeyDown(camMove))
        {
            //playerCursor.cameraRotating = true;
            lastMousePosition = Input.mousePosition;
        }

        //else if (Input.GetKeyUp(camMove))
        //{
        //    //Cursor.lockState = CursorLockMode.None;
        //    //correctMousePosition();
        //    //playerCursor.cameraRotating = false;
        //}

        if (Input.GetKey(camMove))
        {
            // cursor_control.GetComponent<cursor_limbplacer>().cameraRotating = true;
            //Cursor.lockState = CursorLockMode.Locked;
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

        else
        {
            //cursor_control.GetComponent<cursor_limbplacer>().cameraRotating = false;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance = Input.GetAxis("Mouse ScrollWheel") * zoom_sensitivity * Time.deltaTime;

            if (componentBase is Cinemachine3rdPersonFollow)
            {
                (componentBase as Cinemachine3rdPersonFollow).CameraDistance = 
                    Mathf.Clamp((componentBase as Cinemachine3rdPersonFollow).CameraDistance += cameraDistance, zoomMin, zoomMax);
            }
        }
    }

    private void Update()
    {
        // reset the transform 
        if (Input.GetKeyDown(KeyCode.B))
        {
            transform.rotation = Quaternion.Lerp(transform.localRotation, startYRot * startXRot, rotSpeed);
            targetYRot = startYRot;
            targetXRot = startXRot;
            yRotation = startYf;
            xRotation = startXf;
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = startCamDist;

            //transform.position = Vector3.Lerp(currPos, startPos, Time.deltaTime);
            //currPos = startPos;
            //targPos = startPos;
        }
    }

    private void correctMousePosition()
    {
        Mouse.current.WarpCursorPosition(lastMousePosition);
    }
}
