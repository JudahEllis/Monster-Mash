using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawnLocations;
    void Start()
    {
       foreach(MultiplayerJoinManager.PlayerInformation info in CharacterSelectManager.Instance.storedPlayerInformation)
        {
            int spawnIndex = CharacterSelectManager.Instance.storedPlayerInformation.IndexOf(info);
            SpawnPlayer(info.selectedCharacter, playerSpawnLocations[spawnIndex], info.playerInput, info.playerIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlayer(GameObject playerChar, Transform spawnPoint, GameObject playerInput, int index)
    {
        GameObject spawnedPlayer = Instantiate(playerChar, spawnPoint.position, Quaternion.identity);

        PlayerInput[] allInputs = FindObjectsOfType<PlayerInput>();

        foreach(PlayerInput input in allInputs)
        {
            if(input.playerIndex == index)
            {
                spawnedPlayer.GetComponent<input_handler>().playerInput = input;
            }

        }

    }
}
