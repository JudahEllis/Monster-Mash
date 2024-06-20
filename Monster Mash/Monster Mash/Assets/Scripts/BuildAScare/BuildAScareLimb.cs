using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAScareLimb : MonoBehaviour
{
    public bool isSelected = false;

    [SerializeField]
    bool canBePlaced;

    public bool flipped = false;

    bool isMirror = false;

    PlayerInput input;

    InputAction moveLimb;

    CharacterController limbController;

    [SerializeField]
    MonsterPartData monsterPart;

    BuildAScareManager systemManager;

    int partIndex = -1;

    Transform attachPoint;

    void Start()
    {

    }

    public void SpawnObject(string path)
    {
        monsterPart = new MonsterPartData();

        isSelected = true;

        input = FindObjectOfType<PlayerInput>();

        moveLimb = input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part");

        input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started += PlacePart;

        limbController = GetComponent<CharacterController>();

        limbController.radius = 0.1f;

        limbController.height = 0.1f;

        limbController.detectCollisions = false;

        monsterPart.partPrefabPath = path;

        systemManager = FindObjectOfType<BuildAScareManager>();

        attachPoint = transform.GetChild(0);

        //This line is TEMPORARY only until proper mesh detection for placement can be found
        canBePlaced = true;

        UndoData currentStatus = new UndoData(transform.localPosition, transform.localScale, transform.localRotation, this.gameObject);

        systemManager.undoData.Push(currentStatus);
    }

    public void SelectObject()
    {
        moveLimb = input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part");

        input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started += PlacePart;

        isSelected = true;
    }

    public void DeselectObject()
    {
        moveLimb = null;

        isSelected = false;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started -= PlacePart;

        SavePartData();
    }

    void PlacePart(InputAction.CallbackContext context)
    {
        if(canBePlaced)
        {
            moveLimb = null;

            isSelected = false;

            SavePartData();

            input.actions.FindActionMap("Build-A-Scare").FindAction("Place Part").started -= PlacePart;

            systemManager.currentlySelected = null;
        }
    }

    public void Despawn()
    {
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        MoveLimb();

        //CalculatePlacement();
    }

    void CalculatePlacement()
    {
        if(isSelected)
        {
            RaycastHit hit;

            if(Physics.SphereCast(attachPoint.position, 0.1f, transform.up, out hit))
            {
                if(hit.transform.gameObject.CompareTag("Limb") && hit.transform.parent != this.gameObject)
                {
                    canBePlaced = true;
                }
            }
        }
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

        UndoData currentStatus = new UndoData(monsterPart.partPosition, monsterPart.partScale, monsterPart.partRotation, this.gameObject);

        systemManager.undoData.Push(currentStatus);

        if(partIndex != -1)
        {
            systemManager.monsterInformation.monsterParts[partIndex].partPosition = monsterPart.partPosition;

            systemManager.monsterInformation.monsterParts[partIndex].partScale = monsterPart.partScale;

            systemManager.monsterInformation.monsterParts[partIndex].partRotation = monsterPart.partRotation;
        }

        else
        {
            systemManager.monsterInformation.monsterParts.Add(monsterPart);

            systemManager.monsterGameObjects.Add(this.gameObject);

            partIndex = systemManager.monsterInformation.monsterParts.IndexOf(monsterPart);
        }
    }

    public void UndoPart(MonsterPartData partData)
    {
        transform.localPosition = partData.partPosition;

        transform.localScale = partData.partScale;

        transform.localRotation = partData.partRotation;
    }
}
