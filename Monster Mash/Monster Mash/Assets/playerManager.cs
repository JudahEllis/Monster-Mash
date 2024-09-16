using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class playerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    public List<playerController> players = new List<playerController>();
    private List<string> playerNumbersAvailable = new List<string>() { "Player 1" , "Player 2", "Player 3", "Player 4"};

    public string intendedActionMap;

    [SerializeField]
    private Transform[] playerSpawnPoints;


    private void Awake()
    {
        playerInputManager = this.GetComponent<PlayerInputManager>();
    }

    private void OnPlayerJoined()
    {
        #region Rename, Add to Lists, and Player Order

        playerController[] potentialNewPlayer = FindObjectsOfType<playerController>();

        if (potentialNewPlayer.Length != players.Count) //only go through this process if its a new scene or a new player clone has been added
        {
            for (int i = 0; i < potentialNewPlayer.Length; i++)
            {
                if (players.Contains(potentialNewPlayer[i]) == false)//add to list of players in case its a new scene or this is a new player clone
                {
                    players.Add(potentialNewPlayer[i]);

                    if (playerNumbersAvailable.Contains(potentialNewPlayer[i].gameObject.name) == false) //rename if this is a new player clone
                    {
                        potentialNewPlayer[i].playerIndex = players.Count;
                        potentialNewPlayer[i].gameObject.name = "Player " + players.Count;
                    }
                }
            }

            //this is meant to reorder the list everytime a new player clone has been added or 
            for (int u = 0; u < potentialNewPlayer.Length; u++)
            {
                players[potentialNewPlayer[u].playerIndex - 1] = potentialNewPlayer[u];
            }

        }

        togglePlayerJoining();
        #endregion

        #region Correct Action Map

        if (intendedActionMap != null)
        {
            if (intendedActionMap != "")
            {
                switchAllPlayerActionMaps();
            }
        }

        #endregion

        #region Relocate Players
        //spawning players to designated spawn points if they exist
        if (playerSpawnPoints.Length > 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].gameObject.transform.position = playerSpawnPoints[i].position;
            }
        }
        #endregion

        #region Event System to Multiplayer Event System Conversion (if necessary)
        //this switches player event systems to determine whether one player controls the UI or all players control the UI
        if (players.Count > 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                GameObject potentialMultiplayerEventSystem = GameObject.Find("Player " + players[i].playerIndex + " Multiplayer System");

                if (potentialMultiplayerEventSystem != null)
                {
                    players[i].gameObject.GetComponent<PlayerInput>().uiInputModule = potentialMultiplayerEventSystem.GetComponent<InputSystemUIInputModule>();
                }
                else
                {
                    players[i].gameObject.GetComponent<PlayerInput>().uiInputModule = null;
                }
            }
        }
        #endregion
    }

    //This is for removing that dumb redundant unity error
    private void togglePlayerJoining()
    {
        if (playerInputManager != null)
        {
            if (players.Count >= playerInputManager.maxPlayerCount && playerInputManager.joiningEnabled)
            {
                playerInputManager.DisableJoining();
            }
            else if (players.Count < playerInputManager.maxPlayerCount && !playerInputManager.joiningEnabled)
            {
                playerInputManager.EnableJoining();
            }
        }
    }

    private void switchAllPlayerActionMaps()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].switchActionMap(intendedActionMap);
        }
    }

    private void OnPlayerLeft()
    {

    }
}
