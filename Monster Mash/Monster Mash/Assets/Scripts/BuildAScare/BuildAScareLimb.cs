using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAScareLimb : MonoBehaviour
{
    public bool isSelected = false;

    bool isMirror = false;

    PlayerInput input;

    InputAction moveLimb;

    CharacterController limbController;

    [SerializeField]
    MonsterPart monsterPart;

    BuildAScareManager systemManager;

    int partIndex = -1;
    
    void Start()
    {

    }

    public void SpawnObject(string path)
    {
        monsterPart = new MonsterPart();

        isSelected = true;

        input = FindObjectOfType<PlayerInput>();

        moveLimb = input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part");

        input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started += PlacePart;

        limbController = GetComponent<CharacterController>();

        monsterPart.partPrefabPath = path;

        systemManager = FindObjectOfType<BuildAScareManager>();
    }

    void SelectObject()
    {
        moveLimb = input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part");

        input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started += PlacePart;

        isSelected = true;
    }

    void DeselectObject()
    {
        moveLimb = null;

        isSelected = false;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started -= PlacePart;

        SavePartData();
    }

    void PlacePart(InputAction.CallbackContext context)
    {
        moveLimb = null;

        isSelected = false;

        SavePartData();

        systemManager.monsterInformation.monsterParts.Add(monsterPart);

        partIndex = systemManager.monsterInformation.monsterParts.IndexOf(monsterPart);

        input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started -= PlacePart;

        systemManager.currentlySelected = null;
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

            Vector3 yMovement = Camera.main.transform.up * moveLimb.ReadValue<Vector2>().y;

            Vector3 movemnt = xMovement + yMovement;

            limbController.Move(movemnt * Time.deltaTime * 2);

        }
    }

    void SavePartData()
    {
        monsterPart.partRotation = transform.localRotation;

        monsterPart.partScale = transform.localScale;

        monsterPart.partPosition = transform.localPosition;

        if(partIndex != -1)
        {
            systemManager.monsterInformation.monsterParts[partIndex].partPosition = monsterPart.partPosition;

            systemManager.monsterInformation.monsterParts[partIndex].partScale = monsterPart.partScale;

            systemManager.monsterInformation.monsterParts[partIndex].partRotation = monsterPart.partRotation;
        }
    }
}
