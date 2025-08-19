using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class InputRemaper : MonoBehaviour
{

    [SerializeField] private ControlItemData[] allControlItems;

    private void Start()
    {
        foreach (ControlItemData controlItem in allControlItems)
        {
            SetButtonTextToDisplayString(controlItem);
        }

        if (allControlItems != null && allControlItems.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(allControlItems[0].buttonRef.gameObject);
        }
    }


    public void Rebind(ControlItemData controlItem)
    {
        controlItem.buttonRef.GetComponentInChildren<TextMeshProUGUI>().text = "";

       controlItem.rebindTarget.action.Disable();
        controlItem.rebindTarget.action.PerformInteractiveRebinding()
            .WithAction(controlItem.rebindTarget.action)
            .WithControlsHavingToMatchPath("<Gamepad>")
            .WithCancelingThrough(Gamepad.current.buttonEast)
            .OnComplete(callback => 
            {
                Debug.Log("Test");
                SetButtonTextToDisplayString(controlItem, callback); 
            })
            .OnCancel(callback => { SetButtonTextToDisplayString(controlItem, callback); })
            .Start();
    }

    private void SetButtonTextToDisplayString(ControlItemData controlItem, InputActionRebindingExtensions.RebindingOperation callback = null)
    {
        if (callback != null)
        {
            controlItem.rebindTarget.action.Enable();
            callback.Dispose();
        }

        controlItem.buttonRef.GetComponentInChildren<TextMeshProUGUI>().text = controlItem.rebindTarget.action.GetBindingDisplayString();
    }
}
