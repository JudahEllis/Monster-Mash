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
    MultiplayerJoinManager joinManager;

    [SerializeField]
    CanvasGroup monsterSelectPanel;

    [SerializeField]
    CanvasGroup stageSelectPanel;

    public void StartGame()
    {
        playerInputManager.DisableJoining();

        joinManager.StartVersusMatch();
    }


    void ICursorSelectable.OnSelect(MultiplayerCursor cursor)
    {
        if(cursor.joinManager.allowStartGame)
        {
            StartGame();
        }
    }

    void ICursorSelectable.OnEnter(MultiplayerCursor cursor)
    {

    }

    void ICursorSelectable.OnExit(MultiplayerCursor cursor)
    {

    }
}
