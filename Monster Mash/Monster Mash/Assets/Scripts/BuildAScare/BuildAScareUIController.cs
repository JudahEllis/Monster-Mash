using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildAScareUIController : MonoBehaviour
{
    PlayerInput input;

    [Header("Limb Selection UI")]

    [SerializeField]
    Animator cameraAnimator;

    [SerializeField]
    private CanvasGroup limbUI;

    EventSystem eventSystem;

    [SerializeField]
    Image[] partTabDots;

    [SerializeField]
    CanvasGroup[] partButtonLayouts;

    [SerializeField]
    Button[] partTabButtons;

    bool torsoActive = false;

    [SerializeField]
    int currentTab;
    void Start()
    {
        input = FindObjectOfType<PlayerInput>();

        eventSystem = FindObjectOfType<EventSystem>();

        BuildAScareManager.enableLimbUI += ShowUI;

        BuildAScareManager.disableLimbUI += HideUI;

        BuildAScareManager.activateTorso += TorsoActive;

        HideUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HideUI()
    {
        limbUI.alpha = 0;

        limbUI.blocksRaycasts = false;

        cameraAnimator.SetBool("limbUI", false);

        eventSystem.SetSelectedGameObject(null);

        for (int i = 0; i < partButtonLayouts.Length; i++)
        {
            partButtonLayouts[i].alpha = 0;

            partButtonLayouts[i].blocksRaycasts = false;

            partButtonLayouts[i].interactable = false;
        }

        if (input.devices[0] is Gamepad)
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }

    void ShowUI()
    {
        limbUI.alpha = 1;

        limbUI.blocksRaycasts = true;

        cameraAnimator.SetBool("limbUI", true);

        if (!torsoActive)
        {
            foreach (Image img in partTabDots)
            {
                img.color = Color.gray;
            }

            partTabDots[0].color = Color.white;

            LockButtons();
        }

        else
        {
            for (int i = 1; i < partTabDots.Length; i++)
            {
                if (i == currentTab)
                {
                    partTabDots[i].color = Color.white;

                    continue;
                }

                partTabDots[i].color = Color.black;
            }
        }

        for(int i = 0; i < partButtonLayouts.Length; i++)
        {
            partButtonLayouts[i].alpha = 0;

            partButtonLayouts[i].blocksRaycasts = false;

            partButtonLayouts[i].interactable = false;
        }

        partButtonLayouts[currentTab].alpha = 1;

        partButtonLayouts[currentTab].blocksRaycasts = true;

        partButtonLayouts[currentTab].interactable = true;

        if (input.devices[0] is Gamepad)
        {
            eventSystem.SetSelectedGameObject(partButtonLayouts[currentTab].transform.GetChild(0).gameObject);
        }

    }

    void LockButtons()
    {
        for (int i = 0; i < partTabButtons.Length; i++)
        {
            partTabButtons[i].interactable = false;
        }
    }

    void UnlockButtons()
    {
        for (int i = 1; i < partTabButtons.Length; i++)
        {
            partTabButtons[i].interactable = true;
        }
    }

    void TorsoActive()
    {
        torsoActive = true;

        HideUI();

        for(int i = 0; i < partTabDots.Length; i++)
        {
            if(i == 0)
            {
                partTabDots[i].color = Color.grey;

                continue;
            }

            partTabDots[i].color = Color.black;
        }

        UnlockButtons();

        currentTab = 1;
    }

    public void LimbTabButton(int tabNumber)
    {
        currentTab = tabNumber;

        for (int i = 0; i < partButtonLayouts.Length; i++)
        {
            partButtonLayouts[i].alpha = 0;

            partButtonLayouts[i].blocksRaycasts = false;

            partButtonLayouts[i].interactable = false;
        }

        partButtonLayouts[currentTab].alpha = 1;

        partButtonLayouts[currentTab].blocksRaycasts = true;

        partButtonLayouts[currentTab].interactable = true;

        for (int i = 1; i < partTabDots.Length; i++)
        {
            if (i == currentTab)
            {
                partTabDots[i].color = Color.white;

                continue;
            }

            partTabDots[i].color = Color.black;
        }

        if (input.devices[0] is Gamepad)
        {
            eventSystem.SetSelectedGameObject(partButtonLayouts[currentTab].transform.GetChild(0).gameObject);
        }
    }


    #region UI Functions

    //Multiplier parameter is either 1 or negative 1 and effects if buttons adds or subtracts to rotation

    //Rotation is iffy right now as it is not adding an even 45, it will be fixed

    /*
    
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
        if (currentlySelected != null)
        {
            Vector3 newScale = new Vector3(-currentlySelected.transform.localScale.x,
                currentlySelected.transform.localScale.y,
                currentlySelected.transform.localScale.z);

            currentlySelected.transform.localScale = newScale;

            if (currentlySelected.GetComponent<BuildAScareLimb>().flipped)
            {
                currentlySelected.GetComponent<BuildAScareLimb>().flipped = false;
            }

            else
            {
                currentlySelected.GetComponent<BuildAScareLimb>().flipped = true;
            }
        }
    }

    */

    public void NewPart()
    {

    }

    /*
    public void ScaleSlider()
    {
        int modifier;

        if (currentlySelected.GetComponent<BuildAScareLimb>().flipped)
        {
            modifier = -1;
        }

        else
        {
            modifier = 1;
        }

        currentlySelected.transform.localScale = new Vector3(scaleSlider.value * modifier, scaleSlider.value, scaleSlider.value);
    }

    */

    /*

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

    */

    #endregion

}
