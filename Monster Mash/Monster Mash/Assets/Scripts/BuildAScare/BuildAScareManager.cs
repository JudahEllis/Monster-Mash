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

    [Header("General")]

    [SerializeField]
    Transform monsterEmpty;

    [HideInInspector]
    public GameObject currentlySelected;
    
    [SerializeField]
    Transform monsterRotation;

    [SerializeField]
    CinemachineDollyCart cart;

    bool monsterRotating = false;

    [SerializeField]
    Vector3 rotationVector;

    [Header("MonsterData")]

    public MonsterData monsterInformation;

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

        input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").Enable();

        input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").started += PauseSelection;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").canceled += UnpauseSelection;

        input.actions.FindActionMap("Build-A-Scare").FindAction("Hide/Show UI").started += HideShowUI;
    }

    private void OnDisable()
    {
        if(input != null)
        {
            input.actions.FindActionMap("Build-A-Scare").FindAction("Move Part").Disable();

            input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").Disable();

            input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").started -= PauseSelection;

            input.actions.FindActionMap("Build-A-Scare").FindAction("Rotate Character X").canceled -= UnpauseSelection;

            input.actions.FindActionMap("Build-A-Scare").FindAction("Hide/Show UI").started -= HideShowUI;
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

        cart.m_Speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RotateMonster();
    }

    void RotateMonster()
    {
        if(monsterRotating)
        {
            Vector2 rotation = rotateAction.ReadValue<Vector2>();

            cart.m_Speed = rotation.x * 1;

            monsterRotation.LookAt(cart.gameObject.transform.position);
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

    public void ScaleSlider()
    {
        currentlySelected.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
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

        Instantiate(torso, monsterEmpty);

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
}
