using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class monsterSelectManager : MonoBehaviour
{
    //public EventSystem eventHandler;
    public MultiplayerEventSystem[] eventHandler;
    private stageSelectManager stageSelect;

    [SerializeField]
    public PlayerInputManager playerInputManager;

    public GameObject monsterSelectPanel;
    public GameObject stageSelectPanel;
    public Animation camera_MonsterToStageTransition;
    public GameObject portal;


    public monsterAttackSystem[] tempMonsterLibrary;
    public monsterAttackSystem player1SelectedMonster;

    private void Awake()
    {
        stageSelect = FindObjectOfType<stageSelectManager>();
        tempMonsterLibrary = FindObjectsOfType<monsterAttackSystem>();
        establishMonsterConnections();
    }

    public void activateStageSelect()
    {
        //wait a few seconds to open the stage select
        playerInputManager.DisableJoining();
        StartCoroutine(portalDelay());
        //turn off monster select
        monsterSelectPanel.SetActive(false);
        //turn on portal
        portal.SetActive(true);
        //activate animation on camera
        camera_MonsterToStageTransition.Play();
    }

    private IEnumerator portalDelay()
    {
        yield return new WaitForSeconds(1);
        stageSelectPanel.SetActive(true);
        selectNewButton(stageSelectPanel);
    }

    public void cycleStageSelectionRight()
    {

    }

    public void cycleStageSelectionLeft()
    {

    }

    public void confirmStage()
    {

    }

    private void selectNewButton(GameObject menuPanel)
    {
        for (int i = 0; i < eventHandler.Length; i++)
        {
            eventHandler[i].enabled = false;
            Button firstButton = menuPanel.GetComponent<menuPanel>().firstButtonInPanel;
            eventHandler[i].firstSelectedGameObject = firstButton.gameObject;
            eventHandler[i].enabled = true;
            firstButton.Select();
        }
    }

    public void establishMonsterConnections()
    {
        for (int i = 0; i < tempMonsterLibrary.Length; i++)
        {
            tempMonsterLibrary[i].turnOnLimbConnectors();
        }
    }

    public void turnOnSelectedMonster(int monsterInLibrary)
    {
        monsterAttackSystem selectedMonster = tempMonsterLibrary[monsterInLibrary];
        playerController player1 = GameObject.Find("Player 1").GetComponent<playerController>();
        selectedMonster.transform.parent = player1.transform;
        player1.myMonster = selectedMonster;
        player1.myMonster.turnOffLimbConnectors();
        player1.myMonster.connectCurrentLimbs();
        player1.myMonster.awakenTheBeast();
    }
}
