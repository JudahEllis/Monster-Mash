using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BuildAScareManager : MonoBehaviour
{
    public static BuildAScareManager instance { get; private set; }
    enum CurrentBASScreen { StartScreen, PartSelector, PartPlacer, MenuHub}
    enum ControlMode { DirectControl, DragAndDrop}

    [Header("General")]

    CurrentBASScreen currentScreen = CurrentBASScreen.StartScreen;

    ControlMode controlMode;

    List<MonsterData> currentMonsters = new List<MonsterData>();

    EventSystem eventSystem;

    [Header("Start Screen")]

    [SerializeField]
    MonsterSelectIconBAS[] monsterSelectButtons;

    [SerializeField]
    CanvasGroup startScreenUI;

    [Header("Part Selector")]

    [SerializeField]
    Toggle[] partTypeSelectors;

    [SerializeField]
    CanvasGroup[] partTypeParent;

    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            instance = this;
        }

        eventSystem = FindObjectOfType<EventSystem>();


       for(int i = 0; i < monsterSelectButtons.Length; i++)
        {
            if(currentMonsters.Count >= 1 && currentMonsters[i] != null)
            {
                monsterSelectButtons[i].IconSetUp(true, currentMonsters[i]);
            }

            else
            {
                monsterSelectButtons[i].IconSetUp(false, null);
            }
        }
    }

    public void StartNewBuildAScare()
    {
        startScreenUI.alpha = 0;

        startScreenUI.interactable = false;

        eventSystem.SetSelectedGameObject(null);

        currentScreen = CurrentBASScreen.PartSelector;

        partTypeSelectors[4].GetComponent<Animator>().SetTrigger("Pressed");

        partTypeParent[4].interactable = true;

        partTypeParent[4].alpha = 1;

        GameObject firstSelectedTorso = partTypeParent[4].transform.GetChild(0).GetChild(0).gameObject;

        eventSystem.SetSelectedGameObject(firstSelectedTorso);
    }

    void SetControlMode(ControlMode mode)
    {
        switch(mode)
        {
            case ControlMode.DirectControl:



                break;

            case ControlMode.DragAndDrop:



                break;
        }
    }

    void ChangeControlMode(ControlMode mode)
    {
        switch (mode)
        {
            case ControlMode.DirectControl:



                break;

            case ControlMode.DragAndDrop:



                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
