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
        /*
       foreach(MultiplayerJoinManager.PlayerInformation info in CharacterSelectManager.Instance.storedPlayerInformation)
        {
            int spawnIndex = CharacterSelectManager.Instance.storedPlayerInformation.IndexOf(info);
            SpawnPlayer(info.selectedCharacter, info.characterModel, playerSpawnLocations[spawnIndex], info.playerInput, info.playerIndex);
        }
        */

        FunTestSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlayer(GameObject playerChar, GameObject playerModel, Transform spawnPoint, GameObject playerInput, int index)
    {
        GameObject spawnedPlayer = Instantiate(playerChar, spawnPoint.position, Quaternion.identity);

        GameObject spawnedModel = Instantiate(playerModel, spawnedPlayer.transform);

        Vector3 charLocation = new Vector3(0, -0.4f, 0);

        spawnedModel.transform.localPosition = charLocation;

        PlayerInput[] allInputs = FindObjectsOfType<PlayerInput>();

        foreach(PlayerInput input in allInputs)
        {
            if(input.playerIndex == index)
            {
                spawnedPlayer.GetComponent<input_handler>().playerInput = input;
            }

        }

    }

    void FunTestSpawner()
    {
        MonsterTransfer transfer = FindObjectOfType<MonsterTransfer>();

        for(int i = 0; i < transfer.selectedMonsters.Count; i++)
        {
            foreach(MonsterPartData partData in transfer.selectedMonsters[i].monsterParts)
            {
                GameObject partPrefab = Resources.Load(partData.partPrefabPath) as GameObject;

                GameObject spawnedPart = Instantiate(partPrefab, playerSpawnLocations[i]);

                spawnedPart.transform.localPosition = partData.partPosition;

                spawnedPart.transform.localRotation = partData.partRotation;

                spawnedPart.transform.localScale = partData.partScale;
            }
        }
    }
}
