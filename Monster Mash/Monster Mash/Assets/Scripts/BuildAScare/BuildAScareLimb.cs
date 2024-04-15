using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAScareLimb : MonoBehaviour
{
    bool isSelected = false;

     bool isMirror = false;

    PlayerInput input;

    InputAction moveLimb;

    CharacterController limbController;
    
    void Start()
    {
        SpawnObject();
    }

    public void SpawnObject()
    {
        isSelected = true;

        input = FindObjectOfType<PlayerInput>();

        moveLimb = input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part");

        limbController = GetComponent<CharacterController>();
    }


    void FixedUpdate()
    {
        MoveLimb();
    }

    void MoveLimb()
    {
        if(isSelected)
        {

            Vector3 xMovement = Camera.main.transform.right * moveLimb.ReadValue<Vector2>().x;

            Vector3 yMovement = new Vector3(0, moveLimb.ReadValue<Vector2>().y, 0);

            Vector3 movemnt = xMovement + yMovement;

            limbController.Move(movemnt * Time.deltaTime * 2);

        }
    }
}
