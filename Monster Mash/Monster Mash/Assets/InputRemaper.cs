using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class InputRemaper : MonoBehaviour
{

    private ControlItemData[] allControlItems;

    private void Start()
    {
        allControlItems = GetComponentsInChildren<ControlItemData>();

        LoadRebinds();

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
            .WithControlsHavingToMatchPath("<Gamepad>") // limits accepted inputs to gamepad buttons
            .WithCancelingThrough(Gamepad.current.buttonEast)
            .OnComplete(callback =>
            {
                SetButtonTextToDisplayString(controlItem, callback);
                var rebinds = controlItem.rebindTarget.action.actionMap.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString(controlItem.rebindTarget.action.actionMap.name, rebinds);
            })
            .OnCancel(callback => { SetButtonTextToDisplayString(controlItem, callback); })
            .Start();
    }


    private void LoadRebinds()
    {
        string lastActionMapName = "";

        foreach (ControlItemData controlItem in allControlItems)
        {
            string actionMapName = controlItem.rebindTarget.action.actionMap.name;
           
            if (lastActionMapName.Equals(actionMapName)) { return; }

            if (PlayerPrefs.HasKey(actionMapName))
            {
                string rebinds = PlayerPrefs.GetString(actionMapName);
                controlItem.rebindTarget.action.actionMap.LoadBindingOverridesFromJson(rebinds);
            }

            lastActionMapName = actionMapName;
        }
    }

    private void SetButtonTextToDisplayString(ControlItemData controlItem, InputActionRebindingExtensions.RebindingOperation callback = null)
    {
        // Rebind operation cleanup
        if (callback != null)
        {
            controlItem.rebindTarget.action.Enable();
            callback.Dispose();
        }

        controlItem.buttonRef.GetComponentInChildren<TextMeshProUGUI>().text = controlItem.rebindTarget.action.GetBindingDisplayString();
    }
}
