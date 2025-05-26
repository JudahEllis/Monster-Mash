using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharacterSelectReadyScript : MonoBehaviour, ICursorSelectable
{
    [SerializeField]
    public PlayerInputManager playerInputManager;

    [SerializeField]
    CanvasGroup monsterSelectPanel;

    [SerializeField]
    CanvasGroup stageSelectPanel;

    [SerializeField]
    Animation camera_MonsterToStageTransition;

    [SerializeField]
    GameObject portal;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateStageSelect()
    {
        //wait a few seconds to open the stage select
        playerInputManager.DisableJoining();
        StartCoroutine(PortalDelay());
        //turn off monster select
        monsterSelectPanel.alpha = 0;

        monsterSelectPanel.blocksRaycasts = false;

        monsterSelectPanel.interactable = false;

        //turn on portal
        portal.SetActive(true);
        //activate animation on camera
        camera_MonsterToStageTransition.Play();
    }

    private IEnumerator PortalDelay()
    {
        yield return new WaitForSeconds(1);

        stageSelectPanel.interactable = true;

        stageSelectPanel.blocksRaycasts = true;

        stageSelectPanel.alpha = 1;

    }

    void ICursorSelectable.OnSelect(MultiplayerCursor cursor)
    {
        if(cursor.joinManager.allowStartGame)
        {
            ActivateStageSelect();
        }
    }

    void ICursorSelectable.OnEnter(MultiplayerCursor cursor)
    {

    }

    void ICursorSelectable.OnExit(MultiplayerCursor cursor)
    {

    }
}
