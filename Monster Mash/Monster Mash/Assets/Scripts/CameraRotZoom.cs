using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraRotZoom : MonoBehaviour
{
    // public cursor playerCursor;
    private Vector3 lastMousePosition;

    [SerializeField] private CinemachineVirtualCamera _vCam;

    private Cinemachine3rdPersonFollow _3rdPersonFollow;

    private float zoomMin;
    private float zoomMax;

    private float startShoulderOffsetX = 0.0f;
    private float shoulderOffsetMin = 0.0f;
    private float shoulderOffsetMax = 0.0f;

    private float cameraDistanceX;

    private KeyCode camMove = KeyCode.Mouse2;

    private float rotSpeed = 5.0f;

    // PlayerControls playerControls;

    CinemachineComponentBase componentBase;
    float cameraDistance;
    float zoom_sensitivity = 100.0f;

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
            componentBase = _vCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
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
        cameraDistance = startCamDist;

        zoomMin = startCamDist - 7;
        zoomMax = startCamDist + 7;

        _3rdPersonFollow = _vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        startShoulderOffsetX = _3rdPersonFollow.ShoulderOffset.x;
        shoulderOffsetMin = startShoulderOffsetX + 3;
        shoulderOffsetMax = startShoulderOffsetX - 3;

        cameraDistanceX = startShoulderOffsetX;
    }

    void FixedUpdate()
    {
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
            _3rdPersonFollow.ShoulderOffset.x = startShoulderOffsetX;
            cameraDistance = startCamDist;
        }

        HandleCameraZoom();
    }

    private void correctMousePosition()
    {
        Mouse.current.WarpCursorPosition(lastMousePosition);
    }

    private void HandleCameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance += Input.mouseScrollDelta.y * 2.0f;
            cameraDistanceX -= Input.mouseScrollDelta.y;

            //Input.GetAxis("Mouse ScrollWheel") * zoom_sensitivity * Time.deltaTime;

            cameraDistance = Mathf.Clamp(cameraDistance, zoomMin, zoomMax);
            cameraDistanceX = Mathf.Clamp(cameraDistanceX, shoulderOffsetMax, shoulderOffsetMin);

            _3rdPersonFollow.CameraDistance = 
                Mathf.Lerp(_3rdPersonFollow.CameraDistance, cameraDistance,  zoom_sensitivity * Time.deltaTime);

            _3rdPersonFollow.ShoulderOffset.x =
                Mathf.Lerp(_3rdPersonFollow.ShoulderOffset.x, cameraDistanceX, zoom_sensitivity * Time.deltaTime);
        }
    }
}
