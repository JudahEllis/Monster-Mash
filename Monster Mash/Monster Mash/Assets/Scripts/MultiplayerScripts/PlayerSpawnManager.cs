using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawnLocations;

    [SerializeField]
    private Transform players;
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


        GameObject playerPrefab = Resources.Load("Fun-Test Parts/Monster/Player") as GameObject;

        GameObject monsterEmpty = Resources.Load("Fun-Test Parts/Monster/MonsterEmpty") as GameObject;

        for (int i = 0; i < transfer.selectedMonsters.Count; i++)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefab, playerSpawnLocations[i].position, Quaternion.identity);

            spawnedPlayer.transform.parent = players;

            Transform monsterPartStorage = spawnedPlayer.transform.GetChild(1);

            GameObject emptyMonster = Instantiate(monsterEmpty, monsterPartStorage);

            Transform monsterPartVisualsSpawn = emptyMonster.transform.GetChild(0).GetChild(0);

            monsterAttackSystem monsterControl = emptyMonster.GetComponent<monsterAttackSystem>();

            foreach (MonsterPartData partData in transfer.selectedMonsters[i].monsterParts)
            {
                GameObject partPrefab = Resources.Load(partData.partPrefabPath) as GameObject;

                GameObject spawnedPart = Instantiate(partPrefab, monsterPartVisualsSpawn);

                spawnedPart.transform.localPosition = partData.partPosition;

                spawnedPart.transform.localRotation = partData.partRotation;

                spawnedPart.transform.localScale = partData.partScale;
            }

            monsterControl.turnOnLimbConnectors();

            StartCoroutine(Delay(monsterControl));
        }
    }

    IEnumerator Delay(monsterAttackSystem monsterControl)
    {
        yield return new WaitForSeconds(0.1f);

        monsterControl.turnOffLimbConnectors();

        monsterControl.connectCurrentLimbs();

        monsterControl.awakenTheBeast();
    }
}
