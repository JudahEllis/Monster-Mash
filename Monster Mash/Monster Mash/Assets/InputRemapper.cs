using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class RedButtonWrapper
{
    public List<string> redButtons;
    public RedButtonWrapper()
    {
        redButtons = new List<string>();
    }
}


public class InputRemapper : MonoBehaviour
{

    private ControlItemData[] allControlItems;
    // using the input action name as a key to identify specific buttons when the game loads
    private RedButtonWrapper redButtonWrapper = new();
    private readonly string redButtonsKey = "redButtons";

    private void Start()
    {
        allControlItems = GetComponentsInChildren<ControlItemData>();

        if (PlayerPrefs.HasKey(redButtonsKey))
        {
            redButtonWrapper = JsonUtility.FromJson<RedButtonWrapper>(PlayerPrefs.GetString(redButtonsKey));
        }

        LoadRebinds();

        foreach (ControlItemData controlItem in allControlItems)
        {
            SetButtonTextToDisplayString(controlItem);

            if (redButtonWrapper.redButtons.Contains(controlItem.rebindTarget.action.name))
            {
                controlItem.buttonRef.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }
        }

        if (allControlItems != null && allControlItems.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(allControlItems[0].buttonRef.gameObject);
        }
    }

    // called from button event
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
            .OnPotentialMatch(callback =>
            {
                if (controlItem.rebindTarget.action.actionMap.name.Equals("XBOX")) { return; }

                CheckForButtonConflicts(controlItem, callback);
            })
            .Start();
    }

    private void CheckForButtonConflicts(ControlItemData controlItem, InputActionRebindingExtensions.RebindingOperation callback)
    {
        foreach (var binding in controlItem.rebindTarget.action.actionMap.bindings)
        {
            foreach (var candidate in callback.candidates)
            {
                // the path for candidates and the path for bindings are very anoyingly not the same except for the endings.
                // So we need to split the paths into seperate strings and then compare the last element of each array.
                var candidateStrings = candidate.path.Split("/");
                var bindingStrings = binding.path.Split("/");


                // ignore the action we are currentely rebinding when looking for conflicts
                if (binding.action.Equals(controlItem.rebindTarget.action.name)) { continue; }


                // ^1 == Length - 1
                if (candidateStrings[^1].Equals(bindingStrings[^1]))
                {
                    controlItem.buttonRef.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;

                    if (!redButtonWrapper.redButtons.Contains(controlItem.rebindTarget.action.name))
                    {
                        redButtonWrapper.redButtons.Add(controlItem.rebindTarget.action.name);
                        PlayerPrefs.SetString(redButtonsKey, JsonUtility.ToJson(redButtonWrapper));

                    }

                    return;
                }
                else
                {
                    controlItem.buttonRef.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

                    if (redButtonWrapper.redButtons.Contains(controlItem.rebindTarget.action.name))
                    {
                        redButtonWrapper.redButtons.Remove(controlItem.rebindTarget.action.name);
                        PlayerPrefs.SetString(redButtonsKey, JsonUtility.ToJson(redButtonWrapper));
                    }
                }
            }
        }
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
