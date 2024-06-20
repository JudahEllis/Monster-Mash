using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildAScareManager : MonoBehaviour
{
    PlayerInput input;

    InputAction rotateAction;

    InputAction zoomAction;

    [Header("General")]

    [SerializeField]
    Transform monsterEmpty;

    //[HideInInspector]
    public GameObject currentlySelected;
    
    [SerializeField]
    Transform monsterRotation;

    [SerializeField]
    CinemachineDollyCart rotCart;

    [SerializeField]
    CinemachineDollyCart zoomCart;

    bool monsterRotating = false;

    [SerializeField]
    Vector3 rotationVector;

    [Range(2,5)]
    public float cameraZoom = 5;

    [Header("MonsterData")]

    public MonsterData monsterInformation;

    public List<GameObject> monsterGameObjects;

    GameObject torsoObject;

    public int currentIndex;

    public Stack<UndoData> undoData;

    [Header("UI")]

    [SerializeField]
    Animator cameraAnimator;

    [SerializeField]
    private CanvasGroup limbUI;

    [SerializeField]
    CanvasGroup torsoButtons;

    [SerializeField]
    CanvasGroup armButtons;

    [SerializeField]
    CanvasGroup limbInfo;

    [SerializeField]
    Slider scaleSlider;

    EventSystem eventSystem;
    
    [SerializeField]
    GameObject buttonToSelect;

    [SerializeField]
    GameObject bodyButton;

    [SerializeField]
    GameObject armButton;

    [SerializeField]
    GameObject infoButton;

    void Awake()
    {
        input = FindObjectOfType<PlayerInput>();

        eventSystem = FindObjectOfType<EventSystem>();

        input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part").Enable();

        rotateAction = input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X");

        zoomAction = input.actions.FindActionMap("Build-A-Scare").FindAction("Camera Zoom");

        input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").Enable();

        zoomAction.Enable();

        input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").started += PauseSelection;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").canceled += UnpauseSelection;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Hide/Show UI").started += HideShowUI;

        input.actions.FindActionMap("Build-A-Scare").FindAction("PartIndexUp").canceled += PartIndexUp;

        input.actions.FindActionMap("Build-A-Scare").FindAction("PartIndexDown").canceled += PartIndexDown;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Select Part").performed += SelectMonsterPart;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Clear Parts").performed += ClearMonsterParts;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Undo Part").performed += Undo;

        undoData = new Stack<UndoData>();
    }

    private void OnDisable()
    {
        if(input != null)
        {
            input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part").Disable();

            input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").Disable();

            zoomAction.Disable();

            input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").started -= PauseSelection;

            input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").canceled -= UnpauseSelection;

            input.actions.FindActionMap("Build-A-Scare").FindAction("Hide/Show UI").started -= HideShowUI;

            input.actions.FindActionMap("Build-A-Scare").FindAction("PartIndexUp").canceled -= PartIndexUp;

            input.actions.FindActionMap("Build-A-Scare").FindAction("PartIndexDown").canceled -= PartIndexDown;

            input.actions.FindActionMap("Build-A-Scare").FindAction("Select Part").performed -= SelectMonsterPart;

            input.actions.FindActionMap("Build-A-Scare").FindAction("Clear Parts").performed -= ClearMonsterParts;

            input.actions.FindActionMap("Build-A-Scare").FindAction("Undo Part").performed -= Undo;

            undoData.Clear();
        }
    }

    void PauseSelection(InputAction.CallbackContext context)
    {
        if(currentlySelected != null)
        {
          currentlySelected.GetComponent<BuildAScareLimb>().isSelected = false;
        }

        monsterRotating = true;
    }

    void UnpauseSelection(InputAction.CallbackContext context)
    {
        if (currentlySelected != null)
        {
            currentlySelected.GetComponent<BuildAScareLimb>().isSelected = true;
        }

        monsterRotating = false;

        rotCart.m_Speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RotateMonster();

        ZoomCamera();
    }

    void RotateMonster()
    {
        if(monsterRotating)
        {
            Vector2 rotation = rotateAction.ReadValue<Vector2>();

            rotCart.m_Speed = rotation.x * 1;

            monsterRotation.LookAt(rotCart.gameObject.transform.position);
        }
    }
    void ZoomCamera()
    {
        float zoomPos = zoomAction.ReadValue<Vector2>().y;
        zoomCart.m_Speed = zoomPos * 1;
    }

    void PartIndexUp(InputAction.CallbackContext context)
    {
        if(currentlySelected == null)
        {
            currentIndex++;

            if (currentIndex > monsterGameObjects.Count - 1)
            {
                currentIndex = 0;
            }
        }
    }

    void PartIndexDown(InputAction.CallbackContext context)
    {
        if(currentlySelected == null)
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = monsterGameObjects.Count -1;
            }
        }
    }

    void SelectMonsterPart(InputAction.CallbackContext context)
    {
        if(uiHidden)
        {
            monsterGameObjects[currentIndex].GetComponent<BuildAScareLimb>().SelectObject();

            currentlySelected = monsterGameObjects[currentIndex];
        }
    }

   

    #region UI Functions

    //Multiplier parameter is either 1 or negative 1 and effects if buttons adds or subtracts to rotation

    //Rotation is iffy right now as it is not adding an even 45, it will be fixed
    public void XRotate(float multiplier)
    {
        if (currentlySelected != null)
        {
            rotationVector = new Vector3(currentlySelected.transform.rotation.x + (45 * multiplier), 
                currentlySelected.transform.rotation.y, currentlySelected.transform.rotation.z);

            currentlySelected.transform.Rotate(rotationVector, Space.Self);
        }
    }

    public void YRotate(float multiplier)
    {
        if (currentlySelected != null)
        {
            rotationVector = new Vector3(currentlySelected.transform.rotation.x,
                currentlySelected.transform.rotation.y + (45 * multiplier), currentlySelected.transform.rotation.z);

            currentlySelected.transform.Rotate(rotationVector, Space.Self);
        }
    }

    public void ZRotate(float multiplier)
    {
        if (currentlySelected != null)
        {
            rotationVector = new Vector3(currentlySelected.transform.rotation.x,
                currentlySelected.transform.rotation.y, currentlySelected.transform.rotation.z + (45 * multiplier));

            currentlySelected.transform.Rotate(rotationVector, Space.Self);
        }
    }

    public void FlipPart()
    {
        if(currentlySelected != null)
        {
            Vector3 newScale = new Vector3(-currentlySelected.transform.localScale.x,
                currentlySelected.transform.localScale.y,
                currentlySelected.transform.localScale.z);

            currentlySelected.transform.localScale = newScale;

            if(currentlySelected.GetComponent<BuildAScareLimb>().flipped)
            {
                currentlySelected.GetComponent<BuildAScareLimb>().flipped = false;
            }

            else
            {
                currentlySelected.GetComponent<BuildAScareLimb>().flipped = true;
            }
        }
    }

    public void NewPart()
    {
        if(currentlySelected == null)
        {
            SwitchUI(limbInfo, armButtons, armButton);
        }
    }

    public void ScaleSlider()
    {
        int modifier;

        if(currentlySelected.GetComponent<BuildAScareLimb>().flipped)
        {
            modifier = -1;
        }

        else
        {
            modifier = 1;
        }

        currentlySelected.transform.localScale = new Vector3(scaleSlider.value * modifier, scaleSlider.value, scaleSlider.value);
    }

    bool uiHidden = true;
    void HideShowUI(InputAction.CallbackContext context)
    {
        if (uiHidden)
        {
            uiHidden = false;

            limbUI.alpha = 1;

            limbUI.blocksRaycasts = true;

            cameraAnimator.SetBool("limbUI", true);

            if(input.devices[0] is Gamepad )
            {
                eventSystem.SetSelectedGameObject(buttonToSelect);
            }
        }

        else
        {
            uiHidden = true;

            limbUI.alpha = 0;

            limbUI.blocksRaycasts = false;

            cameraAnimator.SetBool("limbUI", false);

            eventSystem.SetSelectedGameObject(null);
        }
    }

    void HideUI()
    {
        uiHidden = true;

        limbUI.alpha = 0;

        limbUI.blocksRaycasts = false;

        cameraAnimator.SetBool("limbUI", false);

        eventSystem.SetSelectedGameObject(null);
    }

    void ShowUI()
    {
        uiHidden = false;

        limbUI.alpha = 1;

        limbUI.blocksRaycasts = true;

        cameraAnimator.SetBool("limbUI", true);

        if (input.devices[0] is Gamepad)
        {
            eventSystem.SetSelectedGameObject(buttonToSelect);
        }
    }

    void SwitchUI(CanvasGroup oldDisplay, CanvasGroup newDisplay, GameObject button)
    {
        buttonToSelect = button;

        oldDisplay.alpha = 0;

        oldDisplay.blocksRaycasts = false;

        newDisplay.alpha = 1;

        newDisplay.blocksRaycasts = true;

        if (input.devices[0] is Gamepad)
        {
            eventSystem.SetSelectedGameObject(button);
        }
    }

    #endregion

    public void SpawnTorso(string path)
    {
        GameObject torso = Resources.Load(path) as GameObject;

        GameObject spawnedTorso = Instantiate(torso, monsterEmpty);

        MonsterPartData torsoPart = new MonsterPartData();

        torsoPart.partPrefabPath = path;

        torsoPart.partPosition = spawnedTorso.transform.localPosition;

        torsoPart.partRotation = spawnedTorso.transform.localRotation;

        torsoPart.partScale = spawnedTorso.transform.localScale;

        monsterInformation.monsterParts.Add(torsoPart);

        torsoObject = spawnedTorso;

        SwitchUI(torsoButtons, armButtons, armButton);

        //Add Logic for Saving Torso as the First Part in the parts Array
    }

    public void SpawnMonsterLimb(string path)
    {
        GameObject limb = Resources.Load(path) as GameObject;

        GameObject spawnedLimb = Instantiate(limb, monsterEmpty);

        spawnedLimb.transform.localPosition = new Vector3(-2f, 0f, 0f);

        spawnedLimb.AddComponent<BuildAScareLimb>();

        spawnedLimb.AddComponent<CharacterController>();

        spawnedLimb.GetComponent<BuildAScareLimb>().SpawnObject(path);

        currentlySelected = spawnedLimb;

        SwitchUI(armButtons, limbInfo, infoButton);
    }

    void ClearMonsterParts(InputAction.CallbackContext context)
    {
        if (currentlySelected != null)
        {
            currentlySelected.GetComponent<BuildAScareLimb>().Despawn();

            currentlySelected = null;
        }

        foreach(GameObject obj in monsterGameObjects)
        {
            Destroy(obj);
        }

        Destroy(torsoObject.gameObject);

        monsterGameObjects.Clear();

        monsterInformation.monsterParts.Clear();

        undoData.Clear();

        SwitchUI(limbInfo, armButtons, armButton);

        SwitchUI(armButtons, torsoButtons, bodyButton);
    }

    public void Undo(InputAction.CallbackContext context)
    {
        if(undoData.Count > 0 && currentlySelected == null)
        {
            undoData.Peek().monsterObj.GetComponent<BuildAScareLimb>().UndoPart(undoData.Peek().partData);

            undoData.Pop();
        }
    }
}
