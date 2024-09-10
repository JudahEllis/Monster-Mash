using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    public List<playerController> players = new List<playerController>();
    private List<string> playerNumbersAvailable = new List<string>() { "Player 1" , "Player 2", "Player 3", "Player 4"};


    private void Awake()
    {
        playerInputManager = this.GetComponent<PlayerInputManager>();
    }

    private void OnPlayerJoined()
    {
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
                        potentialNewPlayer[i].gameObject.name = "Player " + players.Count;
                    }
                }
            }
        }

        togglePlayerJoining();
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

    private void OnPlayerLeft()
    {

    }
}
